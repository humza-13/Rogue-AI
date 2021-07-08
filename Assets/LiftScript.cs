using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{

    public Animation mover;
    Health m_PlayerHealth;
    PlayerInputHandler m_PlayerInputsHandler;
    public bool two_floors = false;

    private void Start()
    {
        m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler, this);

        m_PlayerHealth = m_PlayerInputsHandler.GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, InGameMenuManager>(m_PlayerHealth, this, gameObject);

        if (two_floors == false)
        {
            mover["Lift_animation"].speed = 0;
        }
        else
        {
            mover["Lift 2_floors animation"].speed = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        m_PlayerHealth.invincible = true;
        if (two_floors == false)
        {
            mover["Lift_animation"].speed = 1;
        }
        else
        {
            mover["Lift 2_floors animation"].speed = 1;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        m_PlayerHealth.invincible = false;

        if (two_floors == false)
        {
            mover["Lift_animation"].speed = 0;
        }
        else
        {
            mover["Lift 2_floors animation"].speed = 0;
        }
    }

}
