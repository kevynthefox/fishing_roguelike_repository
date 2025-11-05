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
    //public List<InventoryItem> buffs_to_consume;//{ get; private set; }

    public bool force_change;

    [Header("amounts of item types")]
    public int item_count;
    public int buff_count;
    public int buffs_in_potion_count;
    public int equipment_count;
    public int heirloom_count;
    public int total_count;

    private void Update()
    {
        if (force_change == true)
        {
            InventoryChanged();
            force_change = false;
        }
    }
    public void forceChange()
    {
        Debug.Log("change forced");
        InventoryChanged();
    }

    private void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        
    }

    public event Action onInventoryChangedEvent;
    public event Action onInventoryAddedEvent;
    public event Action onInventoryRemovedEvent;

    public void InventoryChanged()
    {
        if (onInventoryChangedEvent != null)
        {
            onInventoryChangedEvent();
            //Debug.Log("inventory_changed");
        }
    }

    public void count_item_types()
    {
        item_count = 0;
        buff_count = 0;
        buffs_in_potion_count = 0;
        equipment_count = 0;
        heirloom_count = 0;

        foreach (InventoryItem item in inventory)
        {
            for (int i = 0; i < item.stackSize; i++)
            {
                if (item.data.item_type == 1)
                {
                    item_count++;
                }
                if (item.data.item_type == 2)
                {
                    if (item.data.in_potion == true)
                    {
                        buffs_in_potion_count++;
                    }
                    else
                    {
                        buff_count++;
                    }
                }
                if (item.data.item_type == 3)
                {
                    equipment_count++;
                }
                if (item.data.item_type == 4)
                {
                    heirloom_count++;
                }
            }
        }
        total_count = item_count + buff_count + buffs_in_potion_count + equipment_count + heirloom_count;
    }
    public void InventoryAdded()
    {
        if (onInventoryAddedEvent != null)
        {
            onInventoryAddedEvent();
            //Debug.Log("inventory_changed");
        }
    }
    public void InventoryRemoved()
    {
        if (onInventoryRemovedEvent != null)
        {
            onInventoryRemovedEvent();
            //Debug.Log("inventory_changed");
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
            InventoryChanged();
            InventoryAdded();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            //referenceData.position_in_inventory = inventory.IndexOf(newItem);
            m_itemDictionary.Add(referenceData, newItem);
            InventoryChanged();
            InventoryAdded();
        }
        
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
            InventoryChanged();
            InventoryRemoved();
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
        Debug.Log("double checking position start");
        if (inventory[spot].data != real_outcome.data)
        {
            Debug.Log("position was wrong, fixing");
            Debug.Log("index of real outcome: " + inventory.IndexOf(real_outcome));
            int real_outcome_location = inventory.IndexOf(real_outcome);

            swap_position(real_outcome_location, spot);
        }
    }

    public void put_in_right_place(int spot, InventoryItem item_in_spot)
    {
        inventory[spot] = item_in_spot;
    }

    private void OnApplicationQuit()
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.data.in_potion == true)
            {
                item.data.in_potion = false;
                //Debug.Log("setting to not in potion");
            }
        }

    }
}

[Serializable]
public class InventoryItem
{
    public InventoryItemData data;// {  get; private set; }
    public int stackSize;// { get; private set; }
    public bool already_made_item;
    public bool toggleOffOn;
    public int times_used;
    public bool last_item_in_stack;

    public bool assigned_to_section;

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