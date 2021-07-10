using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{

    public Animator mover;
    Health m_PlayerHealth;
    PlayerInputHandler m_PlayerInputsHandler;
 

    private void Start()
    {
        m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler, this);

        m_PlayerHealth = m_PlayerInputsHandler.GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, InGameMenuManager>(m_PlayerHealth, this, gameObject);

       
        mover.speed = 0;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        m_PlayerHealth.invincible = true;
       
        mover.speed = 1;
        


    }
    private void OnTriggerExit(Collider other)
    {
        m_PlayerHealth.invincible = false;

        mover.speed = 0;
        
    }

}
