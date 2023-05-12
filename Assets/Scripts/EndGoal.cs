using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndGoal : MonoBehaviour
{
    public UnityEvent OnBallGoalEnter = new UnityEvent();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnBallGoalEnter.Invoke();
        }
    }
}
