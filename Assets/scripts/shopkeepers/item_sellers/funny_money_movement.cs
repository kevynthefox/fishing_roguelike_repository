using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class funny_money_movement : MonoBehaviour
{
    //public Text money_text;

    //public GameObject currency_holder;

    public GameObject self;
    public GameObject reroll;
    public GameObject redirect;

    public GameObject[] money;

    public GameObject spawn_area;

    public GameObject shopkeeper;


    public bool starter;


    public float money_owed;
    public float reroll_cost;

    public GameObject last_clicked;

    public void Start()
    {
        starter = true;

        StartCoroutine(spawn_logic());
        StartCoroutine(click_detection());
    }

    /*public void Update()
    {
        if (self.GetComponent<object_click_detector>().left_clicked == true)
        {
            StartCoroutine(money_spawn());
        }
    }*/

    public IEnumerator click_detection()
    {
        while (starter == true)
        {
            if (self.GetComponent<object_click_detector>().left_clicked == true)
            {
                last_clicked = self;

                money_owed = shopkeeper.GetComponent<item_manifestation>().money_owed;

                //Vector3 spawnpos = new Vector3(spawn_area.transform.position.x, spawn_area.transform.position.y * 2, spawn_area.transform.position.z);
                //Quaternion rotation = new Quaterion(spawn_area.transform.rotation.x, spawn_area.transform.rotation.y, spawn_area.transform.rotation.z, 0f);
                StartCoroutine(spawn_logic());
                self.GetComponent<object_click_detector>().click_override = true;
            }

            if (reroll.GetComponent<object_click_detector>().left_clicked == true)
            {
                last_clicked = reroll;
                //Debug.Log(reroll.GetComponent<object_click_detector>().left_clicked);
                yield return new WaitForSeconds(4f);
                money_owed = reroll.GetComponent<rerolling>().item_cost;
                StartCoroutine(spawn_logic());
                reroll.GetComponent<object_click_detector>().click_override = true;
            }
            yield return new WaitForSeconds(0.0001f);
        }
    }

    public IEnumerator spawn_logic()
    {

        //Vector3 spawnpos = new Vector3(spawn_area.transform.position.x, spawn_area.transform.position.y, spawn_area.transform.position.z);
        /*if (self.GetComponent<object_click_detector>().left_clicked == true)
        {
            money_owed = shopkeeper.GetComponent<item_manifestation>().money_owed;

            Vector3 spawnpos = new Vector3(spawn_area.transform.position.x, spawn_area.transform.position.y * 2, spawn_area.transform.position.z);
            //Quaternion rotation = new Quaterion(spawn_area.transform.rotation.x, spawn_area.transform.rotation.y, spawn_area.transform.rotation.z, 0f);
        } */

        //Debug.Log(money_owed);
        while (money_owed > 0)
        {
            //Debug.Log("active");
            //absorb_area.SetActive(true);
                

            if (money_owed >= 10)
            {
                    
                StartCoroutine(spawn(0,10));
                StopCoroutine(spawn(0,10));
            }
            else
            {
                if (money_owed >= 1)
                {

                    //Debug.Log("instantiated size 1 money");
                    StartCoroutine(spawn(1, 1));
                    StopCoroutine(spawn(1, 1));
                }
                else
                {
                    if (money_owed > 0)
                    {
                        //money_owed -= money_owed;
                        StartCoroutine(spawn(1,money_owed));
                        StopCoroutine(spawn(1, money_owed));
                    }
                }
                    
            }
            yield return new WaitForSeconds(0.1f);
        }
        while (money_owed <= 0)
        {
            
            self.GetComponent<object_click_detector>().click_override = false;
            reroll.GetComponent<object_click_detector>().click_override = false;


            shopkeeper.GetComponent<item_manifestation>().checking_out = false;
            yield return new WaitForSeconds(1f);
            
            //yield return new WaitForSeconds(5f);
            //absorb_area.SetActive(false);
        }
            
        //wait = 1 / bobber.GetComponent<bobber_impact>().fish_quantity_original;
        //yield return new WaitForSeconds(wait);
        
    }

    public IEnumerator spawn(int i, float a)
    {
        Vector3 spawnpos = new Vector3(spawn_area.transform.position.x, spawn_area.transform.position.y * 2, spawn_area.transform.position.z);
        
        //money_owed = shopkeeper.GetComponent<item_manifestation>().money_owed;
        var money_object = Instantiate(money[i], spawnpos, Quaternion.identity);
        if (last_clicked == self)
        {
            money_object.GetComponent<heat_seeking_money>().home = redirect;
        }
        if (last_clicked == reroll)
        {
            money_object.GetComponent<heat_seeking_money>().home = reroll;
        }
        money_object.GetComponent<Rigidbody>().useGravity = false;
        money_object.tag = "currency_transit";

        if (money_owed < 1 && i == 1)
        {
            //Debug.Log("small money");
            money_object.GetComponent<money_value_holder>().value = money_owed;
            money_object.GetComponent<Transform>().localPosition = new Vector3(money_owed, money_owed, money_owed);
        }

        money_owed -= a;

        yield return new WaitForSeconds(0.1f);
        
    }
}
