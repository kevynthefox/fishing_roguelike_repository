using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class third_person_camera_rotation : MonoBehaviour
{
    [SerializeField] Transform cameraFollowTarget;
    //private player_rotation_third_person_camera input;
    private CharacterController controller;
    float xRotation;
    float yRotation;
    public Vector2 look;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotation();
        
    }

    void CameraRotation()
    {
        xRotation += look.y;
        yRotation += look.x;
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollowTarget.rotation = rotation;
    }
    

    void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
}
