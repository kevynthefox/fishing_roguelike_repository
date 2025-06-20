using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_behavior : MonoBehaviour
{
    //item behavior

    public bool in_inventory;
    public GameObject inventory;

    //trigger IE, jumping or killing an enemy.
    public int trigger_type; //do things like "if space is pressed and trigger type is 1, do this thing
    //type of action
    public int action_type;
    //action done
    //action gameobject if the action is physical
    public GameObject action_object;
    //action effect if action is not physical
    public string action_effect;

    //duration of action
    public float duration;
    //delay between actions
    public float delay;
    //strength added with each one(unlike how in risk of rain(2) where 1st item gives +10 and second gives +5, just like, do +10 for both, because why not? it's literally a copy of the item, why wouldn't it work like this?)
    public int strength;
    //amount of times repeated
    public int stack_size;

    //target for action
    public GameObject target;
    public Vector3 target_transform;
    public Quaternion target_rot;
    //type of target. this is used for things like: if it's an enemy, put a text box that shows the effect and the time left on the effect, if it's the player, add a thing to the player's ui.
    public bool enemy_or_player; //false is enemy, true is player.

    public GameObject player;

    public bool triggered;



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
        foreach (InventoryItem item in InventorySystem.current.inventory)
        {
            //int this_one += 1;
            trigger_type = item.data.trigger_type;
            action_type = item.data.action_type;
            action_object = item.data.action_object;
            action_effect = item.data.action_effect;
            duration = item.data.duration;
            delay = item.data.delay;
            strength = item.data.strength;
            target = GameObject.Find(item.data.target);
            enemy_or_player = item.data.enemy_or_player;
            stack_size = item.stackSize;

            triggers();
        }
        
    }

    public void triggers()
    {
        //detect jumping
        if (player.GetComponent<movement>().isOnGround == true && Input.GetKeyDown(KeyCode.Space) && trigger_type == 1)
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
        for (int i = 0; i < stack_size; i++)
        {
            if (action_type == 1)
            {
                if (target != null)
                {
                    target_transform = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z); //keeping this as its own variable because it may be handy for things later. like, spawning something after the action maybe.
                    target_rot = target.transform.rotation;
                }
                //target_rot = target.transform.rotation;
                Instantiate(action_object, target_transform, target_rot);
                //Instantiate(action_object,new Vector3(0,0,0), Quaternion.identity);
            }
        }
        
    }
}
