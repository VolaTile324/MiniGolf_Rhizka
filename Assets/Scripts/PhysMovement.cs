using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysMovement : MonoBehaviour
{
    [SerializeField] Rigidbody golfBody;
    [SerializeField] float force;

    Vector3 direction;

    private void Update()
    {
        direction = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
            );
    }

    private void FixedUpdate()
    {
        if (direction == Vector3.zero)
            return;

        golfBody.AddForce(direction * force);
    }
}
