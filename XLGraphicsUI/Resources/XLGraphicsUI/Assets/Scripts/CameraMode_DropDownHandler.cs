using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMode_DropDownHandler : MonoBehaviour
{
    public GameObject FollowCam;
    public GameObject NormalCam;
    public GameObject POVCam;
    public GameObject SkateCam;

    public void HandleInputData_CameraMode(int val)
    {
        if (val == 0)
        {
            FollowCam.SetActive(false);
            POVCam.SetActive(false);
            SkateCam.SetActive(false);
            NormalCam.SetActive(true);
        }
        if (val == 1)
        {
            FollowCam.SetActive(false);
            POVCam.SetActive(true);
            SkateCam.SetActive(false);
            NormalCam.SetActive(false);
        }
        if (val == 2)
        {
            FollowCam.SetActive(true);
            POVCam.SetActive(false);
            SkateCam.SetActive(false);
            NormalCam.SetActive(false);
        }
        if (val == 3)
        {
            FollowCam.SetActive(false);
            POVCam.SetActive(false);
            SkateCam.SetActive(true);
            NormalCam.SetActive(false);
        }
    }
}
