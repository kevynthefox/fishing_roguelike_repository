using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory_ui_manager : MonoBehaviour
{

    //public GameObject UIInventoryItemSlot;
    public GameObject m_slotPrefab;
    public GameObject[] section;

    //public int slots_made;
    public void Start()
    {
        //GameEvents.current.InventoryChanged += onInventoryChangedEvent;

        InventorySystem.current.onInventoryChangedEvent += OnUpdateInventory;
    }

    private void OnUpdateInventory()
    {
        for (int i = 1; i < section.Length; i++)
        {
            foreach (Transform t in section[i].transform)
            {
                Destroy(t.gameObject);
            }

            
        }
        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach (InventoryItem item in InventorySystem.current.inventory)
        {
            AddInventorySlot(item);
        }
        /*foreach (InventoryItem item in InventorySystem.current.buffs_to_consume)
        {
            AddInventorySlot(item);
        }*/
    }

    public void AddInventorySlot(InventoryItem item)
    {
        int place = InventorySystem.current.inventory.IndexOf(item);
        GameObject obj = Instantiate(m_slotPrefab);
        obj.transform.SetParent(section[item.data.item_type].transform, false);
        obj.GetComponent<Transform>().Find("item").GetComponent<Draggable_item>().spot_in_inventory = InventorySystem.current.inventory.IndexOf(item);
        obj.GetComponent<InventorySlot>().inventory_slot_position = place;//InventorySystem.current.inventory.IndexOf(item);
        obj.GetComponent<Transform>().Find("item").GetComponent<Draggable_item>().self_inventory_item = item;
        UIInventoryItemSlot slot = obj.GetComponent<UIInventoryItemSlot>();
        slot.Set(item, place);
    }


}

[Serializable]
public struct ItemRequirement
{
    public InventoryItemData itemData;
    public int amount;

    public bool HasRequirement()
    {
        InventoryItem item = InventorySystem.current.Get(itemData);

        if (item == null || item.stackSize < amount) { return false; }

        return true;
    }
}