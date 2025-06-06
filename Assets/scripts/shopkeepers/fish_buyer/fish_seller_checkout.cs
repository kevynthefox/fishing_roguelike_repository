using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish_seller_checkout : MonoBehaviour
{
    public GameObject[] money;

    public GameObject seller;

    public GameObject bobber;

    public float money_owed;

    public bool starter;

    public bool collectible;

    private void Start()
    {
        starter = true;
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        starter = true;

        if (other != null)
        {
            Vector3 spawnpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            while (starter == true)
            {
                money_owed = seller.GetComponent<fish_seller>().money_owed;

                if (other.gameObject.tag == "player")
                {
                    if (money_owed > 0)
                    {
                        collectible = false;

                        if (money_owed >= 10)
                        {
                            Instantiate(money[0], spawnpos, Quaternion.identity);
                            seller.GetComponent<fish_seller>().money_owed -= 10;
                        }
                        else
                        {
                            if (money_owed >= 1)
                            {
                                Instantiate(money[1], spawnpos, Quaternion.identity);
                                seller.GetComponent<fish_seller>().money_owed -= 1;
                            }
                            else
                            {
                                if (money_owed < 1)
                                {
                                    var money_object = Instantiate(money[1], spawnpos, Quaternion.identity);

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
                    }
                }

                yield return new WaitForSeconds(1 / bobber.GetComponent<bobber_impact>().fish_quantity_original);
            }
        }
    }
}
