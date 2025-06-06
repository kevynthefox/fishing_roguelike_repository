using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish_seller : MonoBehaviour
{

    public float money_owed;

    public GameObject bobber;

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "fish")
        {
            if (other.name == "big fish")
            {
                money_owed += other.GetComponent<fish_variable_holder>().fish_quality;
            }
            if (other.name == "small fish")
            {
                money_owed += (other.GetComponent<fish_variable_holder>().fish_quality * other.GetComponent<fish_variable_holder>().fish_quantity);
            }
            Destroy(other.gameObject);
            yield return new WaitForSeconds(1 / (bobber.GetComponent<bobber_impact>().fish_quantity_original * bobber.GetComponent<bobber_impact>().fish_quantity_original));
        }
        
    }
}
