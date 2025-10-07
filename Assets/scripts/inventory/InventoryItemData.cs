using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    //item information
    [Header("item information")]
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    //public string type;

    //item behavior
    [Header("item behavior")]
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
    [Header("action target")]
    public string target_obj;
    public string target_group;
    public Transform target_transform;
    //public Quaternion target_rot;
    //type of target. this is used for things like: if it's an enemy, put a text box that shows the effect and the time left on the effect, if it's the player, add a thing to the player's ui.
    [Header("target type")]
    public bool enemy_or_player; //false is enemy, true is player.

    public GameObject player;

    public bool triggered;

    //position in inventory
    [Header("position in inventory")]
    public int position_in_inventory;

    public float target_vicinity;

    public bool inheret_target_rotation;

    public int item_type; // 1 is regular items. 2 is buffs. 3 is equipment(like fishing rod). 4 is heirlooms.
    public bool in_potion;
    //

    //toggling
    [Header("toggling")]
    public bool toggleable;
    public bool toggleOffOn;
    public Sprite icon_off;

    //click detection
    [Header("click detection")]
    public bool been_clicked_on;
    public bool been_Right_clicked_on;
    public bool been_Left_clicked_on;
    public bool been_middle_clicked_on;

    

    
}
