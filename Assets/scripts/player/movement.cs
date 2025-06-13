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

public Camera Camera;

private Rigidbody Rb;

public Vector3 cameraRelativeMovement;



    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
		
    }

	// Update is called once per frame
	void Update()
	{

        horizontalInput = Input.GetAxis("Horizontal");
		forwardInput = Input.GetAxis("Vertical");

		//moves object forward based on veritcal input
		//transform.Translate(Vector3.left * Time.deltaTime * speed * sprintspeed * forwardInput);

		//rotates the object based on horizontal input
		//transform.Translate(Vector3.forward * Time.deltaTime* speed * sprintspeed *horizontalInput );
		
		//MovePlayerRelativeToCamera();

		//Quaternion rotation = Quaternion.Euler(0,Camera.main.transform.rotation.eulerAngles.y,0);
		//Vector3 moveDirection = (rotation*Input).normalized;

		//transform.rotation  = Camera.transform.rotation;

		//jump controls
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
		{
			Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			isOnGround = false;
		}
	    cameraRelativeMovement = GetComponent<move_relative_to_camera>().cameraRelativeMovement;
        //transform.Translate(cameraRelativeMovement * Time.deltaTime * speed * sprintspeed);
        transform.Translate(cameraRelativeMovement * Time.deltaTime * speed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;
        //Debug.Log("collision: " + collision.gameObject.name);
    }

    /*
        public void MovePlayerRelativeToCamera()
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
            Vector3 cameraRelativeMovement =
                forwardRelativeMovementVector +
                RightRelativeMovementVector;

            // move in world space



        }
        */

    public IEnumerator OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger: " + other.name);
        yield return null;
    }
}
