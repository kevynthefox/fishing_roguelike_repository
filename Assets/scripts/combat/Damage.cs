using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private GameObject health_object;

    private void Start()
    {
        health_object = GameObject.Find("HealthBar");
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            //Debug.Log("triggered");
            if (other.tag == "fish")
            {
                health_object.GetComponent<Health_display>().health -= other.GetComponent<fish_variable_holder>().potentcy;
                yield return new WaitForSeconds(1f);
            }

            if (other.tag == "food_items")
            {
                health_object.GetComponent<Health_display>().health += other.GetComponent<fish_variable_holder>().potentcy;
                other.GetComponent<heat_seeking_fishles>().disable_water = false;
                Destroy(other.gameObject);
                yield return new WaitForSeconds(1f);
            }

            if (other.tag == "super_food_items")
            {
                health_object.GetComponent<Health_display>().health += (2 * other.GetComponent<fish_variable_holder>().potentcy);
                other.GetComponent<heat_seeking_fishles>().disable_water = false;
                Destroy(other.gameObject);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
