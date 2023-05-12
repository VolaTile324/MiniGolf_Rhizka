using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRamp : MonoBehaviour
{
    [SerializeField] float force;
    bool isSpeeding;

    private void OnCollisionEnter(Collision other)
    {
        Speed(other.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        Speed(other);
    }

    private void Speed(Collider other)
    {
        if (isSpeeding == false && other.transform.CompareTag("Player") && other.transform.TryGetComponent<Rigidbody>(out var golfRB))
        {
            golfRB.AddForce(this.transform.forward * force, ForceMode.Impulse);
            isSpeeding = true;
            Invoke("Reset", 0.3f);
        }
    }
}
