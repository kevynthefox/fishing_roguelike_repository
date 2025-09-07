using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour//, IDropHandler
{
    /*public GameObject left_collider;
    public GameObject top_collider;
    public GameObject right_collider;
    public GameObject bottom_collider;
    */
    /*public GameObject left_slot;
    public GameObject top_slot;
    public GameObject right_slot;
    public GameObject bottom_slot;
    */

    //public Transform parent_of_dropped_object;

    public GameObject child;
    //public GameObject second_child;

    //public int triggered_area; // 1 left, 2 top, 3 right, 4 bottom 

    public bool triggered;

    public int inventory_slot_position;

    public bool in_inventory;


    #region dragging_items[depreceated]
    /*public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("dropped");
        GameObject dropped = eventData.pointerDrag;
        Draggable_item draggable_Item = dropped.GetComponent<Draggable_item>();
        
        parent_of_dropped_object = draggable_Item.current_parent;
        
        draggable_Item.parentAfterDrag = transform;
        
        draggable_Item.current_parent = transform;
        //second_child = dropped;


        //relocate(parent_of_dropped_object);
        //relocate(parent_of_dropped_object);
        //if (left_collider.GetComponent<basic_mouseover_detection>().triggered == true || top_collider.GetComponent<basic_mouseover_detection>().triggered == true || right_collider.GetComponent<basic_mouseover_detection>().triggered == true || bottom_collider.GetComponent<basic_mouseover_detection>().triggered == true)
        //{
            
        //}
    }*/

    /*void push(GameObject touched_obj)
    {
        Draggable_item draggable_Item = touched_obj.GetComponent<Draggable_item>();

        parent_of_dropped_object = draggable_Item.current_parent;

        draggable_Item.parentAfterDrag = transform;

        draggable_Item.current_parent = transform;
        //second_child = touched_obj;

        //second_child.GetComponent<Rigidbody2D>().addforce

        //relocate(parent_of_dropped_object);
    }*/

    /*public void OnTriggerEnter2D(Collider2D other)
    {
        if (in_inventory == true)
        {
            Debug.Log("inventory is open");
            //Debug.Log("touched something: " + other.gameObject.name);
            if (other.CompareTag("item"))
            {
                Debug.Log("touching an item");
                /*if (other.gameObject != first_child)
                {
                    Debug.Log("touching a different item");
                    //relocate(other.GetComponent<Draggable_item>().current_parent);

                    
                    push(other.gameObject);

                //}

            }
        }
    }*/

    /*public void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("touching anything" + other.gameObject.name);
        if (other.gameObject.tag == "item_slot")
        {
            if (left_collider.GetComponent<basic_mouseover_detection>().triggered == true)
            {
                left_slot = other.gameObject;
            }
            if (top_collider.GetComponent<basic_mouseover_detection>().triggered == true)
            {
                top_slot = other.gameObject;
            }
            if (right_collider.GetComponent<basic_mouseover_detection>().triggered == true)
            {
                right_slot = other.gameObject;
            }
            if (bottom_collider.GetComponent<basic_mouseover_detection>().triggered == true)
            {
                bottom_slot = other.gameObject;
            }
        }

        
        
    }*/

    /*void relocate(Transform new_location)
    {
        first_child.name = "item " + new_location.GetComponent<InventorySlot>().inventory_slot_position.ToString();
        first_child.GetComponent<Draggable_item>().parentAfterDrag = new_location;
        new_location.GetComponent<InventorySlot>().first_child = first_child;
        first_child.GetComponent<Draggable_item>().relocate();
        //InventorySystem.current.swap_position(first_child.GetComponent<Draggable_item>().spot_in_inventory, second_child.GetComponent<Draggable_item>().spot_in_inventory);//,first_child,second_child);
        //InventorySystem.current.update_position(inventory_slot_position,first_child.GetComponent<Draggable_item>().self_inventory_item);


        //first_child = second_child;
        first_child = this.transform.GetChild(0).gameObject;
        new_location.GetComponent<InventorySlot>().first_child = new_location.transform.GetChild(0).gameObject;
        second_child.GetComponent<Draggable_item>().spot_in_inventory = inventory_slot_position;
        second_child = null;

        Debug.Log("slot [" +inventory_slot_position + "]'s real child is: " + transform.GetChild(0).name);
        //if (first_child != transform.GetChild(0))
        //{
        //    Debug.Log("my child is someone elses");
        //    first_child = transform.GetChild(0).gameObject;
        //}
        //Debug.Log("set current child's parent to the other one");
    }*/
    #endregion
    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.None) { in_inventory = true; }
        if (Cursor.lockState == CursorLockMode.Locked) { in_inventory = false; }
        if (Input.GetKey(KeyCode.Tab))
        {
            //InventorySystem.current.put_in_right_place(inventory_slot_position, first_child.GetComponent<Draggable_item>().self_inventory_item);
            Debug.Log("put in right place");
        }
        
    }
}
