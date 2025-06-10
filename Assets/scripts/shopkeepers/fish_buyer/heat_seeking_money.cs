using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heat_seeking_money : MonoBehaviour
{

    public GameObject home;
    //public transform home;
    public float speed;
    //public GameObject master;

    public float factor = 1;

    void Start()
    {
        //master = GameObject.Find("home_points");
        //home = GameObject.Find("bone_pile");
        //StartCoroutine(seek());
    }

    public void Update()
    {
        //makes the object move faster the further away it is from the other one
        speed = Vector3.Distance(home.transform.position, transform.position);

        //moves this object towards the other object, at this speed per second
        transform.position = Vector3.MoveTowards(transform.position, home.transform.position, speed * Time.deltaTime);
    }
    /*seek()
    {
        //factor = master.GetComponent<factor_holder>().factor;
        //Debug.Log("active");

        while (home != null)
        {
            
            while (transform.position.x > home.transform.position.x)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(-Mathf.Abs(transform.position.x - home.transform.position.x), 0, 0), ForceMode.Acceleration);
                yield return new WaitForSeconds(1/(Mathf.Abs(transform.position.x) - Mathf.Abs(home.transform.position.x)));
            }

            while (transform.position.x < home.transform.position.x)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Abs(home.transform.position.x - transform.position.x), 0, 0), ForceMode.Acceleration);
                yield return new WaitForSeconds(1/(Mathf.Abs(home.transform.position.x) - Mathf.Abs(transform.position.x)));
            }


            while (transform.position.y > home.transform.position.y)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, -Mathf.Abs(transform.position.y - home.transform.position.y), 0), ForceMode.Acceleration);
                yield return new WaitForSeconds(1/(Mathf.Abs(transform.position.y) - Mathf.Abs(home.transform.position.y)));
            }

            while (transform.position.y < home.transform.position.y)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, Mathf.Abs(home.transform.position.y - transform.position.y), 0), ForceMode.Acceleration);
                yield return new WaitForSeconds(1 / (Mathf.Abs(home.transform.position.y) - Mathf.Abs(transform.position.y)));
            }


            while (transform.position.z > home.transform.position.z)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -Mathf.Abs(transform.position.z - home.transform.position.z)), ForceMode.Acceleration);
                yield return new WaitForSeconds(1 / (Mathf.Abs(transform.position.z) - Mathf.Abs(home.transform.position.z)));
            }

            while (transform.position.z < home.transform.position.z)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, Mathf.Abs(home.transform.position.z - transform.position.z)), ForceMode.Acceleration);
                yield return new WaitForSeconds(1 / (Mathf.Abs(home.transform.position.z) - Mathf.Abs(transform.position.z)));
            }

            //factor -= 0.001f;
            yield return new WaitForSeconds(1f);

            

            Debug.Log(1/(Mathf.Abs(home.transform.position.y) - Mathf.Abs(transform.position.y)));
        }
    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other == home)
        {
            //Debug.Log("triggering");
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            factor = 0;
        }
    }
}
