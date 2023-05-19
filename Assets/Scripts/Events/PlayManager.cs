using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;

public class PlayManager : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioClip music;
    [SerializeField] BallController ballController;
    [SerializeField] CameraController camController;
    [SerializeField] GameObject finishDialog;
    [SerializeField] TMP_Text finishText;
    [SerializeField] TMP_Text shootCounter;
    [SerializeField] TMP_Text parCounter;
    [SerializeField] TMP_Text penaltyReason;
    [SerializeField] TMP_Text shootResult;
    [SerializeField] TMP_Text parResult;

    public UnityEvent OnPause;
    int par;
    bool isBallOOB;
    bool isBallTeleporting;
    bool isGoal;
    bool isPaused;
    Vector3 lastKnownPosition;

    private void Start()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        audioManager.PlayBGM(music);
    }

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

        // get pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGoal)
            {
                return;
            }

            if (isPaused)
            {
                return;
            }

            OnPause.Invoke();
        }
    }

    public void PauseGame()
    {
        // check if the game is paused
        if (isPaused == true)
        {
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void OnBallGoalEnter()
    {
        isGoal = true;
        ballController.enabled = false;
        // popup
        finishDialog.gameObject.SetActive(true);
        parResult.text = "Par: " + parCounter.text;
        shootResult.text = "Shots: " + ballController.ShootCount.ToString();

        // par checking
        par = int.Parse(parCounter.text);
        if (ballController.ShootCount == 1)
        {
            finishText.text = "Hole in one!";
            finishText.color = Color.yellow;
        }
        else if (ballController.ShootCount == par - 1)
        {
            finishText.text = "Birdie!";
            finishText.color = Color.green;
        }
        else if (ballController.ShootCount == par - 2)
        {
            finishText.text = "Eagle!";
            finishText.color = Color.green;
        }
        else if (ballController.ShootCount == par - 3)
        {
            finishText.text = "Albatross!";
            finishText.color = Color.green;
        }
        else if (ballController.ShootCount <= par - 4)
        {
            finishText.text = "Condor!";
            finishText.color = Color.green;
        }
        else if (ballController.ShootCount == par)
        {
            finishText.text = "Par!";
            finishText.color = Color.white;
        }
        else if (ballController.ShootCount == par + 1)
        {
            finishText.text = "Bogey!";
            finishText.color = Color.red;
        }
        else if (ballController.ShootCount == par + 2)
        {
            finishText.text = "Double Bogey!";
            finishText.color = Color.red;
        }
        else if (ballController.ShootCount == par + 3)
        {
            finishText.text = "Triple Bogey!";
            finishText.color = Color.red;
        }
        else if (ballController.ShootCount >= par + 4)
        {
            finishText.text = (ballController.ShootCount - par).ToString() + " shots Over Par!";
            finishText.color = Color.red;
        }
    }

    public void OOBMisc()
    {
        penaltyReason.gameObject.SetActive(true);
        penaltyReason.text = "Out of Bound, One-Stroke Penalty";
        OnBallOOB();
    }

    public void OOBWaterHazard()
    {
        penaltyReason.gameObject.SetActive(true);
        penaltyReason.text = "Water Hazard, One-Stroke Penalty";
        OnBallOOB();
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
        ballController.Penalty();
        UpdateShootCount(ballController.ShootCount);
        penaltyReason.gameObject.SetActive(false);
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
