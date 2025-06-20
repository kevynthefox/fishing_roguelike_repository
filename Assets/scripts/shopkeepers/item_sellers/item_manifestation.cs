using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class item_manifestation : MonoBehaviour
{
    public GameObject[] items;

    public GameObject[] item_positions;

    public Text total_owed;

    public List<InventoryItemData> items_owed;

    public GameObject checkout;

    public GameObject player;

    public GameObject wallet;

    public GameObject money_spawner_island;

    /*public GameObject item_pos_1;
    public GameObject item_pos_2;
    public GameObject item_pos_3;
    */
    public string specialty;

    public bool un_make;
    public bool checking_out;
    public bool starter = false;

    public float money_owed;

    //public float other_value;
    public float others_value_divider;
    public float others_value_2;

    public static InventorySystem current;


    public void Start()
    {
        item_maker();
    }

    public void Update()
    {
        //other_value = wallet.GetComponent<money_collector>().others_value;
        others_value_divider = wallet.GetComponent<money_collector>().others_value_divider;
        others_value_2 = wallet.GetComponent<money_collector>().others_value_2;

        if (un_make == true)
        {
            item_unmaker();
        }

        total_owed.text = "bill: " + money_owed;

        //Debug.Log(checking_out);

        if (checkout.GetComponent<object_click_detector>().left_clicked == true || checkout.GetComponent<object_click_detector>().click_override == true)// && checking_out == false)
        {
            //Debug.Log("checking_out");
            starter = true;
            StartCoroutine(checkout_part());
            
        }

        if (checking_out == true)
        {
            starter = false;
        }
    }

    public void item_maker()
    {
        

        if (un_make == false)
        {
            foreach (GameObject l in item_positions)
            {
                //Debug.Log(l);
                int randomIndex = Random.Range(0, items.Length);
                Vector3 item_position = new Vector3(l.transform.position.x, l.transform.position.y, l.transform.position.z);
                var item = Instantiate(items[randomIndex], item_position, Quaternion.identity);
            }
        }
    }

    public void item_unmaker()
    {
        foreach (var i in GameObject.FindGameObjectsWithTag(specialty))
        {
            Destroy(i);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("impact");
        if (other.tag == "currency_transit")
        {
            //Debug.Log("impact");
            money_owed -= other.GetComponent<money_value_holder>().value;
            Destroy(other.gameObject);
        }
        //yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator checkout_part()
    {
        while (starter == true)
        {
            //Debug.Log(money_owed);
            foreach (InventoryItemData item in items_owed)
            {
                
                InventorySystem.current.Add(item);
                items_owed.Remove(item);
                
            }
            //Debug.Log("second part");
            if (money_owed >= 0 && checking_out == false)
            {
                //Debug.Log("subtracting money");
                wallet.GetComponent<money_collector>().money_value -= money_owed;
                checking_out = true;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
