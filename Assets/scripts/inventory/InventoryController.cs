using System;
using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController current;
    public GameObject inventory_display;
    public bool inventory_enabled;

    public List<GameObject> hands;
    public bool left_hand_filled;
    public bool right_hand_filled;

    public bool either_hand_filled;

    public bool potion_hand_filled;

    public GameObject[] section;

    public bool in_buffs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hands[2].SetActive(in_buffs);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventory_enabled = !inventory_enabled;
            InventorySystem.current.force_change = true;
        }
        inventory_display.SetActive(inventory_enabled);


        if (hands[0].transform.childCount > 1)
        {
            left_hand_filled = true;
        }
        else
        {
            left_hand_filled = false;
        }

        if (hands[1].transform.childCount > 1)
        {
            right_hand_filled = true;
        }
        else
        {
            right_hand_filled = false;
        }

        if (hands[2].transform.childCount > 1)
        {
            potion_hand_filled = true;
        }
        else
        {
            potion_hand_filled = false;
        }

        if (left_hand_filled == true || right_hand_filled == true || potion_hand_filled == true)
        {
            either_hand_filled = true;
        }
        else
        {
            either_hand_filled = false;
        }

        if (inventory_enabled == false)
        {
            if (left_hand_filled == true)
            {
                Destroy(hands[0].transform.GetChild(1).gameObject);
            }
            if (right_hand_filled == true)
            {
                Destroy(hands[1].transform.GetChild(1).gameObject);
            }
        }


    }

    #region sectioning
    public void section_1_toFront()
    {
        section[1].transform.SetAsLastSibling();
        in_buffs = false;
    }
    public void section_2_toFront()
    {
        section[2].transform.SetAsLastSibling();
        in_buffs = true;
    }
    public void section_3_toFront()
    {
        section[3].transform.SetAsLastSibling();
        in_buffs = false;
    }
    public void section_4_toFront()
    {
        section[4].transform.SetAsLastSibling();
        in_buffs = false;
    }
    #endregion
}
