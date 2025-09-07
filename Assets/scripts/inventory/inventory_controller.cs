using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class inventory_controller : MonoBehaviour
{
    public static inventory_controller current;
    public GameObject inventory_display;
    public bool inventory_enabled;

    public List<GameObject> hands;
    public int hand_increment;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }
}
