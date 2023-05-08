using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook CMFreeLook;

    public void SetInputActive(bool value)
    {
        if (value)
        {
            CMFreeLook.m_XAxis.m_InputAxisName = "Mouse X";
            CMFreeLook.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        else
        {
            CMFreeLook.m_XAxis.m_InputAxisName = "";
            CMFreeLook.m_YAxis.m_InputAxisName = "";
        }
    }
}
