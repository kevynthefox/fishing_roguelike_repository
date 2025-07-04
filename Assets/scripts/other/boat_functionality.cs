//using System.Numerics;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

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
        direction = Camera.GetComponent<Transform>().forward;
        direction_modified = direction * Time.deltaTime * boat_speed;

        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "player")
        {
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
            other.GetComponent<movement>().speed = player_speed;

           
            other.transform.SetParent(null, true);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "player")
        {
            
            
            

            if (Input.GetKey(KeyCode.Tab))
            {
                boat_yes = !boat_yes;
            }

            if (boat_yes == true)
            {


                Debug.Log("player entered");

                other.GetComponent<movement>().movement_target = this.gameObject;
                player_speed = other.GetComponent<movement>().speed;
                other.GetComponent<movement>().speed = boat_speed;

                

                other.transform.SetParent(this.transform, true);

                


                var forcedirection = Vector3.forward;
                var steer = 0;
                
                if (Input.GetKey(KeyCode.A))
                {
                    steer = -1;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    steer = +1;
                }
                transform.Rotate(0, steer, 0);
            }
            else
            {

                other.transform.rotation = Quaternion.identity;
                //other.transform.localScale = Vector3.one;
                Debug.Log("player not in boat");

                other.GetComponent<movement>().movement_target = other.gameObject;
                other.GetComponent<movement>().speed = player_speed;

                
                other.transform.SetParent(null, true);

                
            }
        }
    }

}
