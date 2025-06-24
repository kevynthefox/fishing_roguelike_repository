using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class return_to_start : MonoBehaviour
{

    public GameObject home;
    public GameObject master;

    public float factor = 1;
    public float speed;

    public bool mode;

    void Start()
    {
        master = GameObject.Find("Bone.002");
    }

    void Update()
    {
        if (mode == false)
        {
            //makes the object move faster the further away it is from the other one
            speed = Vector3.Distance(home.transform.position, transform.position);

            //moves this object towards the other object, at this speed per second
            transform.position = Vector3.MoveTowards(transform.position, home.transform.position, speed * Time.deltaTime);
        }

        //factor = master.GetComponent<factor_holder>().factor;
        //Debug.Log("active");
        mode = master.GetComponent<return_to_start>().mode;
        if (mode == true)
        {
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
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggering");
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        factor = 0;
    }
}
