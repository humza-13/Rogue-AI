using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    private int logic_points;
    private int kill_points;

    public void Update_lScore(int l_points)
    {
        logic_points += l_points;
        Debug.Log(logic_points);
    }
    public void Update_kScore(int k_points)
    {
        kill_points += k_points;
        Debug.Log(kill_points);
    }
    public void show()
    {
        Debug.Log(logic_points);
        Debug.Log(kill_points);
    }
}

