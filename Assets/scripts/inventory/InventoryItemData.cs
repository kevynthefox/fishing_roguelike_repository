using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    //item information
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    //public string type;

    //item behavior

    public bool in_inventory;
    public GameObject inventory;

    //trigger IE, jumping or killing an enemy.
    public int trigger_type; //do things like "if space is pressed and trigger type is 1, do this thing
    //type of action
    public int action_type;
    //action done
    //action gameobject if the action is physical
    public GameObject[] action_object;
    //action effect if action is not physical
    public int action_effect;

    //duration of action
    public float duration;
    //delay between actions
    public float delay;
    //strength added with each one(unlike how in risk of rain(2) where 1st item gives +10 and second gives +5, just like, do +10 for both, because why not? it's literally a copy of the item, why wouldn't it work like this?)
    public int strength;

    //target for action
    public string target_obj;
    public string target_group;
    public Transform target_transform;
    //public Quaternion target_rot;
    //type of target. this is used for things like: if it's an enemy, put a text box that shows the effect and the time left on the effect, if it's the player, add a thing to the player's ui.
    public bool enemy_or_player; //false is enemy, true is player.

    public GameObject player;

    public bool triggered;

    //position in inventory
    public int position_in_inventory;

    public float target_vicinity;

    public bool inheret_target_rotation;

    //

    //toggling
    public bool toggleOffOn;
    public Sprite icon_off;

    //click detection
    public bool been_clicked_on;
    public bool been_Right_clicked_on;
    public bool been_Left_clicked_on;
    public bool been_middle_clicked_on;

    /*

    //trigger type list

    //trigger type 1 is jumping

    //action type list

    //action type 1 is spawning an object at the target location

    public void Awake()
    {
        player = GameObject.Find("player");
    }

    public void Update()
    {
        //if (InventorySystem.current.inventory.Contains(this))

        triggers();
    }

    public void triggers()
    {
        //detect jumping
        if (player.GetComponent<movement>().isOnGround == false && trigger_type == 1)
        {
            triggered = true;
        }

        if (triggered == true)
        {
            action_taker();
            triggered = false;
        }
    }

    public void action_taker()
    {
        if (action_type == 1)
        {
            if (target != null)
            {
                target_transform = target.transform; //keeping this as its own variable because it may be handy for things later. like, spawning something after the action maybe.
            }
            //target_rot = target.transform.rotation;
            Instantiate(action_object, target_transform, target_transform);
            //Instantiate(action_object,new Vector3(0,0,0), Quaternion.identity);
        }
    }*/
}
