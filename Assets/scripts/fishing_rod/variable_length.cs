using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class variable_length : MonoBehaviour
{
    public float distance;
    public bool enabled_fishing;
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        enabled_fishing = false;
        //GetComponent<Rigidbody>().isKinematic = !enabled_fishing;

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpringJoint>().maxDistance = distance / 10;
        GetComponent<Rigidbody>().useGravity = enabled_fishing;
        velocity = GetComponent<Rigidbody>().velocity;
        GetComponent<SpringJoint>().spring = 1000 - distance * 10;
        GetComponent<SpringJoint>().damper = 1000 + distance * 10;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            distance += 1;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            distance -= 1;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            enabled_fishing = !enabled_fishing;
            transform.Rotate(0, 0, 0);
            transform.Translate(0, 0, 0);

            rewind();
            GetComponent<Rigidbody>().isKinematic = !enabled_fishing;
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            //distance = 0;
        }

    }

    IEnumerator rewind()
    {
        //GetComponent<Rigidbody>().velocity = new Vector3(0,distance * distance,0);
       
        yield return new WaitForSeconds(distance);
    }
}
