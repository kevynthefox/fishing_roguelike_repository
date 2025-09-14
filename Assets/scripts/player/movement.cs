using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{

    public float speed = 20f;
    //private float turnSpeed = 45.0f;
    public float horizontalInput;
    public float forwardInput;
    //public float sprintspeed;

	//jump related things
	public float jumpForce = 10;
	private float gravityModifier;
	public bool isOnGround = true;

    public GameObject camera_holder;

    public Camera Camera;

    private Rigidbody Rb;

    public Vector3 cameraRelativeMovement;

    public GameObject movement_target;


    // Start is called before the first frame update
    void Start()
    {
        movement_target = this.gameObject;

        Rb = GetComponent<Rigidbody>();

        camera_holder = GameObject.Find("camera system");

        Camera = camera_holder.GetComponent<camera_holder>().first_person;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponentInChildren<Health_display>().dead == false)
        {

            //horizontalInput = Input.GetAxis("Horizontal");
            //forwardInput = Input.GetAxis("Vertical");


            cameraRelativeMovement = GetComponent<move_relative_to_camera>().cameraRelativeMovement;

            movement_target.transform.Translate(cameraRelativeMovement * Time.deltaTime * speed);
        }
    }

    private void Update()
    {
        if (GetComponentInChildren<Health_display>().dead == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;
       
    }

    

    public IEnumerator OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger: " + other.name);
        yield return null;
    }
}
