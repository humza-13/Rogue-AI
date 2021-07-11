using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RiddleManagr : MonoBehaviour
{
    PlayerCharacterController m_PlayerController;
    public List<RiddleActivator> riddles { get; private set; }
    public int numberOfRiddlesTotal { get; private set; }
    public int numberOfRiddlesRemaining => riddles.Count;

    public UnityAction<RiddleActivator, int> onRemoveRiddle;

    private void Awake()
    {
        m_PlayerController = FindObjectOfType<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, EnemyManager>(m_PlayerController, this);

        riddles = new List<RiddleActivator>();
    }

    public void RegisterRiddle(RiddleActivator riddle)
    {
        riddles.Add(riddle);

        numberOfRiddlesTotal++;
    }

    public void UnregisterRiddle(RiddleActivator riddle)
    {
        int riddleRemainingNotification = numberOfRiddlesRemaining - 1;

        if (onRemoveRiddle != null)
        {
            onRemoveRiddle.Invoke(riddle, riddleRemainingNotification);
        }

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        riddles.Remove(riddle);
    }
}
