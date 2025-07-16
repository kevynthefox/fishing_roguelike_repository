using System.Collections;
using UnityEngine;


public class item_pickup : MonoBehaviour
{
    public bool left_clicked;

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
        if (this.GetComponent<item_pickup>().enabled == true)
        {
            Debug.Log("mouse is over");
            if (Input.GetMouseButtonDown(0))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    for (int i = 0; i <= amount_of_items; i++)
                    {
                        InventorySystem.current.Add(this.self_item);
                        amount_of_items--;
                    }
                }
                else
                {
                    InventorySystem.current.Add(this.self_item);
                    amount_of_items--;
                }
                if (amount_of_items <= 0)
                {
                    Destroy(this.gameObject);
                }

            }


            if (TryGetComponent<item_buying>(out item_buying buying))
            {
                //Debug.Log("i have item buying");
                buying.enabled = true;
                sell_value = buying.item_cost;
            }

            if (Input.GetMouseButtonDown(1))
            {
                wallet.GetComponent<money_collector>().money_value += (sell_value * amount_of_items);
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }
}
