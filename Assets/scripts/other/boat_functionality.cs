//using System.Numerics;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class boat_functionality : MonoBehaviour
{
    public GameObject camera_holder;
    public CinemachineCamera Camera;

    public GameObject boat_visible;

    public Vector3 direction;
    public Vector3 direction_modified;

    public float player_speed;

    public float boat_speed;

    public bool boat_yes;

    void Start()
    {
        camera_holder = GameObject.Find("camera system");

        Camera = camera_holder.GetComponent<camera_holder>().CinemachineCamera;
    }

    void Update()
    {
        if (TimeManager.current.update == true)
        {
            direction = Camera.GetComponent<Transform>().forward;
            direction_modified = direction * Time.deltaTime * boat_speed;


        }
    }

    public void FixedUpdate()
    {
        if (TimeManager.current.update == true)
        {
            if (boat_yes == true)
            {
                var forcedirection = Vector3.forward;
                var steer = 0;
                var move = 0;
                if (Input.GetKey(KeyCode.A))
                {
                    steer = -1;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    steer = +1;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    move = -1;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    move = +1;
                }
                transform.Rotate(0, steer, 0);
                transform.Translate(forcedirection * move * boat_speed * Time.deltaTime);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "player")
        {
            player_speed = other.GetComponent<movement>().speed;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "player")
        {
            boat_yes = false;
            other.transform.rotation = Quaternion.identity;
            other.transform.localScale = Vector3.one;

            Debug.Log("player exited");

            other.GetComponent<movement>().movement_target = other.gameObject;
            //other.GetComponent<movement>().speed = player_speed;

            GetComponent<move_relative_to_camera>().in_boat = boat_yes;

            other.GetComponent<movement>().enabled = !boat_yes;
            //GetComponent<movement>().enabled = boat_yes;
            other.transform.SetParent(null, true);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "player")
        {

            GetComponent<move_relative_to_camera>().in_boat = boat_yes;
            other.GetComponent<movement>().enabled = !boat_yes;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                boat_yes = !boat_yes;
            }

            if (boat_yes == true)
            {
                

                Debug.Log("player entered");

                //other.GetComponent<movement>().movement_target = this.gameObject;

                //other.GetComponent<movement>().speed = boat_speed;
                
                //GetComponent<movement>().enabled = boat_yes;
                

                other.transform.SetParent(this.transform, true);

                


                
            }
            else
            {

                other.transform.rotation = Quaternion.identity;
                //other.transform.localScale = Vector3.one;
                Debug.Log("player not in boat");

                //other.GetComponent<movement>().movement_target = other.gameObject;
                //other.GetComponent<movement>().speed = player_speed;

                
                other.transform.SetParent(null, true);

                
            }
        }
    }

}


