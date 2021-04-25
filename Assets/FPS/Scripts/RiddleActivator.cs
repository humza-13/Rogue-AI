using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RiddleActivator : MonoBehaviour
{
    public GameObject RiddleWindow;
    public GameObject HintWindow;
    PlayerInputHandler m_PlayerInputsHandler;
    InGameMenuManager menu;
    public GameObject riddle_hat;

    public RiddleSetting riddle_setting;
    public TextMeshProUGUI r_title;
    public TextMeshProUGUI r_description;
    public TextMeshProUGUI r_time;
    public InputField answer_input;
    private bool start_time = false;
    private float time = 0;
    private string player_answer;
    
    public TextMeshProUGUI h_title;
    public TextMeshProUGUI h_description;
    


    void Start()
    {
        m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, RiddleActivator>(m_PlayerInputsHandler, this);

        menu = FindObjectOfType<InGameMenuManager>();
        DebugUtility.HandleErrorIfNullFindObject<InGameMenuManager, RiddleActivator>(menu, this);


        riddle_hat.SetActive(true);
        RiddleWindow.SetActive(false);
        HintWindow.SetActive(false);
    }

    private void Update()
    {
        // Lock cursor when clicking outside of menu
        if (!RiddleWindow.activeSelf && Input.GetMouseButtonDown(0) && !menu.menuRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (start_time == true)
        {
            time += Time.unscaledDeltaTime;
            r_time.text = time.ToString("F0");
        }
       

    }

        private void OnTriggerEnter(Collider other)
    {
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
            r_description.text = riddle_setting.description;
            start_time = true;

            EventSystem.current.SetSelectedGameObject(null);

        }

    }

    public void OpenHintWindow()
    {
        h_title.text = riddle_setting.title;
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
            time /= 60;
            Debug.Log(input);
            Debug.Log(time);
            time = 0;
            riddle_hat.SetActive(false);
            CloseRiddle();
            
        }
        else
        {
            Debug.Log("false");
        }
    }


}
