using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Assertions.Must;

public class BallController : MonoBehaviour
{
    [SerializeField] Collider col;
    [SerializeField] Rigidbody golfRB;
    [SerializeField] float force;

    bool shoot;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayCast, out var hitInfo) && hitInfo.collider == col)
            {
                shoot = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (shoot)
        {
            shoot = false;

            Vector3 direction = Camera.main.transform.forward;
            direction.y = 0;
            golfRB.AddForce(direction * force, ForceMode.Impulse);
        }

        if (golfRB.velocity.sqrMagnitude < 0.01f && golfRB.velocity.sqrMagnitude > 0)
        {
            golfRB.velocity = Vector3.zero;
        }
    }

    public bool isMove()
    {
        return golfRB.velocity != Vector3.zero;
    }
}
