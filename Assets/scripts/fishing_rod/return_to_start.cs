using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class return_to_start : MonoBehaviour
{

    public GameObject home;
    public GameObject master;

    public float factor = 1;

    void Start()
    {
        master = GameObject.Find("home_points");
    }

    void Update()
    {
        //factor = master.GetComponent<factor_holder>().factor;
        //Debug.Log("active");

        if (transform.position.x > home.transform.position.x)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-Mathf.Abs(factor),0,0), ForceMode.Impulse);
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

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggering");
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        factor = 0;
    }
}
