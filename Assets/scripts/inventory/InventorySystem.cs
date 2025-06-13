using System;
using System.Collections;
using System.Collections.Generic;
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