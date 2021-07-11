using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Objective))]
public class ObjectiveSolveRiddles : MonoBehaviour
{
    [Tooltip("Chose whether you need to solve every riddle or only a minimum amount")]
    public bool mustSolveAllRiddles = true;
    [Tooltip("If MustSolveAllRiddles is false, this is the amount of riddles is required")]
    public int riddlesToCompleteObjective = 5;
    [Tooltip("Start sending notification about remaining riddles when this amount of riddles is left")]
    public int notificationRiddlesRemainingThreshold = 3;

    RiddleManagr m_riddle;
    Objective m_Objective;
    int m_riddleTotal;
    void Start()
    {
        m_Objective = GetComponent<Objective>();
        DebugUtility.HandleErrorIfNullGetComponent<Objective, ObjectiveSolveRiddles>(m_Objective, this, gameObject);

        m_riddle = FindObjectOfType<RiddleManagr>();
        DebugUtility.HandleErrorIfNullFindObject<RiddleManagr, ObjectiveSolveRiddles>(m_riddle, this);
        m_riddle.onRemoveRiddle += OnRiddleSolve;

        if (mustSolveAllRiddles)
            riddlesToCompleteObjective = m_riddle.numberOfRiddlesTotal;


        // set a title and description specific for this type of objective, if it hasn't one
        if (string.IsNullOrEmpty(m_Objective.title))
            m_Objective.title = "Solve " + (mustSolveAllRiddles ? "all the" : riddlesToCompleteObjective.ToString()) + " riddles";

        if (string.IsNullOrEmpty(m_Objective.description))
            m_Objective.description = GetUpdatedCounterAmount();
    }
    string GetUpdatedCounterAmount()
    {
        return m_riddleTotal + " / " + riddlesToCompleteObjective;
    }

    void OnRiddleSolve (RiddleActivator riddle, int remaining)
    {
        if (m_Objective.isCompleted)
            return;

        if (mustSolveAllRiddles)
            riddlesToCompleteObjective = m_riddle.numberOfRiddlesTotal;

        m_riddleTotal = m_riddle.numberOfRiddlesTotal - remaining;
        int targetRemaning = mustSolveAllRiddles ? remaining : riddlesToCompleteObjective - m_riddleTotal;

        // update the objective text according to how many riddles remain to solve
        if (targetRemaning == 0)
        {
            m_Objective.CompleteObjective(string.Empty, GetUpdatedCounterAmount(), "Objective complete : " + m_Objective.title);
        }
        else
        {

            if (targetRemaning == 1)
            {
                string notificationText = notificationRiddlesRemainingThreshold >= targetRemaning ? "One riddle left" : string.Empty;
                m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
            }
            else if (targetRemaning > 1)
            {
                // create a notification text if needed, if it stays empty, the notification will not be created
                string notificationText = notificationRiddlesRemainingThreshold >= targetRemaning ? targetRemaning + " riddles to solve left" : string.Empty;

                m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
            }
        }
    }
}
