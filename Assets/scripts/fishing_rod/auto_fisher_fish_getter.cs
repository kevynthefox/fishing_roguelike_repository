using System.Collections;
using UnityEngine;

public class auto_fisher_fish_getter : MonoBehaviour
{
    public GameObject[] fish_to_spawn;

    public GameObject rod;



    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("touched something");
        if (collision.gameObject.CompareTag("water"))
        {
            //Debug.Log("collision was water");
            fish_to_spawn = collision.gameObject.GetComponent<fishing_area_value_holder>().fish;
        }

    }





    public void clearlist()
    {
        fish_to_spawn = null;
    }
}
