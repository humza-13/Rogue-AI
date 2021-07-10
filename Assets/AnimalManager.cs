using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalManager : MonoBehaviour
{
    Health health;
    public GameObject player;
    public float healAmount;
    public float damageAmount;
    public RuntimeAnimatorController animCon;
    private Animator anim;
    public bool isUnhealthy;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.onDie += OnDie;


    }
    private void Update()
    {
        if (health.isCritical())
        {
            agent.isStopped = true;
            anim.runtimeAnimatorController = animCon;
        }
    }

    void OnDie()
    {
        if(isUnhealthy == false) {
            heal();
        }
        else
        {
            giveDamage();
        }
        
            // this will call the OnDestroy function
        Destroy(gameObject);
    }

    void heal()
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth && playerHealth.canPickup())
        {
            playerHealth.Heal(healAmount);
        }
    }

    void giveDamage()
    {
        Health playerHealth = player.GetComponent<Health>();
        playerHealth.TakeDamage(damageAmount, gameObject);

    }
}
