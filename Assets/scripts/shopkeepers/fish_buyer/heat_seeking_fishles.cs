using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class heat_seeking_fishles : MonoBehaviour
{

    public GameObject home;
    //public GameObject master;

    public float factor = 1;
    public float speed;

    public bool disable_water;

    private GameObject water;

    public Vector3 direction;
    public Vector3 direction_modified;


    public Camera cam;

    void Start()
    {
        //master = GameObject.Find("home_points");
        //home = GameObject.Find("sell guy");
        water = GameObject.Find("water");
        cam = Camera.main;
    }

    void Update()
    {
        if (disable_water == true)
        {
            if (water.tag != "Untagged")
            {
                water.tag = "Untagged";
            }
        }
        else
        {
            if (water.tag != "water")
            {
                water.tag = "water";
            }
        }

        if (home != null)
        {
            

            //factor = master.GetComponent<factor_holder>().factor;
            //Debug.Log("active");
            /*
            if (transform.position.x > home.transform.position.x)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(-Mathf.Abs(factor), 0, 0), ForceMode.Impulse);
            }

            if (transform.position.x < home.transform.position.x)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Abs(factor), 0, 0), ForceMode.Impulse);
            }


            if (transform.position.y > home.transform.position.y)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, -Mathf.Abs(factor), 0), ForceMode.Impulse);
            }

            if (transform.position.y < home.transform.position.y)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, Mathf.Abs(factor), 0), ForceMode.Impulse);
            }


            if (transform.position.z > home.transform.position.z)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -Mathf.Abs(factor)), ForceMode.Impulse);
            }

            if (transform.position.z < home.transform.position.z)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, Mathf.Abs(factor)), ForceMode.Impulse);
            }

            factor -= 0.001f;
            */
            //makes the object move faster the further away it is from the other one
            speed = Vector3.Distance(home.transform.position, transform.position);

            //moves this object towards the other object, at this speed per second
            transform.position = Vector3.MoveTowards(transform.position, home.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "npc")
        {
            //Debug.Log("triggering");
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            factor = 0;
        }

        if (this.GetComponent<Collider>() != null)
        {
            Debug.Log("i exist and am touching something");

            if (other.gameObject.tag == "fishing_rod")
            {
                Debug.Log("touching the fishing rod. state: " + other.gameObject.GetComponent<fishing_rod_movement>().blocking);

                if (other.gameObject.GetComponent<fishing_rod_movement>().blocking == false && other.gameObject.GetComponent<fishing_rod_movement>().attacking == true)
                {
                    Debug.Log("touched the rod. not blocking");
                    this.tag = "super_food_items";
                    disable_water = false;
                    home = null;
                }
                else
                {
                    Debug.Log("fling");
                    direction = cam.GetComponent<Transform>().forward;
                    direction_modified = direction * Time.deltaTime * 5000;

                    GetComponent<Rigidbody>().AddForce(direction_modified, ForceMode.Impulse);
                }
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.GetComponent<Collider>() != null)
        {
            Debug.Log("i exist and am touching something");

            if (collision.gameObject.tag == "fishing_rod")
            {
                Debug.Log("touching the fishing rod. state: " + collision.gameObject.GetComponent<fishing_rod_movement>().blocking);

                if (collision.gameObject.GetComponent<fishing_rod_movement>().blocking == false)
                {
                    Debug.Log("touched the rod. not blocking");
                    this.tag = "super_food_items";
                    disable_water = false;
                    home = null;
                }
                else
                {
                    Debug.Log("fling");
                    direction = cam.GetComponent<Transform>().forward;
                    direction_modified = direction * Time.deltaTime * 500;

                    GetComponent<Rigidbody>().AddForce(direction_modified, ForceMode.Impulse);
                }    
            }
            
        }
    }
}
