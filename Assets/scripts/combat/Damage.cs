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

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            //Debug.Log("triggered");
            

            if (other.tag == "food_items")
            {
                health_object.GetComponent<Health_display>().health += other.GetComponent<fish_variable_holder>().potentcy / 2;
                
                Wavespawner.current.Remove_alive(other.gameObject);
                Destroy(other.gameObject);
                yield return new WaitForSeconds(1f);
            }

            if (other.tag == "super_food_items")
            {
                health_object.GetComponent<Health_display>().health += (2 * other.GetComponent<fish_variable_holder>().potentcy) / 2;
                
                Wavespawner.current.Remove_alive(other.gameObject);
                Destroy(other.gameObject);
                yield return new WaitForSeconds(1f);
            }

            if (invincibility == false)
            {

                if (other.tag == "fish" || other.tag == "fish_enemy")
                {
                    health_object.GetComponent<Health_display>().health -= other.GetComponent<fish_variable_holder>().potentcy / 2;
                    yield return new WaitForSeconds(1f);
                }

                if (other.tag == "projectile")
                {
                    health_object.GetComponent<Health_display>().health -= other.GetComponent<projectile_controller>().damage / 2;
                }
            }
        }
    }
}
