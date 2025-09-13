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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventory_enabled = !inventory_enabled;  
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
    }
}
