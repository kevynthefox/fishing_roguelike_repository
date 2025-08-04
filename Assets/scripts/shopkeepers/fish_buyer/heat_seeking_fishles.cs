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

    public float health = 1;


    public bool enemy;

    public List<GameObject> targets;




    void Start()
    {
        //master = GameObject.Find("home_points");
        //home = GameObject.Find("sell guy");


    }

    void Update()
    {


        if (home != null)
        {



            //makes the object move faster the further away it is from the other one
            speed = Vector3.Distance(home.transform.position, transform.position);

            //moves this object towards the other object, at this speed per second
            transform.position = Vector3.MoveTowards(transform.position, home.transform.position, speed * Time.deltaTime);
            //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.position, home.transform.position, 0, 360));
            transform.LookAt(home.transform); // you need to child the object to an empty gameobject so that the object maintains the rotation you want.
        }

        if (home != null && enemy == false)
        {
            StartCoroutine(failsafe_counter());

        }

        if (enemy == true && home == null)
        {
            foreach (GameObject potential_target in GameObject.FindGameObjectsWithTag("player"))
            {

                if (targets.Contains(potential_target) == false)
                {
                    targets.Add(potential_target);
                }

            }

            int random_target = UnityEngine.Random.Range(0, targets.Count);

            home = targets[random_target];
        }

        if (health == 0)
        {
            home = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == true)
        {
            //Debug.Log("this fish collided with: " + other.gameObject.name);
        }
        if (other.gameObject.tag == "npc")
        {
            //Debug.Log("triggering");
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            factor = 0;
        }
        if (other.gameObject.tag == "projectile" || other.gameObject.tag == "fishing_rod")
        {
            this.tag = "super_food_items";
            Wavespawner.current.Remove_alive(this.gameObject);
            home = null;
            health = 0;
        }

    }


    public IEnumerator failsafe_counter()
    {
        
        yield return new WaitForSeconds(1);
        //Debug.Log("failed the safe");
        GetComponent<Rigidbody>().useGravity = true;

    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (this.GetComponent<Collider>() != null)
        {
            //Debug.Log("i exist and am touching something");

            if (collision.gameObject.tag == "fishing_rod")
            {
                //Debug.Log("touching the fishing rod. state: " + collision.gameObject.GetComponent<fishing_rod_movement>().blocking);

                if (collision.gameObject.GetComponent<fishing_rod_movement>().blocking == false)
                {
                    //Debug.Log("touched the rod. not blocking");
                    this.tag = "super_food_items";
                    disable_water = false;
                    home = null;
                }
                else
                {
                    //Debug.Log("fling");
                    direction = cam.GetComponent<Transform>().forward;
                    direction_modified = direction * Time.deltaTime * 500;

                    GetComponent<Rigidbody>().AddForce(direction_modified, ForceMode.Impulse);
                }    
            }
            
        }
    }*/
}
