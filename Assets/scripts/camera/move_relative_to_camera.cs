using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_relative_to_camera : MonoBehaviour
{

    public Camera Camera;
    public Vector3 cameraRelativeMovement;

    void Update()
    {
        // get player input
        float playerVerticalInput =
            Input.GetAxis("Vertical");
        float playerHorizontalInput =
            Input.GetAxis("Horizontal");

        // get camera vectors
        Vector3 cameraForward =
            Camera.main.transform.forward;
        Vector3 cameraRight =
            Camera.main.transform.right;

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
         cameraRelativeMovement = (forwardRelativeMovementVector + RightRelativeMovementVector);
        //transform.Translate(cameraRelativeMovement * Time.deltaTime * speed);
    }
}
