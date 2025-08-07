using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_abilities : MonoBehaviour
{
    public List<GameObject> spawnable_objects;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            var new_obj = Instantiate(spawnable_objects[0], transform);
            new_obj.transform.parent = null;
            //InventorySystem.current.gameObject.GetComponent<Item_behavior>().list_of_players.Add(new_obj);
            InventorySystem.current.gameObject.GetComponent<Item_behavior>().apply_changes_that_have_been_made(new_obj);
            Debug.Log("times updated");
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            var new_obj = Instantiate(spawnable_objects[1], transform);
            new_obj.transform.parent = null;
            //InventorySystem.current.gameObject.GetComponent<Item_behavior>().list_of_players.Add(new_obj);
            InventorySystem.current.gameObject.GetComponent<Item_behavior>().apply_changes_that_have_been_made(new_obj);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            var new_obj = Instantiate(spawnable_objects[2], transform);
            new_obj.transform.parent = null;
            //InventorySystem.current.gameObject.GetComponent<Item_behavior>().list_of_players.Add(new_obj);
            InventorySystem.current.gameObject.GetComponent<Item_behavior>().apply_changes_that_have_been_made(new_obj);
        }
    }
}
