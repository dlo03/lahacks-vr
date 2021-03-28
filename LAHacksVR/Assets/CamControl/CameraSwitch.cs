using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject faceCam;
    public GameObject overheadCam;

    void Start()
    {
        // Find the camera game objects
        faceCam = GameObject.Find("/[VRSimulator_CameraRig]/Neck/Camera");
        overheadCam = GameObject.Find("/OverheadCamera");
    }

    // Update is called once per frame
    void Update()
    {
        // Switch to the face cam when 1 is pressed
        if (Input.GetKeyDown("1"))
        {
            faceCam.SetActive(true);
            overheadCam.SetActive(false);
        }
        // Switch to the overhead cam when 2 is pressed
        if (Input.GetKeyDown("2"))
        {
            faceCam.SetActive(false);
            overheadCam.SetActive(true);
        }
    }
}
