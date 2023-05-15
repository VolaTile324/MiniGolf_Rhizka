using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class BallController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Rigidbody golfRB;
    [SerializeField] float force;
    [SerializeField] LineRenderer aimLine;
    [SerializeField] Transform aimWorld;
    [SerializeField] GameObject aimBox;
    [SerializeField] Slider powerSlider;
    [SerializeField] GameObject readyStatus;
    [SerializeField] GameObject rollingStatus;

    int shootCount;
    bool shoot;
    bool shootingMode;
    float forceFactor;
    Vector3 forceDirection;
    Ray ray;
    Plane plane;

    public bool ShootingMode { get => shootingMode; }
    public int ShootCount { get => shootCount; }
    public UnityEvent<int> OnBallShoot = new UnityEvent<int>();

    private void Update()
    {
        if (shootingMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                aimLine.gameObject.SetActive(true);
                aimWorld.gameObject.SetActive(true);
                aimBox.gameObject.SetActive(true);
                plane = new Plane(Vector3.up, this.transform.position);
            }
            else if (Input.GetMouseButton(0))
            {
                // force direction
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                plane.Raycast(ray, out var distance);
                forceDirection = (this.transform.position - ray.GetPoint(distance));
                forceDirection.Normalize();

                // force factor
                var mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                var ballViewportPos = Camera.main.WorldToViewportPoint(this.transform.position);
                var pointerDirection = ballViewportPos - mouseViewportPos;
                pointerDirection.z = 0;
                pointerDirection.z *= Camera.main.aspect;
                pointerDirection.z = Mathf.Clamp(pointerDirection.z, -0.5f, 0.5f);
                forceFactor = pointerDirection.magnitude * 2;
                // Debug.Log(forceFactor);

                // aim arrow vis
                aimWorld.transform.position = this.transform.position;
                aimWorld.forward = forceDirection;
                aimWorld.localScale = new Vector3(1, 1, 0.5f + forceFactor);

                var ballScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                var mouseScreenPos = Input.mousePosition;
                ballScreenPos.z = 1f;
                mouseScreenPos.z = 1f;
                var positions = new Vector3[] {
                    Camera.main.ScreenToWorldPoint(ballScreenPos),
                    Camera.main.ScreenToWorldPoint(mouseScreenPos)
                };
                aimLine.SetPositions(positions);
                aimLine.endColor = Color.Lerp(Color.green, Color.red, forceFactor);
                powerSlider.value = forceFactor;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                shoot = true;
                shootingMode = false;
                aimLine.gameObject.SetActive(false);
                aimWorld.gameObject.SetActive(false);
                aimBox.gameObject.SetActive(false);
            }

            // player want to cancel the shot
            if (Input.GetMouseButtonDown(1))
            {
                shootingMode = false;
                aimLine.gameObject.SetActive(false);
                aimWorld.gameObject.SetActive(false);
                aimBox.gameObject.SetActive(false);
            }
        }

        if (this.isMove())
        {
            readyStatus.SetActive(false);
            rollingStatus.SetActive(true);
            // find child image, make it rotating
            rollingStatus.transform.GetChild(0).Rotate(0, 0, 2);
        }
        else
        {
            readyStatus.SetActive(true);
            rollingStatus.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (shoot)
        {
            shoot = false;

            golfRB.AddForce(forceDirection * force * forceFactor, ForceMode.Impulse);
            shootCount++;
            OnBallShoot.Invoke(shootCount);
        }

        if (golfRB.velocity.sqrMagnitude < 0.01f && golfRB.velocity.sqrMagnitude != 0)
        {
            golfRB.velocity = Vector3.zero;
        }
    }

    public void Penalty()
    {
        shootCount++;
    }

    public void CancelShot()
    {
        shootingMode = false;
        aimLine.gameObject.SetActive(false);
        aimWorld.gameObject.SetActive(false);
        aimBox.gameObject.SetActive(false);
    }

    public bool isMove()
    {
        return golfRB.velocity != Vector3.zero;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (this.isMove())
        {
            return;
        }

        shootingMode = true;
    }
}
