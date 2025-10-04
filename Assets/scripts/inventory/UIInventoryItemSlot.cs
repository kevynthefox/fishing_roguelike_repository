using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image m_icon;

    [SerializeField]
    private TextMeshProUGUI m_label;

    [SerializeField]
    private GameObject m_stackObj;

    [SerializeField]
    private Text m_stackLabel;

    [SerializeField]
    private Text m_slotLabel;

    
    public GameObject m_slotButton_1;
    public GameObject m_slotButton_2;
    public int item_internal;

    public void Set(InventoryItem item,int place)
    {
        m_slotLabel.text = place.ToString();// InventorySystem.current.inventory.IndexOf(item).ToString();
        name = "item slot: " + place.ToString();
        if (item.data.trigger_type == 5)
        {
            if (item.data.toggleOffOn == true)
            {
                m_icon.sprite = item.data.icon;
            }
            else
            {
                m_icon.sprite = item.data.icon_off;
            }
        }
        else
        {
            m_icon.sprite = item.data.icon;
        }
        m_label.text = item.data.displayName;
        if (item.stackSize <= 1)
        {
            m_stackObj.SetActive(false);
            return;
        }

        m_stackLabel.text = item.stackSize.ToString();

        if (item.data.item_type == 2)
        {
            if (item.data.in_potion == true)
            {
                m_slotButton_2.SetActive(true);
                m_slotButton_1.SetActive(false);
            }
            else
            {
                m_slotButton_2.SetActive(false);
                m_slotButton_1.SetActive(true);
            }
        }
        else
        {
            m_slotButton_2.SetActive(false);
            m_slotButton_1.SetActive(false);
        }

        item_internal = InventorySystem.current.inventory.IndexOf(item);
        
        
    }

    public void PutInPotion()
    {
        InventorySystem.current.inventory[item_internal].data.in_potion = !InventorySystem.current.inventory[item_internal].data.in_potion;
        //InventorySystem.current.inventory[item_internal].data.item_type = 5;
        InventorySystem.current.forceChange();
    }
}
