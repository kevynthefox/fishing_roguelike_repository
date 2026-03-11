using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_grabber : MonoBehaviour
{
    public GameObject Object_b;
    public List<GameObject> blacklisted;

    public float speed;

    public void Update()
    {
        if (TimeManager.current.update == true)
        {
            if (Object_b != null)
            {
                //Object_b.transform.position = this.transform.position;
                speed = Vector3.Distance(this.transform.position, transform.position);
                Object_b.transform.position = Vector3.MoveTowards(transform.position, this.transform.position, speed * Time.deltaTime);
            }
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (Input.GetMouseButton(2))
        {
            
            //Debug.Log("current blacklist: " + b.name);
            if (collision.gameObject != blacklisted[0] && collision.gameObject != blacklisted[1] && collision.gameObject != blacklisted[2] && collision.gameObject.tag != "water" && collision.gameObject.tag != "water_off")
            {
                //Debug.Log("dragged: " + b.name + " actual: " + collision.gameObject.name);
                Object_b = collision.gameObject;
            }
            
            if (collision.gameObject.tag == "fish")
            {
                collision.gameObject.tag = "food_items";
                //collision.gameObject.GetComponent<heat_seeking_fishles>().target = null;
                Wavespawner.current.Remove_alive(collision.gameObject);
            }

            if (collision.gameObject.name == "COD")
            {
                if (collision.gameObject.GetComponent<COD>().collided_with_wall == false)
                {
                    Vector3 spawn_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    int randomIndex = Random.Range(0, GetComponent<fishing_script>().fish.Length);
                    var fish_object = Instantiate(this.GetComponent<fishing_script>().fish[randomIndex], spawn_pos, Quaternion.identity);
                    collision.gameObject.GetComponent<COD>().size -= fish_object.GetComponent<fish_variable_holder>().fish_quality;
                    fish_object.GetComponent<heat_seeking_fishles>().target = GameObject.Find("sell guy");
                }
            }
        }
        else
        {
            Object_b = null;
        }
        
        
    }
}
