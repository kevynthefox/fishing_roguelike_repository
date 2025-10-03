using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class camera_controls : MonoBehaviour
{

    public GameObject camera_holder;

    public CinemachineCamera cam1;
    public Camera cam2;
    public Vector2 look;

    //public Transform first_person_target;
    //public Transform third_person_target;

    public GameObject player_model;

    public bool first_or_third; //false is first, true is third.

    //[SerializeField] private CinemachineCamera cam;

    // Start is called before the first frame update
    void Start()
    {
        camera_holder = GameObject.Find("camera system");

        cam1 = camera_holder.GetComponent<camera_holder>().CinemachineCamera;
        cam2 = camera_holder.GetComponent<camera_holder>().third_person;

        //first_person_target = GameObject.Find("1st_person_follow_target").transform;
        //third_person_target = GameObject.Find("3rd_person_follow_target").transform;

        cam1.enabled = true;
    	cam2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
	    if (Input.GetKeyDown(KeyCode.F))
	    {
            cam1.enabled = !cam1.enabled;
		    cam2.enabled = !cam2.enabled;
            cam2.GetComponent<FreeFlyCamera>().enabled = cam2.enabled;

            first_or_third = !first_or_third;

            

	    }

        //GetComponent<movement>().enabled = !cam2.enabled;
        //GetComponent<move_relative_to_camera>().enabled = !cam2.enabled;
        //cam1.GetComponent<camera_rotate>().enabled = !cam2.enabled;
        player_model.GetComponent<attach_to_object>().enabled = !cam2.enabled;
        cam2.GetComponent<attach_to_object>().enabled = !cam2.enabled;
        

        /*if (first_or_third == false)
        {
            cam1.GetComponent<CinemachineCamera>().Target.TrackingTarget = first_person_target;
            cam1.GetComponent<CinemachineCamera>().Target.LookAtTarget = null;
            cam1.GetComponent<CinemachineHardLockToTarget>().enabled = true;
            cam1.GetComponent<CinemachinePanTilt>().enabled = true;
            //cam1.GetComponent<CinemachineInputAxisController>().
            //cam1.GetComponent<CinemachineCamera>().Follow = cam1.GetComponent<CinemachineThirdPersonFollow>().FollowTarget;

        }
        if (first_or_third == true)
        {
            cam1.GetComponent<CinemachineCamera>().Target.TrackingTarget = third_person_target;
            cam1.GetComponent<CinemachineCamera>().Target.LookAtTarget = GameObject.Find("player").transform;
            cam1.GetComponent<CinemachineThirdPersonFollow>().enabled = true;
            cam1.GetComponent<CinemachineHardLookAt>().enabled = true;
            // cam.GetCinemachineComponent<CinemachineCamera>().FollowTargetPosition = third_person_follow;
            cam1.GetComponent<CinemachineCamera>().Follow = cam1.GetComponent<CinemachineThirdPersonFollow>().FollowTarget;
        }*/

    }
    

    void OnLook(InputValue value )
    {
        look = value.Get<Vector2>();
    }
}
