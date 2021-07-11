using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectorManager : MonoBehaviour
{
    public Button[] btns;
    FirebaseManager firebase;
    public TMP_Text username;
    public TMP_Text logicScore;
    public TMP_Text killScore;
    public bool isSignedin;
    

    private void Start()
    {
     
        firebase = GetComponent<FirebaseManager>();      
        username.text = firebase.getUserName();
        isSignedin = true;
    }

    private void Update()
    {
        if(isSignedin == true)
        { 
            firebase.RefreashDb();
            Debug.Log("1");  
        }
       

        logicScore.text = PlayerPrefs.GetInt("LogicPoints").ToString();
        killScore.text = PlayerPrefs.GetInt("KillPoints").ToString();

        int levelreached = PlayerPrefs.GetInt("LevelReached");
        
        for (int i = 0; i < btns.Length; i++)
        {
            if (i + 1 > levelreached)
            {
                btns[i].interactable = false;
            }
        }


    }




}
