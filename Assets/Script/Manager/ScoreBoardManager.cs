using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour {


    public float AverageTimePerLevel;
    public Text AvTimeText;
    public float TotalTimeinGame;
    public Text TotalTimeText;
    public float averageDeathPerLevel;
    public Text AvDeathText;
    public int totalDeath;
    public Text TotalDeathText;
    public int totalRestart;
    public Text TotalRestartText;
    public int totalReachedEndPoint;
    public Text TotalReachedText;
    

    public void FindAndSetText()
    {
        print("Text" + AverageTimePerLevel + " " + totalReachedEndPoint);
        Transform tr = GameObject.Find("ScoreCanvas").transform;
        AvTimeText = tr.GetChild(1).GetComponent<Text>();
        AvTimeText.text = "Average time per level: " + AverageTimePerLevel.ToString("0.0");
        TotalTimeText = tr.GetChild(2).GetComponent<Text>();
        TotalTimeText.text = "Total time in game: " + TotalTimeinGame.ToString("0.0");
        AvDeathText = tr.GetChild(3).GetComponent<Text>();
        AvDeathText.text = "Average death per level: " + averageDeathPerLevel.ToString("0.0");
        TotalDeathText = tr.GetChild(4).GetComponent<Text>();
        TotalDeathText.text = "Total death in game: " + totalDeath.ToString();
        TotalRestartText = tr.GetChild(5).GetComponent<Text>();
        TotalRestartText.text = "Total restart in game: " + totalRestart.ToString();
        TotalReachedText = tr.GetChild(6).GetComponent<Text>();
        TotalReachedText.text = "Total end points reached in game: " + totalReachedEndPoint.ToString();
    }

    public void ActiveText()
    {

    }


    void SetActiveAfterTime(GameObject o, float t = 1f)
    {

    }

}
