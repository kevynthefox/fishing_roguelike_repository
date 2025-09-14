using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class item_buying : MonoBehaviour
{
    public bool left_clicked;

    public GameObject self;
    public InventoryItemData self_item;

    public GameObject player;
    public GameObject wallet;


    public float item_cost;
    

    

    public GameObject gamesettings;

    public GameObject shop;
    public string group;

    private void Start()
    {
        player = GameObject.Find("player");
        wallet = GameObject.Find("bone_pile");
        shop = GameObject.Find(group);
        gamesettings = GameObject.Find("game_settings");

        
    }

    public void LateUpdate()
    {
        if (TimeManager.current.update == true)
        {
            gamesettings = GameObject.Find("game_settings");

            item_cost = GetComponent<item_price_holder>().item_cost;


        }
    }

    public IEnumerator OnMouseOver()
    {
        if (this.gameObject.GetComponent<item_buying>().enabled == true)
        {
            Debug.Log("mouse is over, item_buying");
            if (Input.GetMouseButton(0))
            {
                if (wallet != null)
                {
                    if (wallet.GetComponent<money_collector>().money_value >= item_cost)
                    {
                        if (shop != null)
                        {
                            if (self_item.toggleable == true)
                            {
                                self_item.toggleOffOn = false;
                            }
                            shop.GetComponent<item_manifestation>().money_owed += item_cost;

                            shop.GetComponent<item_manifestation>().items_owed.Add(self_item);
                        }
                        Destroy(self);
                    }
                }
            }


            yield return null;
        }
    }

}
