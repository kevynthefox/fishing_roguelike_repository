using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    public int action_effect;

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

    public bool over_trigger_prevention;

    public InventoryItem current_item;

    public float target_vicinity;

    public bool inheret_target_rotation;

    //trigger type list

    //trigger type 1 is jumping
    //type 2 is activates upon wining fishing.
    //type 3 activates when you push the activation button(same button for all of this type)

    //action type list

    //action type 1 is spawning an object at the target location
    //type 2 is spawning something based on the position the fishing game won in(like, which colored bar)
    //type 3 mulitplies the values on the fishing bar. consumes the items afterwords.
    //type 4 adds to the values on the fishing bar, consumes after.

    public void Awake()
    {
        player = GameObject.Find("player");
        object_holder = GameObject.Find("object_holder_object");
        fishing_controller = object_holder.GetComponent<object_holder>().bobber;
    }

    public void Update()
    {
        //if (InventorySystem.current.inventory.Contains(this))
        foreach (InventoryItem item in InventorySystem.current.inventory.ToList())
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
            target_transform = target.transform;
            enemy_or_player = item.data.enemy_or_player;
            stack_size = item.stackSize;

            current_item = item;

            target_vicinity = item.data.target_vicinity;
            inheret_target_rotation = item.data.inheret_target_rotation;

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
        if (fishing_controller.GetComponent<fishing_script>().win_state == 1 && over_trigger_prevention == false)
        {
            triggered = true;
            over_trigger_prevention = true;
            //Debug.Log("won");
        }
        
        if (over_trigger_prevention == true && fishing_controller.GetComponent<fishing_script>().win_state != 1)
        {
            over_trigger_prevention = false; // resets over trigger prevention for the win state state.
            //Debug.Log("reset over_trigger_prevention"); 
        }

        if (Input.GetKeyDown(KeyCode.Z) && trigger_type == 3)
        {
            triggered = true;
            //Debug.Log("ability activated");
        }

        if (triggered == true)
        {
            action_taker();
            triggered = false;
            //Debug.Log("stopped");
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

            if (action_type == 3)
            {

                if (action_effect == 1)
                {
                    //this used to multiply the fishquantity stat directly, but 1: that didn't work, 2: the minimum stat ones already did that good enough.
                    if (fishing_controller.GetComponent<fishing_script>().fish_quantity_max_buff_mult * strength * strength != 1)
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_max_buff_mult *= strength * strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_max *= strength * strength;
                    }
                    else
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_mult = strength * strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min = strength * strength;
                    }

                    if (fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_mult * strength * strength != 1)
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_mult *= strength * strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min *= strength * strength;
                    }
                    else
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_mult = strength * strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min = strength * strength;
                    }
                }
                if (action_effect == 2)
                {
                    if (fishing_controller.GetComponent<fishing_script>().fish_quantity_max_buff_mult * strength != 1)
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_max_buff_mult *= strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_max *= strength;
                    }
                    else
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_mult = strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min = strength;
                    }    
                }
                if (action_effect == 3)
                {
                    if (fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_mult * strength != 1)
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_mult *= strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min *= strength;
                    }
                    else
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_mult = strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quantity_min = strength;
                    }
                }
                if (action_effect == 4)
                {
                    foreach (GameObject fish in GameObject.FindGameObjectsWithTag("fish"))
                    {
                        if (fish.GetComponent<fish_variable_holder>().potentcy * strength != 1)
                        {
                            fish.GetComponent<fish_variable_holder>().potentcy *= strength;
                        }
                        else
                        {
                            fish.GetComponent<fish_variable_holder>().potentcy = strength;
                        }
                        //Debug.Log("multiplied potency");
                        
                        //Debug.Log("found fish");
                    }
                }
                if (action_effect == 5)
                {
                    if (fishing_controller.GetComponent<fishing_script>().fish_quality_min_buff_mult * strength != 1)
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quality_min_buff_mult *= strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quality_min *= strength;
                    }
                    else
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quality_min_buff_mult = strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quality_min = strength;
                    }
                }
                if (action_effect == 6)
                {
                    if (fishing_controller.GetComponent<fishing_script>().fish_quality_max_buff_mult * strength != 1)
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quality_max_buff_mult *= strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quality_max *= strength;
                    }
                    else
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quality_max_buff_mult = strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quality_max = strength;
                    }
                }
                if (action_effect == 7)
                {
                    //this used to multiply the fishquality stat directly, but 1: that didn't work, 2: the minimum stat ones already did that good enough.
                    if (fishing_controller.GetComponent<fishing_script>().fish_quality_max_buff_mult * strength * strength != 1)
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quality_max_buff_mult *= strength * strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quality_max *= strength * strength;
                    }
                    else
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quality_max_buff_mult = strength * strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quality_max = strength * strength;
                    }
                    if (fishing_controller.GetComponent<fishing_script>().fish_quality_min_buff_mult * strength * strength != 1)
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quality_min_buff_mult *= strength * strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quality_min *= strength * strength;
                    }
                    else
                    {
                        fishing_controller.GetComponent<fishing_script>().fish_quality_min_buff_mult = strength * strength;
                        fishing_controller.GetComponent<fishing_script>().fish_quality_min = strength * strength;
                    }
                }
                InventorySystem.current.Remove(current_item.data);
                //Debug.Log("buffed");
                //Debug.Log(current_item.data.name);
                
            }

            if (action_type == 4)
            {

                if (action_effect == 1)
                {
                    
                    fishing_controller.GetComponent<fishing_script>().fish_quantity_buff_add += strength;
                    fishing_controller.GetComponent<fishing_script>().fish_quantity += strength;
                    
                    
                }
                if (action_effect == 2)
                {
                    
                    fishing_controller.GetComponent<fishing_script>().fish_quantity_max_buff_add += strength;
                    fishing_controller.GetComponent<fishing_script>().fish_quantity_max += strength;
                    
                }
                if (action_effect == 3)
                {
                    
                    fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_add += strength;
                    fishing_controller.GetComponent<fishing_script>().fish_quantity_min += strength;
                    
                }
                if (action_effect == 4)
                {
                    foreach (GameObject fish in GameObject.FindGameObjectsWithTag("fish"))
                    {
                        
                        fish.GetComponent<fish_variable_holder>().potentcy += strength;

                    }
                }
                if (action_effect == 5)
                {
              
                    fishing_controller.GetComponent<fishing_script>().fish_quality_min_buff_add += strength;
                    fishing_controller.GetComponent<fishing_script>().fish_quality_min += strength;

                }
                if (action_effect == 6)
                {

                    fishing_controller.GetComponent<fishing_script>().fish_quality_max_buff_add += strength;
                    fishing_controller.GetComponent<fishing_script>().fish_quality_max += strength;

                }
                if (action_effect == 7)
                {
                    
                    fishing_controller.GetComponent<fishing_script>().fish_quality_buff_add += strength;
                    fishing_controller.GetComponent<fishing_script>().fish_quality += strength;

                }
                InventorySystem.current.Remove(current_item.data);
                //Debug.Log("buffed");
                //Debug.Log(current_item.data.name);

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

            if(target_vicinity != 0)
            {
                pos.x += Random.Range(-target_vicinity, target_vicinity + 1);
                pos.y += Random.Range(-target_vicinity, target_vicinity + 1);
                pos.z += Random.Range(-target_vicinity, target_vicinity + 1);
            }

            if (inheret_target_rotation == false)
            {
                rot = action_object[object_to_spawn].transform.rotation;
            }
            var obj = Instantiate(action_object[object_to_spawn], pos, rot);
        }
    }
}
