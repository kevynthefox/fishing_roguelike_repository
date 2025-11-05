using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish_seller : MonoBehaviour
{

    public float money_owed;

    public GameObject bobber;

    public GameObject object_holder;

    public void Awake()
    {
        object_holder = GameObject.Find("object_holder_object");
        bobber = object_holder.GetComponent<object_holder>().bobber;
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "fish")
        {
            if (other.name == "big fish")
            {
                money_owed += other.GetComponent<fish_variable_holder>().fish_quality * other.GetComponent<fish_variable_holder>().fish_quantity * other.GetComponent<fish_variable_holder>().potentcy;
            }
            if (other.name == "small fish")
            {
                money_owed += (other.GetComponent<fish_variable_holder>().fish_quality * other.GetComponent<fish_variable_holder>().fish_quantity * other.GetComponent<fish_variable_holder>().potentcy);
            }
            Destroy(other.gameObject);
            yield return new WaitForSeconds(1 / (bobber.GetComponent<fishing_script>().fish_quantity_original * bobber.GetComponent<fishing_script>().fish_quantity_original));
        }
        
    }
}
