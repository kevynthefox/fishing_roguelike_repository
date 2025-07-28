using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private GameObject health_object;
    public bool invincibility;

    private void Start()
    {
        health_object = GameObject.Find("HealthBar");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {

            if (other.isTrigger == true)
            {
                //Debug.Log("triggered");


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
                }
            }
        }
    }
}
