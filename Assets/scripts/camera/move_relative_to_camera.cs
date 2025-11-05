using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class move_relative_to_camera : MonoBehaviour
{
    public GameObject camera_holder;
    public CinemachineCamera Camera;
    public Vector3 cameraRelativeMovement;

    public bool in_boat;

    public void Start()
    {
        camera_holder = GameObject.Find("camera system");

        Camera = camera_holder.GetComponent<camera_holder>().CinemachineCamera;
    }

    void FixedUpdate()
    {
        // get player input
        float playerVerticalInput =
            Input.GetAxis("Vertical");
        float playerHorizontalInput =
            Input.GetAxis("Horizontal");

        // get camera vectors
        Vector3 cameraForward =
            Camera.transform.forward;
        Vector3 cameraRight =
            Camera.transform.right;

        // remove y and normalize
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        // rotate the input vectors
        Vector3 forwardRelativeMovementVector =
            playerVerticalInput * cameraForward;
        Vector3 RightRelativeMovementVector =
            playerHorizontalInput * cameraRight;

        // create camera-relative moevement vector
        if (in_boat == true)
        {
            cameraRelativeMovement = (forwardRelativeMovementVector);
        }
        else
        {
            cameraRelativeMovement = (forwardRelativeMovementVector + RightRelativeMovementVector);
        }
        //transform.Translate(cameraRelativeMovement * Time.deltaTime * speed);
    }
}
