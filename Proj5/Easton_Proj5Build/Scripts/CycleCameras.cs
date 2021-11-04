using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Edited by Cole Easton
/// Cycles the active camera when C is pressed
/// </summary>
public class CycleCameras : MonoBehaviour
{
    // Camera array that holds a reference to every camera in the scene
    public Camera[] cameras;
    /// <summary>
    /// The descriptions corresponding to each camera.  Must be the same length as cameras
    /// </summary>
    public string[] descriptions;

    // Current camera 
    private int currentCameraIndex;


    // Use this for initialization
    void Start()
    {
        currentCameraIndex = 0;

        // Turn all cameras off, except the first default one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        // If any cameras were added to the controller, enable the first one
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Press the 'C' key to cycle through cameras in the array
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Cycle to the next camera
            currentCameraIndex++;

            // If cameraIndex is in bounds, set this camera active and last one inactive
            if (currentCameraIndex < cameras.Length)
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
            // If last camera, cycle back to first camera
            else
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                currentCameraIndex = 0;
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Author: Cole Easton
    /// Displays directions for how to cycle the camera and info on the current camera
    /// </summary>
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 250, 55), "Press 'C' to change camera views\n" +
            "Camera " + currentCameraIndex + "\n" + descriptions[currentCameraIndex]);

    }

}
