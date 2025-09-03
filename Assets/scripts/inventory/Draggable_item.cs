using System.Collections;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable_item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Transform parentAfterDrag;
    public Transform inventory_bar;

    public Transform current_parent;

    public InventoryItem self_inventory_item;

    public int spot_in_inventory;

    public GameObject player;

    public bool already_made_item;
    public bool toggleOffOn;

    public void Awake()
    {
        inventory_bar = GameObject.Find("inventory_bar").transform;
        player = GameObject.Find("player");
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(inventory_bar);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End drag");
        transform.SetParent(parentAfterDrag);
        spot_in_inventory = (parentAfterDrag.GetComponent<InventorySlot>().inventory_slot_position);
        image.raycastTarget = true;
    }

  

    public void relocate()
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
    #region item_dropping
    public void click_detection()
    {
        //Debug.Log("clicked on");

        if (Input.GetMouseButtonUp(1))
        {
            //Debug.Log("right clicked on ");
            if (Input.GetKey(KeyCode.LeftControl))
            {
                //Debug.Log("dropped one big item from inventory");
                for (int i = 0; i <= self_inventory_item.stackSize + 1; i++)
                {
                    drop_item(false);
                }
            }
            else
            {
                //Debug.Log("dropping 1 item from inventory");
                drop_item(true);
            }    
                
        }
    }

    public void drop_item(bool multiple_items_or_not) // false is one item worth several, true is several each worth one.
    {
        Vector3 random_pos_around_player = new Vector3(Random.Range(-5, 5) + player.transform.position.x, 0 + player.transform.position.y, Random.Range(-5, 5) + player.transform.position.z);
        var spawned_item = Instantiate(self_inventory_item.data.prefab, random_pos_around_player, self_inventory_item.data.prefab.transform.rotation);

        if (spawned_item.TryGetComponent<item_price_holder>(out item_price_holder price_Holder)) price_Holder.buy_or_pickup = true;
        

        /*GameObject bigger_item;
        GameObject smaller_item;*/
        if (multiple_items_or_not == false)
        {
            //Debug.Log("one big item");
            //int original_stack_size = self_inventory_item.stackSize;
            //Debug.Log("original_stack size: " + original_stack_size);
            if (InventorySystem.current.inventory[spot_in_inventory].already_made_item == false)
            {
                spawned_item.GetComponent<item_pickup>().amount_of_items = self_inventory_item.stackSize;
                InventorySystem.current.inventory[spot_in_inventory].already_made_item = true;

                for (int i = 0; i <= self_inventory_item.stackSize + 1; i++)
                {
                    InventorySystem.current.Remove(self_inventory_item.data);
                    Debug.Log("times removing");
                }

            }
            else
            {
                spawned_item.GetComponent<item_pickup>().amount_of_items = 0;
                Destroy(spawned_item);
                for (int i = 0; i <= self_inventory_item.stackSize; i++)
                {
                    InventorySystem.current.Remove(self_inventory_item.data);
                }
                
            }
            

            /*if (self_inventory_item.stackSize > 2)
            {
                if (self_inventory_item.stackSize <= 1)
                {
                    Destroy(spawned_item);
                }
            } */   

            /*int original_item_size = InventorySystem.current.inventory[spot_in_inventory].stackSize;
            Debug.Log("original size: " +  original_item_size);
            if (self_inventory_item.stackSize > 2)
            {
                if (spawned_item.GetComponent<item_pickup>().amount_of_items != original_item_size)
                {
                    Debug.Log("not original stack size, destroying");
                    Destroy(spawned_item);
                }
            }*/
            
            
            

            
        }
        else
        {
            //Debug.Log("many items");
            if (InventorySystem.current.inventory[spot_in_inventory].already_made_item == false)
            {
                spawned_item.GetComponent<item_pickup>().amount_of_items = 1;
            }
            else
            {
                spawned_item.GetComponent<item_pickup>().amount_of_items = 0;
                Destroy(spawned_item);
            }
            InventorySystem.current.Remove(self_inventory_item.data);
        }
        
        InventorySystem.current.InventoryChanged();

    }

    public void item_clicked_on()
    {
        
        if (Input.GetMouseButtonDown(0)) { self_inventory_item.data.been_Left_clicked_on = true; }
        if (Input.GetMouseButtonDown(1)) { self_inventory_item.data.been_Right_clicked_on = true; }
        if (Input.GetMouseButtonDown(2) && self_inventory_item.data.toggleable == true) { self_inventory_item.data.been_middle_clicked_on = true; Debug.Log("middle clicked"); StartCoroutine(middleclickFalse());
        }
        else { self_inventory_item.data.been_clicked_on = true; }

        //Invoke("item_clicked_off", 1f);
        //item_clicked_off();
    }

    public void item_clicked_off()
    {
        self_inventory_item.data.been_clicked_on = false;
        self_inventory_item.data.been_Left_clicked_on = false; 
        self_inventory_item.data.been_Right_clicked_on = false;
        self_inventory_item.data.been_middle_clicked_on = false; Debug.Log("UN middle clicked");
    }

    public IEnumerator middleclickFalse()
    {
        yield return new WaitForSeconds(1);
        self_inventory_item.data.been_middle_clicked_on = false; Debug.Log("UN middle clicked(via numerator)");
    }

    #endregion
}
