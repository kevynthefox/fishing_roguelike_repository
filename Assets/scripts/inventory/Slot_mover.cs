using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot_mover : MonoBehaviour
{
    public TMP_InputField input;
    public GameObject parent;
    public GameObject parent_er;

    public int current_location;
    public GameObject equipped_item_holder;
    public bool already_activated;

    public void send_item_to_slot()
    {
        Debug.Log("sending item to slot");

        Debug.Log("int parse of text: " + int.Parse(input.text));
        Debug.Log("inventory count: " + InventorySystem.current.inventory.Count);
        if (int.Parse(input.text) > InventorySystem.current.inventory.Count -1)
        {
            input.text = "slot does not\nexist. try again";
        }
        else
        {
            InventorySystem.current.swap_position(parent.transform.GetChild(1).GetComponent<Draggable_item>().spot_in_inventory, int.Parse(input.text));

            if (InventoryController.current.left_hand_filled == true)
            {
                Destroy(InventoryController.current.hands[0].transform.GetChild(1).gameObject);
            }
            if (InventoryController.current.right_hand_filled == true)
            {
                Destroy(InventoryController.current.hands[1].transform.GetChild(1).gameObject);
            }
            

            InventorySystem.current.forceChange();
            input.text = "input slot #\nhere";
        }        
    }

    //0 is neither, 1 is left, 2 is right.
    public void equip_item(int hand_to_go_in)
    {
        
        if (already_activated == true)
        {
            Debug.Log("de-activating");
            equipment_system.current.despwan_equipment(hand_to_go_in);
            already_activated = !already_activated;
            equipped_item_holder.transform.GetChild(0).transform.parent = parent_er.transform;
            parent_er.transform.GetChild(1).transform.localPosition = Vector3.zero;
            InventorySystem.current.Add(parent_er.transform.GetChild(1).GetComponentInChildren<Draggable_item>().self_inventory_item.data);


        }
        else
        {
            Debug.Log("activating");
            if (equipped_item_holder.transform.childCount == 0)
            {
                if (parent_er.transform.childCount == 2)
                {
                    Debug.Log(parent_er.transform.GetChild(1).name);
                    GameObject child = parent_er.transform.GetChild(1).gameObject;

                    if (child.TryGetComponent<Draggable_item>(out Draggable_item dragg))
                    {
                        InventoryItem item = dragg.self_inventory_item;
                        equipment_system.current.spawn_equipment(hand_to_go_in, dragg.self_inventory_item.data, item.data.position, item.data.rotation, item.data.hand_change, item.data.left_hand_position, item.data.left_hand_rotation, item.data.right_hand_position, item.data.right_hand_rotation);
                        InventorySystem.current.Remove(item.data);
                    }

                    child.transform.parent = equipped_item_holder.transform;
                    child.transform.localPosition = Vector3.zero;

                }
            }
            already_activated = !already_activated;
        }


        
    }
}
