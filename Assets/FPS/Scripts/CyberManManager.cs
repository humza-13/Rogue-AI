using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CyberManManager : MonoBehaviour
{
    Health health;
    public GameObject player;
    public float healAmount;
    public RuntimeAnimatorController animCon;
    private Animator anim;
    private NavMeshAgent cyberman;
    Health m_PlayerHealth;
    bool isInvincible;
    private float count;
    NotificationHUDManager notice;

    private void Start()
    {
        notice = FindObjectOfType<NotificationHUDManager>();
        m_PlayerHealth = player.GetComponent<Health>();
        cyberman = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.onDie += OnDie;
        isInvincible = false;


    }
    private void Update()
    {
        if (health.isCritical())
        {
            cyberman.isStopped = true;
            anim.runtimeAnimatorController = animCon;
        }

        if (isInvincible)
        {
            m_PlayerHealth.invincible = true;
            count += Time.deltaTime;

            if(count >= 60)
            {
                isInvincible = false;
                takeInvincibilty();
                count = 0;
            }
        }

    }

    void OnDie()
    {
        m_PlayerHealth.Heal(healAmount);
        
        giveInvincibilty();

 
    }

    void giveInvincibilty()
    {
        isInvincible = true;
        notice.CreateNotification("You are now invincible for 60 seconds!");
    }

    void takeInvincibilty()
    {
        m_PlayerHealth.invincible = false;
    }
}
