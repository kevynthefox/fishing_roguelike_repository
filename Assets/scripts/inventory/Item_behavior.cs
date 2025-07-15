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
    public GameObject[] action_object;
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
    public Transform target_transform;
    public Quaternion target_rot;
    //type of target. this is used for things like: if it's an enemy, put a text box that shows the effect and the time left on the effect, if it's the player, add a thing to the player's ui.
    public bool enemy_or_player; //false is enemy, true is player.

    public GameObject player;
    public GameObject object_holder,fishing_controller;

    public bool triggered;



    //trigger type list

    //trigger type 1 is jumping
    //type 2 is activates upon wining fishing.

    //action type list

    //action type 1 is spawning an object at the target location
    //type 2 is spawning something based on the position the fishing game won in(like, which colored bar)

    public void Awake()
    {
        player = GameObject.Find("player");
        object_holder = GameObject.Find("object_holder_object");
        fishing_controller = object_holder.GetComponent<object_holder>().bobber;
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
        if (fishing_controller.GetComponent<fishing_script>().win_state == 1)
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
                spawn_obj(0); 
            }

            if (action_type == 2)
            {
                float bar_pos = fishing_controller.GetComponent<fishing_script>().bar_pos;
                if (0.00f < bar_pos && bar_pos < 0.14f) spawn_obj(0);
                if (0.14f < bar_pos && bar_pos < 0.28f) spawn_obj(1);
                if (0.28f < bar_pos && bar_pos < 0.42f) spawn_obj(2);
                if (0.42f < bar_pos && bar_pos < 0.56f) spawn_obj(3);
                if (0.56f < bar_pos && bar_pos < 0.70f) spawn_obj(4);
                if (0.70f < bar_pos && bar_pos < 0.84f) spawn_obj(5);
                if (0.84f < bar_pos && bar_pos < 1.00f) spawn_obj(6);
            }
        }
        
    }

    public void spawn_obj(int object_to_spawn)
    {
        if (target == null)
        {
            var obj = Instantiate(action_object[object_to_spawn], Vector3.zero, Quaternion.identity);
        }
        else
        {
            target_transform.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
            var obj = Instantiate(action_object[object_to_spawn], pos, rot);
        }
    }
}
