using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayManager : MonoBehaviour
{
    [SerializeField] BallController ballController;
    [SerializeField] CameraController camController;
    [SerializeField] GameObject finishDialog;
    [SerializeField] TMP_Text finishText;
    [SerializeField] TMP_Text shootCounter;

    bool isBallOOB;
    bool isBallTeleporting;
    bool isGoal;
    Vector3 lastKnownPosition;

    private void OnEnable()
    {
        ballController.OnBallShoot.AddListener(UpdateShootCount);
    }

    private void OnDisable()
    {
        ballController.OnBallShoot.RemoveListener(UpdateShootCount);
    }

    private void Update()
    {
        if (ballController.ShootingMode)
        {
            lastKnownPosition = ballController.transform.position;
        }

        var inputActive = Input.GetMouseButton(0) 
            && ballController.isMove() == false 
            && ballController.ShootingMode == false 
            && isBallOOB == false;

        camController.SetInputActive(inputActive);
    }

    public void OnBallGoalEnter()
    {
        isGoal = true;
        ballController.enabled = false;
        // popup
        finishDialog.gameObject.SetActive(true);
        finishText.text = $"You finished in {ballController.ShootCount} shots!";
    }

    public void OnBallOOB()
    {
        if (isGoal)
        {
            return;
        }
        
        if (isBallTeleporting == false)
        {
            Invoke("TeleportBallToLastPos", 2);
        }
        ballController.enabled = false;
        isBallOOB = true;
        isBallTeleporting = true;
    }

    public void TeleportBallToLastPos()
    {
        TeleportBall(lastKnownPosition);
    }

    public void TeleportBall(Vector2 targetPos)
    {
        var golfRBvar = ballController.GetComponent<Rigidbody>();
        golfRBvar.isKinematic = true;
        ballController.transform.position = lastKnownPosition;
        golfRBvar.isKinematic = false;

        ballController.enabled = true;
        isBallTeleporting = false;
        isBallOOB = false;
    }

    public void UpdateShootCount(int count)
    {
        shootCounter.text = count.ToString();
    }
}
