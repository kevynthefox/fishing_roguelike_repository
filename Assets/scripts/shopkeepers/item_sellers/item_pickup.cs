using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class item_pickup : MonoBehaviour
{
    public bool left_clicked;
    public bool touched;

    public InventoryItemData self_item;

    public int amount_of_items;

    public float sell_value;

    private GameObject wallet;



    private void Awake()
    {
        wallet = GameObject.Find("bone_pile");

    }

 

    public IEnumerator OnMouseOver()
    {
        if (this.gameObject.GetComponent<item_pickup>().enabled == true)
        {
            if (TryGetComponent<item_price_holder>(out item_price_holder price_holder))
            {
                sell_value = price_holder.item_cost;
            }


            //Debug.Log("mouse is over, item pickup");
            if (Input.GetMouseButton(0))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    
                    for (int i = 0; i <= amount_of_items; i++)
                    {
                        if (self_item.toggleable == true)
                        {
                            self_item.toggleOffOn = false;
                        }
                        InventorySystem.current.Add(this.self_item);
                        amount_of_items--;
                    }
                }
                else
                {
                    if (self_item.toggleable == true)
                    {
                        self_item.toggleOffOn = false;
                    }
                    InventorySystem.current.Add(this.self_item);
                    amount_of_items--;
                }
                if (amount_of_items <= 0)
                {
                    Destroy(this.gameObject);
                }

            }


            

            if (Input.GetMouseButtonDown(1))
            {
                wallet.GetComponent<money_collector>().money_value += (sell_value * amount_of_items);
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (this.gameObject.GetComponent<item_pickup>().enabled == true)
        {
            if (other.gameObject.name == "player")
            {
                

                if (TryGetComponent<item_price_holder>(out item_price_holder price_holder))
                {
                    sell_value = price_holder.item_cost;
                }


                //Debug.Log("mouse is over, item pickup");

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    for (int i = 0; i <= amount_of_items; i++)
                    {
                        if (self_item.toggleable == true)
                        {
                            self_item.toggleOffOn = false;
                        }
                        InventorySystem.current.Add(this.self_item);
                        amount_of_items--;
                    }
                }
                else
                {
                    if (self_item.toggleable == true)
                    {
                        self_item.toggleOffOn = false;
                    }
                    InventorySystem.current.Add(this.self_item);
                    amount_of_items--;
                }
                if (amount_of_items <= 0)
                {
                    Destroy(this.gameObject);
                }
                
            }
            yield return null;
        }
    }
}
