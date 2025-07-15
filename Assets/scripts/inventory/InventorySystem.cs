using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem current;
    public Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory;//{ get; private set; }



    private void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        
    }

    public event Action onInventoryChangedEvent;

    public void InventoryChanged()
    {
        if (onInventoryChangedEvent != null)
        {
            onInventoryChangedEvent();
        }
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }    
        return null;
    }

    public void Add(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            //referenceData.position_in_inventory = inventory.IndexOf(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
        InventoryChanged();
    }

    public void Remove(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue (referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();

            if(value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
    }

    public void swap_position(int spot_1, int spot_2)//,GameObject obj_1, GameObject obj_2)
    {
        InventoryItem temp = inventory[spot_1];
        inventory[spot_1] = inventory[spot_2];
        inventory[spot_2] = temp;
        /*obj_1.GetComponent<Draggable_item>().spot_in_inventory = spot_2;
        obj_2.GetComponent<Draggable_item>().spot_in_inventory = spot_1;*/
        Debug.Log("swapped index a with index b");
    }

    public void update_position(int spot, InventoryItem real_outcome)
    {
        if (inventory[spot] != real_outcome)
        {
            int real_outcome_location = inventory.IndexOf(real_outcome);
            swap_position(real_outcome_location, spot);
        }
    }
}

[Serializable]
public class InventoryItem
{
    public InventoryItemData data;// {  get; private set; }
    public int stackSize;// { get; private set; }

    public InventoryItem(InventoryItemData source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}