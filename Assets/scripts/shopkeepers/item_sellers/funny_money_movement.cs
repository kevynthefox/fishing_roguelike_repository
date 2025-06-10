using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class funny_money_movement : MonoBehaviour
{
    //public Text money_text;

    //public GameObject currency_holder;

    public GameObject self;

    public GameObject[] money;

    public GameObject spawn_area;

    public GameObject shopkeeper;


    public bool starter;


    public float money_owed;


    public void Start()
    {
        starter = true;
    }

    /*public void Update()
    {
        if (self.GetComponent<object_click_detector>().left_clicked == true)
        {
            StartCoroutine(money_spawn());
        }
    }*/

    public void Update()
    {
        money_owed = shopkeeper.GetComponent<item_manifestation>().money_owed;
        //Vector3 spawnpos = new Vector3(spawn_area.transform.position.x, spawn_area.transform.position.y, spawn_area.transform.position.z);
        if (self.GetComponent<object_click_detector>().left_clicked == true)
        {

            Vector3 spawnpos = new Vector3(spawn_area.transform.position.x, spawn_area.transform.position.y *2, spawn_area.transform.position.z);
            //Quaternion rotation = new Quaterion(spawn_area.transform.rotation.x, spawn_area.transform.rotation.y, spawn_area.transform.rotation.z, 0f);
            

            
            if (money_owed > 0)
            {
                //absorb_area.SetActive(true);
                

                if (money_owed >= 10)
                {
                    
                    spawn(0, 10);
                }
                else
                {
                    if (money_owed >= 1)
                    {
                        
                        //Debug.Log("instantiated size 1 money");
                        spawn(1,1);
                    }
                    else
                    {
                        if (money_owed > 0)
                        {
                            money_owed -= money_owed;
                            spawn(1,money_owed);
                        }
                    }
                    
                }
            }
            else
            {
                shopkeeper.GetComponent<item_manifestation>().checking_out = false;
                //yield return new WaitForSeconds(5f);
                //absorb_area.SetActive(false);
            }
            
            //wait = 1 / bobber.GetComponent<bobber_impact>().fish_quantity_original;
            //yield return new WaitForSeconds(wait);
        }
    }

    public void spawn(int i, float a)
    {
        Vector3 spawnpos = new Vector3(spawn_area.transform.position.x, spawn_area.transform.position.y * 2, spawn_area.transform.position.z);
        
        money_owed = shopkeeper.GetComponent<item_manifestation>().money_owed;
        var money_object = Instantiate(money[i], spawnpos, Quaternion.identity);

        money_object.GetComponent<heat_seeking_money>().home = GameObject.Find("roof");
        money_object.GetComponent<Rigidbody>().useGravity = false;
        money_object.tag = "currency_transit";
        if (money_owed < 1)
        {
            
            money_object.GetComponent<money_value_holder>().value = money_owed;
            money_object.GetComponent<Transform>().localPosition = new Vector3(money_owed, money_owed, money_owed);
        }
    }
}
