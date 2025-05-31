using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class variable_length : MonoBehaviour
{
    public float distance;
    public bool enabled_fishing;
    public Vector3 velocity;

    public GameObject bobber;

    public GameObject master;

    public bool bobber_returned;
    

    // Start is called before the first frame update
    void Start()
    {
        enabled_fishing = false;
        //GetComponent<Rigidbody>().isKinematic = !enabled_fishing;
        master = GameObject.Find("Bone.002");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpringJoint>().maxDistance = distance / 10;
        GetComponent<Rigidbody>().useGravity = enabled_fishing;
        velocity = GetComponent<Rigidbody>().velocity;
        GetComponent<SpringJoint>().spring = 1000 - distance * 10;
        GetComponent<SpringJoint>().damper = 1000 + distance * 10;

        enabled_fishing = master.GetComponent<variable_length>().enabled_fishing;

        if (enabled_fishing == true)
        {
            if (Input.GetMouseButton(0)) //(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                distance += 1;
            }

            if (Input.GetMouseButton(1)) //(Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                distance -= 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            enabled_fishing = !enabled_fishing;
            
            //GetComponent<Rigidbody>().isKinematic = !enabled_fishing;
        }

        if (enabled_fishing == false)
        {
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            distance = 0;
        }
        
        GetComponent<return_to_start>().enabled = !enabled_fishing;

        bobber_returned = bobber.GetComponent<bobber_impact>().returned;
        
        if (bobber_returned == true)
        {
            StartCoroutine(wait_then_reset());
        }

    }

    IEnumerator wait_then_reset()
    {
        yield return new WaitForSeconds(bobber.GetComponent<bobber_impact>().fishing_time_cool);
        distance = 0;
    }    

   // IEnumerator rewind()
    //{
        //GetComponent<Rigidbody>().velocity = new Vector3(0,distance * distance,0);
       
        //yield return new WaitForSeconds(distance);
    //}

    //IEnumerator launch()
    //{
     //   distance += 1;
     //   yield return new WaitForSeconds(0.001f);
    //}    
}
