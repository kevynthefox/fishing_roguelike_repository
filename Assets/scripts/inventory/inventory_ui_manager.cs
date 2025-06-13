using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory_ui_manager : MonoBehaviour
{

    public GameObject UIInventoryItemSlot;
    public GameObject m_slotPrefab;
    public void Start()
    {
        //GameEvents.current.InventoryChanged += onInventoryChangedEvent;

        InventorySystem.current.onInventoryChangedEvent += OnUpdateInventory;
    }

    private void OnUpdateInventory()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach (InventoryItem item in InventorySystem.current.inventory)
        {
            AddInventorySlot(item);
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(m_slotPrefab);
        obj.transform.SetParent(transform, false);

        UIInventoryItemSlot slot = obj.GetComponent<UIInventoryItemSlot>();
        slot.Set(item);
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