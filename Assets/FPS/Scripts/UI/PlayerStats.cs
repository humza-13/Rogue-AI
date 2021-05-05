using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    private int logic_points;
    private int kill_score;
    public RiddleActivator a_riddle;

    [Header("UI Elements")]
    public TextMeshProUGUI d_logic_score;
    public TextMeshProUGUI d_kill_score;

    private void Start()
    {
        a_riddle = FindObjectOfType<RiddleActivator>();

        // setting logic points and kill scors value to zero on start
        logic_points = 0;
        kill_score = 0;

    }
    public void IncreaseLogicPoints(int points, float time)
    {
        // using exponential decay w.r.t time taken to give logic points 
        float k = (float)-0.001;
        logic_points += Mathf.CeilToInt(points * Mathf.Exp(k * time));
        UpdateLogicScore();
    }

    public void UpdateLogicScore ()
    {
        d_logic_score.text = logic_points.ToString();
    }

    public int getLogicPoints()
    {
        return logic_points;
    }

    public void IncreaseKillScore()
    {
        kill_score += 2;
        UpdateKillScore();
    }

    private void UpdateKillScore()
    {
        d_kill_score.text = kill_score.ToString();
    }

    public int GetKillScore()
    {
        return kill_score;
    }
}

