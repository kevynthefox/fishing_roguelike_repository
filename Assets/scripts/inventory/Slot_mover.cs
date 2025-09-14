using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot_mover : MonoBehaviour
{
    public TMP_InputField input;
    public GameObject parent;

    public int current_location;
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
            InventorySystem.current.force_change = true;
            input.text = "input slot #\nhere";
        }        
    }
}
