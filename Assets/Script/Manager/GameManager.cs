using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public ScoreBoardManager scoreManager;
    public static float xLimit = 51.2f;
    public static float yLimit = 35.2f;
    public static System.Random rnd = new System.Random();
    int _nDeath;
    List<int> _nDeathPerLevel;
    int _nDeathEndGame = 10;
    int _nRestart;
    List<float> _timePerLevelList;
    float _time;
    List<string> _themeOfLevelList;
    List<string> _allThemesList;
    List<int> _satisfactionPerLevelList;
    List<GameObject> _currentFireBall;
    public Vector3 lastStartPosition;
    bool _inGame;
    int _level;
    int _nReachedEndPointInCurrentLevel;
    public Transform player;
    public GameObject endPoint;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        scoreManager = GetComponent<ScoreBoardManager>();
        if (scoreManager == null)
        {
            scoreManager = gameObject.AddComponent<ScoreBoardManager>();
        }
        DontDestroyOnLoad(this);
        

    }

    // Use this for initialization
    void Start () {
        //NewGame();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
	
	// Update is called once per frame
	void Update () {
		if(_inGame)
        {
            //_timePerLevelList[_level] += Time.deltaTime;
            _time += Time.deltaTime;
            if(Input.GetKeyDown("r"))
            {
                Restart();
            }
        }
	}

    public void InitializeThemes()
    {
        _allThemesList = new List<string>() { "Social Media", "Loneliness", "Friends", "Family", "Learning",
            "Happiness", "Hunger", "Texting", "Time", "Aging" };
    }

    public void NewGameButton()
    {
        print("New game button");
        SceneManager.LoadScene("Game");
    }

    public void NewGame()
    {
        
        InitializeThemes();
        player = GameObject.Find("Player").transform;
        _nDeath = 0;
        _nRestart = 0;
        scoreManager.totalReachedEndPoint = 0;
        _nDeathPerLevel = new List<int>();
        _satisfactionPerLevelList = new List<int>();
        _themeOfLevelList = new List<string>();
        _timePerLevelList = new List<float>();
        lastStartPosition = Vector3.zero;
        _level = 0;
        _nReachedEndPointInCurrentLevel = 0;
        _time = 0;

        System.Random rdn = new System.Random();
        while(_allThemesList.Count > 0)
        {
            int i = rdn.Next(0, _allThemesList.Count);
            _themeOfLevelList.Add(_allThemesList[i]);
            _allThemesList.Remove(_allThemesList[i]);
            //_timePerLevelList.Add(0);
            

        }
        if(endPoint == null)
        {
            endPoint = Instantiate(Resources.Load("EndPoint"), new Vector2(xLimit - (3.2f * 4), yLimit - (3.2f * 4)), Quaternion.identity) as GameObject;
        }

        //test
        GenerateFireBall();
        LaunchFireBall();
        //
        

        _inGame = true;
    }

    public void EndGame()
    {
        _inGame = false;
        _timePerLevelList.Add(_time);
        _nDeathPerLevel.Add(_nDeath);
        
        scoreManager.totalRestart = _nRestart;
        scoreManager.AverageTimePerLevel = 0;
        scoreManager.TotalTimeinGame = 0;
        foreach (float t in _timePerLevelList)
        {
            scoreManager.AverageTimePerLevel += t;
            scoreManager.TotalTimeinGame +=t;
        }
        scoreManager.AverageTimePerLevel /= _timePerLevelList.Count;

        scoreManager.averageDeathPerLevel = 0;
        scoreManager.totalDeath = 0;
        foreach(int d in _nDeathPerLevel)
        {
            scoreManager.averageDeathPerLevel += d;
            scoreManager.totalDeath += d;
        }
        scoreManager.averageDeathPerLevel /= _nDeathPerLevel.Count;

        print("Text" + scoreManager.AverageTimePerLevel + " " + scoreManager.totalReachedEndPoint);
        SceneManager.LoadScene("EndGame");
    }

    /// <summary>
    /// Which cadran is the position X,Y: Positive, Negative
    /// </summary>
    public enum Cadran
    {
        PP,NP,NN,PN
    }

    public int CadranToInt(Cadran c)
    {
        if (c == Cadran.PP)
            return 0;
        else if (c == Cadran.NP)
            return 1;
        else if (c == Cadran.NN)
            return 2;
        else // PN
            return 3;
    }
    public Cadran IntToCadran(int i)
    {
        if (i == 0)
            return Cadran.PP;
        else if (i == 1)
            return Cadran.NP;
        else if (i == 2)
            return Cadran.NN;
        else // PN
            return Cadran.PN;
    }

    public Cadran WhichCadran(Vector2 pos)
    {
        if(pos.x >0)
        {
            if(pos.y >0)
            {
                return Cadran.PP;
            }
            else
            {
                return Cadran.PN;
            }
        }
        else
        {
            if(pos.y>0)
            {
                return Cadran.NP;
            }
            else
            {
                return Cadran.NN;
            }
        }
    }

    public Vector2 GeneratePositionInCadran(Cadran c)
    {

        if (c == Cadran.PP)
        {
            return new Vector2(rnd.Next(0, (int)(xLimit/3.2))*3.2f, rnd.Next(0, (int)(yLimit / 3.2)) * 3.2f);
        }
        if (c == Cadran.PN)
        {
            return new Vector2(rnd.Next(0, (int)(xLimit / 3.2)) * 3.2f, -rnd.Next(0, (int)(yLimit / 3.2)) * 3.2f);
        }
        if (c == Cadran.NN)
        {
            return new Vector2(-rnd.Next(0, (int)(xLimit / 3.2)) * 3.2f, -rnd.Next(0, (int)(yLimit / 3.2)) * 3.2f);
        }
        // NP
        return new Vector2(-rnd.Next(0, (int)(xLimit / 3.2)) * 3.2f, rnd.Next(0, (int)(yLimit / 3.2)) * 3.2f);
    }

    public void GenerateEndPoint()
    {
        int i = rnd.Next(0, 3);
        Cadran c = WhichCadran(player.position);
        while (CadranToInt(c) == i)
        {
            i = rnd.Next(0, 3);
        }
        bool occupied = true;
        Vector2 pos = Vector2.zero;
        while (occupied)
        {
            pos = GeneratePositionInCadran(IntToCadran(i));
            if (player.gameObject.GetComponent<PlayerMovement>().CheckIfFreeSpace(pos))
            {
                occupied = false;
            }
        }
        endPoint.transform.position = pos;
    }
    /// <summary>
    /// Generate FireBall for current level
    /// </summary>
    public void GenerateFireBall()
    {
        _currentFireBall = new List<GameObject>() {
        Instantiate(Resources.Load("FireBall"), new Vector2((xLimit - rnd.Next(0, 3) * 3.2f), (yLimit - rnd.Next(0, 3) * 3.2f)), Quaternion.identity) as GameObject,
        Instantiate(Resources.Load("FireBall"), new Vector2(-(xLimit - rnd.Next(0, 3) * 3.2f), (yLimit - rnd.Next(0, 3) * 3.2f)), Quaternion.identity) as GameObject,
        Instantiate(Resources.Load("FireBall"), new Vector2(-(xLimit - rnd.Next(0, 3) * 3.2f), -(yLimit - rnd.Next(0, 3) * 3.2f)), Quaternion.identity) as GameObject,
        Instantiate(Resources.Load("FireBall"), new Vector2((xLimit - rnd.Next(0, 3) * 3.2f), -(yLimit - rnd.Next(0, 3) * 3.2f)), Quaternion.identity) as GameObject
        };
    }

    public void LaunchFireBall()
    {
        foreach(GameObject o in _currentFireBall)
        {
            o.GetComponent<FireBallMovement>().LauchAtSpeed((_level + 1f) * 10f);
        }
    }

    public void ReachedEndPoint()
    {
        _nReachedEndPointInCurrentLevel += 1;
        scoreManager.totalReachedEndPoint += 1;
        if (_nReachedEndPointInCurrentLevel >= 4)
        {
            NextLevel();
        }
        else
        {
            GenerateEndPoint();
        }

    }

    public void NextLevel()
    {
        _level += 1;
        lastStartPosition = player.position;
        LaunchFireBall();
        GenerateEndPoint();
        _nDeathPerLevel.Add(_nDeath);
        _nDeath = 0;
        _timePerLevelList.Add(_time);
        _time = 0;
        Restart();
    }

    public void Death()
    {
        _nDeath += 1;
        if (_nDeath <= _nDeathEndGame)
        {
            Restart();
        }
        else
        {
            EndGame();
        }
    }

    public void Restart()
    {
        
        _nRestart += 1;
        _nReachedEndPointInCurrentLevel = 0;
        player.gameObject.GetComponent<PlayerMovement>().Restart();
    }

    public void PauseGame()
    {
        _inGame = false;
    }

    public void ResumeGame()
    {
        _inGame = true;
    }

    /// <summary>
    /// Generate a float
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public static float NextFloat(float min, float max)
    {
        double range = (double) max - (double) min;
        double sample = rnd.NextDouble();
        double scaled = (sample * range) + min;
        float f = (float)scaled;
        return f;
    }

    

    /*
    private void OnLevelWasLoaded(int level)
    {
        if(level == SceneManager.GetActiveScene().buildIndex)
        {
            print("ScoreBoard");
            scoreManager.FindAndSetText();
        }
    }*/

    void OnSceneLoaded(Scene level, LoadSceneMode mode)
    {
        if (level == SceneManager.GetSceneByName("EndGame"))
        {
            print("ScoreBoard");
            scoreManager.FindAndSetText();
        }
        else if(level == SceneManager.GetSceneByName("Game"))
        {
            print("New game");
            NewGame();
        }
    }

}
