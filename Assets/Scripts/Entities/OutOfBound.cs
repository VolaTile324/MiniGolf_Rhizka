using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OutOfBound : MonoBehaviour
{
    public UnityEvent OnBallEnter = new UnityEvent();

    /* private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            OnBallEnter.Invoke();
        }
    } */

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            OnBallEnter.Invoke();
        }
    }
}
