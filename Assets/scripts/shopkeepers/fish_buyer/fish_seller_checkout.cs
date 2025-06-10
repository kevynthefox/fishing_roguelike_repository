using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class fish_seller_checkout : MonoBehaviour
{
    public GameObject[] money;

    public GameObject seller;

    public GameObject bobber;

    public GameObject spawn_area;

    //public GameObject absorb_area;

    public float money_owed;

    public bool starter;

    public bool collectible;

    public float wait;

    private void Start()
    {
        starter = true;
    }

    public void Update()
    {
        rotation(spawn_area.transform.rotation.x, spawn_area.transform.rotation.y, spawn_area.transform.rotation.z);
    }

    public static Quaternion rotation(float x, float y, float z)
    {
        return new Quaternion(x,y,z, 1);
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        starter = true;

        if (other != null)
        {
            //Vector3 spawnpos = new Vector3(spawn_area.transform.position.x, spawn_area.transform.position.y, spawn_area.transform.position.z);
            while (starter == true)
            {
                
                Vector3 spawnpos = new Vector3(spawn_area.transform.position.x, spawn_area.transform.position.y, spawn_area.transform.position.z);
                //Quaternion rotation = new Quaterion(spawn_area.transform.rotation.x, spawn_area.transform.rotation.y, spawn_area.transform.rotation.z, 0f);
                money_owed = seller.GetComponent<fish_seller>().money_owed;

                if (other.gameObject.tag == "player")
                {
                    if (money_owed > 0)
                    {
                        //absorb_area.SetActive(true);
                        collectible = false;

                        if (money_owed >= 10)
                        {
                            var money_object =Instantiate(money[0], spawnpos, rotation(spawn_area.transform.rotation.x, spawn_area.transform.rotation.y, spawn_area.transform.rotation.z));
                            money_object.GetComponent<heat_seeking_money>().home = GameObject.Find("bone_pile");
                            seller.GetComponent<fish_seller>().money_owed -= 10;
                            yield return new WaitForSeconds(wait);
                            
                        }
                        else
                        {
                            if (money_owed >= 1)
                            {
                                var money_object = Instantiate(money[1], spawnpos, Quaternion.identity);
                                money_object.GetComponent<heat_seeking_money>().home = GameObject.Find("bone_pile");
                                seller.GetComponent<fish_seller>().money_owed -= 1;
                                yield return new WaitForSeconds(wait);
                                
                            }
                            else
                            {
                                if (money_owed < 1)
                                {
                                    var money_object = Instantiate(money[1], spawnpos, Quaternion.identity);
                                    money_object.GetComponent<heat_seeking_money>().home = GameObject.Find("bone_pile");
                                    money_object.GetComponent<money_value_holder>().value = money_owed;
                                    money_object.GetComponent<Transform>().localPosition = new Vector3(money_owed, money_owed, money_owed);
                                    
                                    seller.GetComponent<fish_seller>().money_owed -= money_owed;
                                }
                            }
                        }
                    }
                    else
                    {
                        collectible = true;
                        yield return new WaitForSeconds(5f);
                        //absorb_area.SetActive(false);
                    }
                }
                wait = 1 / bobber.GetComponent<bobber_impact>().fish_quantity_original;
                yield return new WaitForSeconds(wait);
            }
        }
    }
}
