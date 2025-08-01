using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject health_object;
    public bool invincibility;

    public bool trigger_or_collision; // trigger for damage being applied when touching a trigger, collision for when touching a collision.

    private void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (trigger_or_collision == false)
        {


            //Debug.Log("triggered");
            if (other.gameObject != null)
            {





                if (other.tag == "food_items")
                {
                    health_object.GetComponent<Health_display>().health += other.GetComponent<fish_variable_holder>().potentcy;// / 2;

                    Wavespawner.current.Remove_alive(other.gameObject);
                    Destroy(other.gameObject);

                }

                if (other.tag == "super_food_items")
                {
                    health_object.GetComponent<Health_display>().health += (2 * other.GetComponent<fish_variable_holder>().potentcy);// / 2;

                    Wavespawner.current.Remove_alive(other.gameObject);
                    Destroy(other.gameObject);

                }

                if (invincibility == false)
                {
                    if (other != null)
                    {
                        if (other.tag == "fish" || other.tag == "fish_enemy")
                        {
                            health_object.GetComponent<Health_display>().health -= other.GetComponent<fish_variable_holder>().potentcy;// / 2;

                        }

                        if (other.tag == "projectile")
                        {
                            health_object.GetComponent<Health_display>().health -= other.GetComponent<projectile_controller>().damage;// / 2;
                        }
                    }
                    else
                    {
                        Debug.Log("other was null");
                    }
                }
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (trigger_or_collision == true)
        {


            //Debug.Log("triggered");
            if (collision.gameObject != null)
            {





                if (collision.gameObject.tag == "food_items")
                {
                    health_object.GetComponent<Health_display>().health += collision.gameObject.GetComponent<fish_variable_holder>().potentcy;// / 2;

                    Wavespawner.current.Remove_alive(collision.gameObject);
                    Destroy(collision.gameObject);

                }

                if (collision.gameObject.tag == "super_food_items")
                {
                    health_object.GetComponent<Health_display>().health += (2 * collision.gameObject.GetComponent<fish_variable_holder>().potentcy);// / 2;

                    Wavespawner.current.Remove_alive(collision.gameObject);
                    Destroy(collision.gameObject);

                }

                if (invincibility == false)
                {
                    if (collision != null)
                    {
                        if (collision.gameObject.tag == "fish" || collision.gameObject.tag == "fish_enemy")
                        {
                            health_object.GetComponent<Health_display>().health -= collision.gameObject.GetComponent<fish_variable_holder>().potentcy;// / 2;

                        }

                        if (collision.gameObject.tag == "projectile")
                        {
                            health_object.GetComponent<Health_display>().health -= collision.gameObject.GetComponent<projectile_controller>().damage;// / 2;
                        }
                    }
                    else
                    {
                        Debug.Log("other was null");
                    }
                }
            }

        }
    }
}
