using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayManager : MonoBehaviour
{
    [SerializeField] BallController ballController;
    [SerializeField] CameraController camController;

    private void Update()
    {
        var inputActive = Input.GetMouseButton(0) && ballController.isMove() == false;
        camController.SetInputActive(inputActive);
    }
}
