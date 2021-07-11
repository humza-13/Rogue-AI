using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RiddleActivator : MonoBehaviour
{
    delegate void Action_type();
    Action_type action_type;

    [Header("Doors with Animations")]
    public Animation door;

    [Header("Doors to Destroy")]
    public GameObject doortodestroy;

    [Header("Riddle Type")]
    public bool isOptional;
    private float oriddle_time = 60; 

    [Header("UI Elements")]
    public GameObject RiddleWindow;
    public GameObject HintWindow;
    
    PlayerInputHandler m_PlayerInputsHandler;
    InGameMenuManager menu;


    [Header("Riddle Object")]
    public GameObject riddle_hat;

    [Header("Riddle Action ")]
    [Tooltip("1 for conventional doors, 2 for sliding doors)")]
    public int action_types;

    [Header("Doors without Animations")]
    public Transform door_left;
    public Transform door_right;


    public RiddleSetting riddle_setting;
    private PlayerStats stats;
    [Header("Window Elements")]
    public TextMeshProUGUI r_title;
    public TextMeshProUGUI r_description;
    public TextMeshProUGUI r_time;
    public GameObject r_tryAgain;
    public InputField answer_input;
    
    private bool start_time = false;
    private float time = 0;
    private string player_answer;
    [Header("Hint Window Elements")]
    public TextMeshProUGUI h_title;
    public TextMeshProUGUI h_description;

    private bool i_active = false;
   
  
   

    void Start()
    {
        m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, RiddleActivator>(m_PlayerInputsHandler, this);

        menu = FindObjectOfType<InGameMenuManager>();
        DebugUtility.HandleErrorIfNullFindObject<InGameMenuManager, RiddleActivator>(menu, this);

        stats = FindObjectOfType<PlayerStats>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerStats, RiddleActivator>(stats, this);

       

        //setting all riddle and hint windows inactive and riddle object to active
        riddle_hat.SetActive(true);
        RiddleWindow.SetActive(false);
        HintWindow.SetActive(false);
        r_tryAgain.SetActive(false);
      
    }

    private void Update()
    {
       
        // Lock cursor when clicking outside of menu
        if (!RiddleWindow.activeSelf && Input.GetMouseButtonDown(0) && !menu.menuRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // checking if riddle is activated 
        if (start_time == true && i_active == true)
        {
            if (isOptional == false)
            {
                // starting time for riddle
                time += Time.unscaledDeltaTime;
                r_time.text = time.ToString("F0");
            }
            else
            {
                oriddle_time -= Time.unscaledDeltaTime;
                r_time.text = oriddle_time.ToString("F0");

                if (oriddle_time < 0)
                {
                    start_time = false;
                    timeUp();
                }
            }
        }
      


    }

        private void OnTriggerEnter(Collider other)
    {
        i_active = true;
        
        // opening riddle on trigger
        openRiddle();
    }

    private void openRiddle()
    {
 
        RiddleWindow.SetActive(true);

        if (RiddleWindow.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            r_title.text = riddle_setting.title;
            // formatting input string
            riddle_setting.description = riddle_setting.description.Replace(".", "." + System.Environment.NewLine);
            r_description.text = riddle_setting.description;
            start_time = true;

            EventSystem.current.SetSelectedGameObject(null);

        }

    }

    public void OpenHintWindow()
    {
        HintWindow.SetActive(true);
        if (HintWindow.activeSelf && i_active == true)
        {
            h_title.text = riddle_setting.title;
            h_description.text = riddle_setting.hints.Replace(".", "." + System.Environment.NewLine); 
          
        }
  
    }
    
    public void CloseRiddle()
    {
        RiddleWindow.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
    public void CloseHint()
    {
        h_title.text = "";
        h_description.text = "";
        HintWindow.SetActive(false);  
    }

    public void GetAnswer()
    {
        if (i_active == true)
        {
            player_answer = answer_input.text;
            CheckAnswer(player_answer.ToUpper());
        }
    }

    public void CheckAnswer(string input)
    {
        if (input.Equals(riddle_setting.answer))
        {
           
            if (isOptional == false)
            {
                start_time = false;
                GiveLogicPoints();
            }
            else
            {
                GiveOptionalPoints();
            }

            time = 0;
            oriddle_time = 60;
            answer_input.text = "";
            r_tryAgain.SetActive(false);
            CloseRiddle();
            i_active = false;
            riddle_setting.answer = null;
            riddle_setting.hints = "";
            riddle_action(action_types);
            riddle_hat.SetActive(false);

            

        }
        else
        {
            r_tryAgain.SetActive(true);   
        }
        
    }

    public void GiveLogicPoints()
    {
        // calling players stats increase logic point function to give logic points
        stats.IncreaseLogicPoints(riddle_setting.riddle_score, time);
    }
    public void GiveOptionalPoints()
    {
        // calling players stats increase logic point function to give logic points
        stats.IncreaseLogicPoints(riddle_setting.riddle_score, 0);
    }
    void timeUp()
    {
        answer_input.text = "";
        r_tryAgain.SetActive(false);
        oriddle_time = 60;
        CloseRiddle();
        riddle_setting.answer = null;
        riddle_hat.SetActive(false);
    }
    public void riddle_action(int type)
    {
        if (type == 0)
        {
            action_type = null;

        }

        if (type == 1)
        {
            action_type = conventional_door;
            action_type();
           
            
        }
        else if (type == 2)
        {
            action_type = slidding_door;
            action_type();
        }
        else if (type == 3)
        {
            action_type = destroy_door;
            action_type();
        }


    }

    void conventional_door()
    {
        
        Quaternion rotationL = Quaternion.AngleAxis(-180, Vector3.down);
        Quaternion rotationR = Quaternion.AngleAxis(270, Vector3.down);

        for (int i = 0; i < 500; i++)
        {
            door_left.transform.rotation = Quaternion.Slerp(door_left.transform.rotation, rotationL, .0125f);
            door_right.transform.rotation = Quaternion.Slerp(door_right.transform.rotation, rotationR, .0125f);
            
        }
    }

    void slidding_door()
    {
        door.Play();
  
    }

    void destroy_door()
    {
        doortodestroy.SetActive(false);
    }

}
