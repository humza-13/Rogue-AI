using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleAction : MonoBehaviour
{
    delegate void Action_type();
    Action_type action_type;

    [Header("Left and Right part of door")]
    public Transform door_left;
    public Transform door_right;


    private bool open = false;


    public void Update()
    {
        if (open == true)
        {
            action_type();
        }
    }
    public void riddle_action(int type) { 
        if (type == 1)
        {
            
            action_type = conventional_door;
            open = true;
        }
        else if(type == 2)
        {
           // action_type = sliding_door;
        }
    
    }

    void conventional_door()
    {
        Quaternion rotationL = Quaternion.AngleAxis(-180, Vector3.down);
        Quaternion rotationR = Quaternion.AngleAxis(270, Vector3.down);
        
        door_left.transform.rotation = Quaternion.Slerp(door_left.transform.rotation, rotationL, .0125f);
        door_right.transform.rotation = Quaternion.Slerp(door_right.transform.rotation, rotationR, .0125f);

    }
}
