using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_grabber : MonoBehaviour
{
    public GameObject Object_b;

    public void Update()
    {
        if (Object_b != null)
        {
            Object_b.transform.position = this.transform.position;
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (Input.GetMouseButton(2))
        {
            if (collision.gameObject.name != "Terrain" || collision.gameObject.name == "ground") //was gonna add in a "|| collision.gameObject.name != "water"" but raising the sea level is funny af
            {
                Object_b = collision.gameObject;
            }

            if (collision.gameObject.tag == "fish")
            {
                collision.gameObject.tag = "food_items";
                //collision.gameObject.GetComponent<heat_seeking_fishles>().home = null;
                Wavespawner.current.Remove_alive(collision.gameObject);
            }
        }
        else
        {
            Object_b = null;
        }
        
        
    }
}
