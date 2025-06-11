using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class redirection_for_funnymovement : MonoBehaviour
{
    /*public GameObject roof;
    public GameObject face;
    public GameObject player_hand; // come back to this one when you have a player model.
    public GameObject table_spot;
    */
    public GameObject[] places;
    public int randomindex;
    public bool first_time;

    public float timer;

    public void Update()
    {
        if (first_time == false)
        {
            randomindex = Random.Range(0, places.Length);
            first_time = true;
        }

        if (first_time == true)
        {
            if (timer > 0)
            {
                timer -= 1 * Time.deltaTime;
                Debug.Log("subtracting");
                //yield return new WaitForSeconds(1f);
            }
            else
            {
                first_time = false;
                //StartCoroutine(time_false());
            }
        }
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.tag == "currency_transit")
        {
            Debug.Log("adding");
            timer += 10 * Time.deltaTime;
            other.GetComponent<heat_seeking_money>().home = places[randomindex];
        }
        yield return null;
    }

    public IEnumerator time_false()
    {
        yield return new WaitForSeconds(3f);
        
    }
}
