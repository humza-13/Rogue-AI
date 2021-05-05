using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RiddleActivator : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject RiddleWindow;
    public GameObject HintWindow;
    
    PlayerInputHandler m_PlayerInputsHandler;
    InGameMenuManager menu;
    [Header("Riddle Object")]
    public GameObject riddle_hat;
    
    public RiddleSetting riddle_setting;
    public PlayerStats stats;
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
    


    void Start()
    {
        m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, RiddleActivator>(m_PlayerInputsHandler, this);

        menu = FindObjectOfType<InGameMenuManager>();
        DebugUtility.HandleErrorIfNullFindObject<InGameMenuManager, RiddleActivator>(menu, this);

        stats = FindObjectOfType<PlayerStats>();

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
        if (start_time == true)
        {
            // starting time for riddle
            time += Time.unscaledDeltaTime;
            r_time.text = time.ToString("F0");
        }
       

    }

        private void OnTriggerEnter(Collider other)
    {
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
        h_title.text = riddle_setting.title;
        riddle_setting.hints = riddle_setting.hints.Replace(".", "." + System.Environment.NewLine);
        h_description.text = riddle_setting.hints;
        HintWindow.SetActive(true);
        
  
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
        HintWindow.SetActive(false);  
    }

    public void GetAnswer()
    {
        player_answer = answer_input.text;
        CheckAnswer(player_answer);

    }

    public void CheckAnswer(string input)
    {
        if (input.Equals(riddle_setting.answer))
        {
            start_time = false;
            GiveLogicPoints();
            time = 0;
            riddle_hat.SetActive(false);
            CloseRiddle();
            
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
   

}
