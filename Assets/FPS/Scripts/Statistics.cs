using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    private int logic_points;
    private int kill_points;

    private void Start()
    {
        if (PlayerPrefs.HasKey("LogicPoints"))
        {
            logic_points = PlayerPrefs.GetInt("LogicPoints");
        }

        if (PlayerPrefs.HasKey("KillPoints"))
        {
            kill_points = PlayerPrefs.GetInt("KillPoints");
        }
    }
    public void Update_lScore(int l_points)
    {
        logic_points += l_points;
        PlayerPrefs.SetInt("LogicPoints", logic_points);
    }
    public void Update_kScore(int k_points)
    {
        kill_points += k_points;
        PlayerPrefs.SetInt("KillPoints", kill_points);
    }

    public void resetScores()
    {
        PlayerPrefs.SetInt("LogicPoints", 0);
        PlayerPrefs.SetInt("KillPoints", 0);
        PlayerPrefs.SetInt("LevelReached", 0);
        
    }
 
}

