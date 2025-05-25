using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    private PlayerInputsManager input;
    private CharacterController controller;
    public float speed;
    public float sprintspeed;

    [SerializeField] Transform cameraFollowTarget;
    [SerializeField] GameObject mainCam;
    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInputsManager>();
        controller = GetComponent<CharacterController>();
        speed = GetComponent<movement>().speed;
        sprintspeed = GetComponent<movement>().sprintspeed;
    }

    // Update is called once per frame
    void Update()
    {
        speed = 0;
        Vector3 inputDirection = new Vector3(input.move.x, 0, input.move.y);
        float targetRotation = 0;
        
        if(input.move != Vector2.zero)
        {
            speed = GetComponent<movement>().speed;
            targetRotation = Quaternion.LookRotation(inputDirection).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation,0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
        }


        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        controller.Move(targetDirection * speed * sprintspeed * Time.deltaTime);
        Debug.Log("targetDirection" + targetDirection);
    }
    void LateUpdate()
    {
        CameraRotation();

    }
    void CameraRotation()
    {
        xRotation += input.look.y;
        yRotation += input.look.x;
        //xRotation = Mathf.Clamp(xRotation, -30, 70);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollowTarget.rotation = rotation;
    }
}
