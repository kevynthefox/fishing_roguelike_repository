using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Item_behavior : MonoBehaviour
{

    public static Item_behavior current;

    [Header("item information")]

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
    public GameObject target_obj;
    public string target_group;
    public Transform target_transform;
    public Quaternion target_rot;
    //type of target. this is used for things like: if it's an enemy, put a text box that shows the effect and the time left on the effect, if it's the player, add a thing to the player's ui.
    public bool enemy_or_player; //false is enemy, true is player.

    [Header("stored info for this script ")]

    public GameObject player;
    public GameObject player_effect_manager;
    public GameObject object_holder, fishing_controller, wavespawner;
    public List<GameObject> simple_rods;
    
    [Header("trigger info")]
    
    public bool triggered;
    public bool auto_triggered;
    public bool passive_triggered;
    public int toggle_triggered; //0 is no, 1 is off, 2 is on.
    public bool buff_activation;

    public bool over_trigger_prevention;
    public bool over_auto_trigger_prevention;
    public int times_used;
    public bool over_toggle_prevention;

    [Header("misc item info")]
    public InventoryItem current_item;

    public float target_vicinity;

    public bool inheret_target_rotation;

    public bool toggleOffOn;

    //gravity
    public Vector3 gravity_outcome = new Vector3(0, -9.81f, 0);
    public bool gravity_OffOn = true;
    public bool gravity_reverse;


    //trigger type list

    //trigger type 1 is jumping
    //type 2 is activates upon wining fishing.
    //type 3 activates when you push the activation button(same button for all of this type)
    //type 4 is passive(always active when the item is in your inventory. buff removed upon removing the item.
    //type 5 is togglable;

    //action type list

    //action type 1 is spawning an object at the target location
    //type 2 is spawning something based on the position the fishing game won in(like, which colored bar)
    //type 3 mulitplies the values on the fishing bar. consumes the items afterwords.
    //type 4 adds to the values on the fishing bar, consumes after.
    //type 5 is a health (additive) increase to all players. (passive?)
    //type 6 is a health (multiplicative) increase to all players.(passive?)
    //type 7 reverses gravity
    //type 8 removes gravity


    [Header("list section")]
    public List<GameObject> list_of_players;
    public List<GameObject> effect_manager_queue;
    public int place_in_queue;
    public int place_in_queue_in_queue;



    [Header("syncing new building section")]
    public List<int> add_list;
    public List<int> mult_list;
    public List<int> divide_list;

    [Header("other")]
    public bool been_clicked_on;
    public bool been_Right_clicked_on;
    public bool been_Left_clicked_on;


    public void Awake()
    {
        gravity_outcome = new Vector3(0, -9.81f, 0);
        gravity_OffOn = true;


        player = GameObject.Find("player");
        object_holder = GameObject.Find("object_holder_object");
        wavespawner = GameObject.Find("fish_wave_spawner");
        fishing_controller = object_holder.GetComponent<object_holder>().bobber;

        current = this;
    }

    private void Start()
    {
        StartCoroutine(list_time_buffer());
    }

    public void Update()
    {
        //player inventory
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
            if (item.data.target_obj.Length > 0)
            {
                target_obj = GameObject.Find(item.data.target_obj);
                target_transform = target_obj.transform;
            }
            target_group = item.data.target_group;
            enemy_or_player = item.data.enemy_or_player;
            stack_size = item.stackSize;

            current_item = item; //Debug.Log(current_item.data.name);

            target_vicinity = item.data.target_vicinity;
            inheret_target_rotation = item.data.inheret_target_rotation;

            times_used = item.times_used;

            if (item.data.been_middle_clicked_on == true && over_toggle_prevention == false)// && Input.GetMouseButtonDown(2))
            {
                item.data.toggleOffOn = !item.data.toggleOffOn;
                over_toggle_prevention = true;
                StartCoroutine(timed_untoggler(item));
            }
            toggleOffOn = item.data.toggleOffOn;

            been_clicked_on = item.data.been_clicked_on;
            been_Left_clicked_on = item.data.been_Left_clicked_on;
            been_Right_clicked_on = item.data.been_Right_clicked_on;

            buff_activation = false;

            triggers(item.data.in_potion);
        }
        //other effect managers
        //Debug.Log(effect_manager_queue.Count);
        for (int i = 0; i < effect_manager_queue.Count; i++)
        {
            //Debug.Log("in queue for the effect managers");
            place_in_queue = i;
            foreach (InventoryItemData item in effect_manager_queue[place_in_queue].GetComponent<effect_manager>().effect_data)
            {
                Debug.Log("getting data from the effect manager");
                place_in_queue_in_queue = effect_manager_queue[place_in_queue].GetComponent<effect_manager>().effect_data.IndexOf(item);
                //place_in_queue_in_queue++;
                //place_in_queue_in_queue = 0;

                //int this_one += 1;
                trigger_type = item.trigger_type;
                action_type = item.action_type;
                action_object = item.action_object;
                action_effect = item.action_effect;
                duration = item.duration;
                delay = item.delay;
                strength = item.strength;
                if (item.target_obj.Length > 0)
                {
                    target_obj = GameObject.Find(item.target_obj);
                    target_transform = target_obj.transform;
                }
                target_group = item.target_group;
                enemy_or_player = item.enemy_or_player;
                stack_size = 0;// item.stackSize;

                current_item.data = item; //Debug.Log(current_item.name);

                target_vicinity = item.target_vicinity;
                inheret_target_rotation = item.inheret_target_rotation;

                //times_used = item.times_used;

                if (item.been_middle_clicked_on == true && over_toggle_prevention == false)// && Input.GetMouseButtonDown(2))
                {
                    item.toggleOffOn = !item.toggleOffOn;
                    over_toggle_prevention = true;
                    //StartCoroutine(timed_untoggler(item));
                }
                toggleOffOn = item.toggleOffOn;

                been_clicked_on = item.been_clicked_on;
                been_Left_clicked_on = item.been_Left_clicked_on;
                been_Right_clicked_on = item.been_Right_clicked_on;

                buff_activation = true;

                triggers(item.in_potion);
            }
        }

        //Debug.Log("been right clicked on = " + been_Right_clicked_on);


        if (stack_size == 1)
        {
            current_item.last_item_in_stack = true;
        }
        else
        {
            current_item.last_item_in_stack = false;
        }

        if (gravity_OffOn == true)
        {
            if (gravity_reverse == true)
            {
                Physics.gravity = -gravity_outcome;
            }
            if (gravity_reverse == false)
            {
                Physics.gravity = gravity_outcome;
            }
        }
        else
        {
            Physics.gravity = gravity_outcome * 0;
        }



    }



    public void triggers(bool in_potion)
    {
        //Debug.Log("triggers-ing");
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


        foreach (GameObject simple_rod in simple_rods)
        {
            if (simple_rod.GetComponent<Auto_fisher>().fish == false)
            {
                //triggered = true;
                if (over_auto_trigger_prevention == false)
                {
                    auto_triggered = true;
                    over_auto_trigger_prevention = true;
                }
                //Debug.Log("won");
            }
            if (over_auto_trigger_prevention == true && simple_rod.GetComponent<Auto_fisher>().fish == true)
            {
                over_auto_trigger_prevention = false;
            }
        }


        if (over_trigger_prevention == true && fishing_controller.GetComponent<fishing_script>().win_state != 1)
        {
            over_trigger_prevention = false; // resets over trigger prevention for the win state state.

            //Debug.Log("reset over_trigger_prevention"); 
        }

        if (Input.GetKeyDown(KeyCode.Z) && trigger_type == 3 && in_potion == true)
        {
            for (int i = 0; i < stack_size; i++)
            {
                player_effect_manager.GetComponent<effect_manager>().spawnBuffs(current_item.data.action_effect);
                InventorySystem.current.Remove(current_item.data);
            }
            triggered = true;
            
            //Debug.Log("ability activated");
        }
        
        //Debug.Log("place in queue in queue: " + place_in_queue_in_queue);
        if (place_in_queue_in_queue >= 0 && trigger_type == 3 && buff_activation == true)
        {
            triggered = true;
        }

        if (trigger_type == 4)
        {
            passive_triggered = true;
        }

        if (trigger_type == 5)
        {
            //it is done like this because the effect needs to be reversed when the item is turned off.
            if (toggleOffOn == true)
            {
                toggle_triggered = 2;
            }
            if (toggleOffOn == false)
            {
                toggle_triggered = 1;
            }
        }

        if (triggered == true)
        {
            action_taker();
            triggered = false;

            //Debug.Log("stopped");
        }

        if (auto_triggered == true)
        {
            action_taker();
            auto_triggered = false;
        }

        if (passive_triggered == true)
        {
            //Debug.Log("passive action triggering");
            passive_action_doer();
        }

        if (toggle_triggered != 0)
        {
            action_taker();
        }

        if (triggered == false)
        {
            if (buff_activation == true)
            {
                buff_activation = false;
            }
        }
    }



    public void action_taker()
    {
        if (stack_size > 0)
        {
            for (int i = 0; i < stack_size; i++)
            {
                action_taker_executer();
                current_item.times_used = i + 1;
            }
        }
        else
        {
            action_taker_executer();
            Debug.Log("place in queue: " + place_in_queue + ", place in queue in queue: " + place_in_queue_in_queue);

            //if (item_done_being_used == true)
            //{
                effect_manager_queue[place_in_queue].GetComponent<effect_manager>().removeBuffs(place_in_queue_in_queue);
            //}
        }

    }

    void action_taker_executer()
    {

        if (action_type == 1 && triggered == true)
        {
            spawn_obj(0);
        }

        if (action_type == 2)
        {
            if (auto_triggered == false)
            {
                if (triggered == true)
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
            else
            {
                spawn_obj(Random.Range(0, 7));
            }
        }

        if (action_type == 3 && triggered == true)
        {
            //if (buff_activation == true)
            //{
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

                foreach (GameObject simple_rod in simple_rods)
                {
                    if (simple_rod.GetComponent<Auto_fisher>().fish_quantity_max_buff_mult * strength * strength != 1)
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quantity_max_buff_mult *= strength * strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quantity_max *= strength * strength;
                    }
                    else
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_mult = strength * strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quantity_min = strength * strength;
                    }

                    if (simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_mult * strength * strength != 1)
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_mult *= strength * strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quantity_min *= strength * strength;
                    }
                    else
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_mult = strength * strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quantity_min = strength * strength;
                    }
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

                foreach (GameObject simple_rod in simple_rods)
                {
                    if (simple_rod.GetComponent<Auto_fisher>().fish_quantity_max_buff_mult * strength != 1)
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quantity_max_buff_mult *= strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quantity_max *= strength;
                    }
                    else
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_mult = strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quantity_min = strength;
                    }
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

                foreach (GameObject simple_rod in simple_rods)
                {
                    if (simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_mult * strength != 1)
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_mult *= strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quantity_min *= strength;
                    }
                    else
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_mult = strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quantity_min = strength;
                    }
                }
            }
            if (action_effect == 4)
            {

                fishing_controller.GetComponent<fishing_script>().fish_potency_buff_mult += strength;

                foreach (GameObject simple_rod in simple_rods)
                {
                    simple_rod.GetComponent<Auto_fisher>().fish_potency_buff_mult += strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quantity_max += strength;
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

                foreach (GameObject simple_rod in simple_rods)
                {
                    if (simple_rod.GetComponent<Auto_fisher>().fish_quality_min_buff_mult * strength != 1)
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quality_min_buff_mult *= strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quality_min *= strength;
                    }
                    else
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quality_min_buff_mult = strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quality_min = strength;
                    }
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

                foreach (GameObject simple_rod in simple_rods)
                {
                    if (simple_rod.GetComponent<Auto_fisher>().fish_quality_max_buff_mult * strength != 1)
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quality_max_buff_mult *= strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quality_max *= strength;
                    }
                    else
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quality_max_buff_mult = strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quality_max = strength;
                    }
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

                foreach (GameObject simple_rod in simple_rods)
                {
                    //this used to multiply the fishquality stat directly, but 1: that didn't work, 2: the minimum stat ones already did that good enough.
                    if (simple_rod.GetComponent<Auto_fisher>().fish_quality_max_buff_mult * strength * strength != 1)
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quality_max_buff_mult *= strength * strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quality_max *= strength * strength;
                    }
                    else
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quality_max_buff_mult = strength * strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quality_max = strength * strength;
                    }
                    if (simple_rod.GetComponent<Auto_fisher>().fish_quality_min_buff_mult * strength * strength != 1)
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quality_min_buff_mult *= strength * strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quality_min *= strength * strength;
                    }
                    else
                    {
                        simple_rod.GetComponent<Auto_fisher>().fish_quality_min_buff_mult = strength * strength;
                        //simple_rod.GetComponent<Auto_fisher>().fish_quality_min = strength * strength;
                    }
                }
            }
            //}
            //InventorySystem.current.Remove(current_item.data);
            //if (buff_activation == true)
            //{
                //effect_manager_queue[place_in_queue].GetComponent<effect_manager>().removeBuffs(place_in_queue_in_queue);
            //}
            //Debug.Log("buffed");
            //Debug.Log(current_item.data.name);

        }

        if (action_type == 4 && triggered == true)
        {
            //if (buff_activation == true)
            //{
            if (action_effect == 1)
            {

                fishing_controller.GetComponent<fishing_script>().fish_quantity_max_buff_add += strength * strength;
                fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_add += strength * strength;
                fishing_controller.GetComponent<fishing_script>().fish_quantity_max += strength * strength;
                fishing_controller.GetComponent<fishing_script>().fish_quantity_min += strength * strength;

                foreach (GameObject simple_rod in simple_rods)
                {
                    simple_rod.GetComponent<Auto_fisher>().fish_quantity_max_buff_add += strength * strength;
                    simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_add += strength * strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quantity_max += strength * strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quantity_min += strength * strength;
                }
            }
            if (action_effect == 2)
            {

                fishing_controller.GetComponent<fishing_script>().fish_quantity_max_buff_add += strength;
                fishing_controller.GetComponent<fishing_script>().fish_quantity_max += strength;

                foreach (GameObject simple_rod in simple_rods)
                {
                    simple_rod.GetComponent<Auto_fisher>().fish_quantity_max_buff_add += strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quantity_max += strength;
                }
            }
            if (action_effect == 3)
            {

                fishing_controller.GetComponent<fishing_script>().fish_quantity_min_buff_add += strength;
                fishing_controller.GetComponent<fishing_script>().fish_quantity_min += strength;

                foreach (GameObject simple_rod in simple_rods)
                {
                    simple_rod.GetComponent<Auto_fisher>().fish_quantity_min_buff_add += strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quantity_min += strength;
                }
            }
            if (action_effect == 4)
            {

                fishing_controller.GetComponent<fishing_script>().fish_potency_buff_add += strength;

                foreach (GameObject simple_rod in simple_rods)
                {
                    simple_rod.GetComponent<Auto_fisher>().fish_potency_buff_add += strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quantity_max += strength;
                }
            }
            if (action_effect == 5)
            {

                fishing_controller.GetComponent<fishing_script>().fish_quality_min_buff_add += strength;
                fishing_controller.GetComponent<fishing_script>().fish_quality_min += strength;


                foreach (GameObject simple_rod in simple_rods)
                {
                    simple_rod.GetComponent<Auto_fisher>().fish_quality_min_buff_add += strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quality_min += strength;
                }
            }
            if (action_effect == 6)
            {

                fishing_controller.GetComponent<fishing_script>().fish_quality_max_buff_add += strength;
                fishing_controller.GetComponent<fishing_script>().fish_quality_max += strength;

                foreach (GameObject simple_rod in simple_rods)
                {
                    simple_rod.GetComponent<Auto_fisher>().fish_quality_max_buff_add += strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quality_max += strength;
                }
            }
            if (action_effect == 7)
            {

                fishing_controller.GetComponent<fishing_script>().fish_quality_max_buff_add += strength * strength;
                fishing_controller.GetComponent<fishing_script>().fish_quality_min_buff_add += strength * strength;
                fishing_controller.GetComponent<fishing_script>().fish_quality_max += strength * strength;
                fishing_controller.GetComponent<fishing_script>().fish_quality_min += strength * strength;

                foreach (GameObject simple_rod in simple_rods)
                {
                    simple_rod.GetComponent<Auto_fisher>().fish_quality_max_buff_add += strength * strength;
                    simple_rod.GetComponent<Auto_fisher>().fish_quality_min_buff_add += strength * strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quality_max += strength * strength;
                    //simple_rod.GetComponent<Auto_fisher>().fish_quality_min += strength * strength;
                }
            }
            //}
            //InventorySystem.current.Remove(current_item.data);
            //if (buff_activation == true)
            //{
                //effect_manager_queue[place_in_queue].GetComponent<effect_manager>().removeBuffs(place_in_queue_in_queue);
            //}
            //Debug.Log("buffed");
            //Debug.Log(current_item.data.name);

        }

        if (action_type == 7 && toggle_triggered == 2)
        {
            gravity_reverse = true;
        }
        if (action_type == 7 && toggle_triggered == 1)
        {
            gravity_reverse = false;
        }

        if (action_type == 8 && toggle_triggered == 2)
        {
            gravity_OffOn = false;
        }
        if (action_type == 8 && toggle_triggered == 1)
        {
            gravity_OffOn = true;
        }


        
    }

    public void passive_action_doer()
    {
        //Debug.Log("passive action doing");


        if (stack_size > times_used && !Input.GetMouseButton(1))
        {
            //Debug.Log("stack size was bigger(over), doing a thing");
            passive_action_taker(1);

        }

        if (been_Right_clicked_on == true && Input.GetKey(KeyCode.LeftControl))
        {
            for (int i = 0; i < times_used; i++)
            {
                passive_action_taker(-1);
                //Debug.Log("control, last item in stack, undoing");
            }

        }

        if (((stack_size < times_used) || (times_used == 1 && been_Right_clicked_on == true)) && !Input.GetKey(KeyCode.LeftControl))
        {
            if (times_used == 1 && been_Right_clicked_on)
            {
                //Debug.Log("last item in stack, undoing");
            }

            //Debug.Log("stack size was smaller, undoing a thing, over " + "stack size: " + stack_size + " times used: " + times_used);
            //if (times_used == 1) yield return new WaitForSeconds(.02f);
            passive_action_taker(-1);

        }


    }
    public void passive_action_taker(int sign)
    {
        //Debug.Log("passive action taking");
        if (action_type == 5)
        {
            foreach (GameObject player in list_of_players)
            {
                player.transform.GetComponentInChildren<Health_display>().health_max = player.transform.GetComponentInChildren<Health_display>().health_max + (sign * strength);// * stack_size);
                //Debug.Log(player.name);
            }
            add_list.Add(sign * strength);
        }

        if (action_type == 6)
        {
            foreach (GameObject player in list_of_players)
            {
                if (sign == 1)
                {
                    player.transform.GetComponentInChildren<Health_display>().health_max = player.transform.GetComponentInChildren<Health_display>().health_max * (strength);// * stack_size);
                }
                if (sign == -1)
                {
                    player.transform.GetComponentInChildren<Health_display>().health_max = player.transform.GetComponentInChildren<Health_display>().health_max / (strength);// * stack_size);
                }
            }
            if (sign == 1)
            {
                mult_list.Add(strength);
            }
            if (sign == -1)
            {
                divide_list.Add(strength);
            }
        }

        current_item.times_used += sign;
    }


    public void Untrigger(string buff_name)
    {
        effect_manager_queue[place_in_queue].GetComponent<effect_manager>().removeVisualBuffs(buff_name);
    }

    public void spawn_obj(int object_to_spawn)
    {
        if (target_obj == null)
        {
            var obj = Instantiate(action_object[object_to_spawn], Vector3.zero, action_object[object_to_spawn].transform.rotation);
        }
        else
        {
            target_transform.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);

            if (target_vicinity != 0)
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

    public void gather_groups(string tag, List<GameObject> list_to_target)
    {
        //Debug.Log("gathering list of objects with the tag: " + tag + " and sending it to list: " + list_to_target);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            if (!list_to_target.Contains(obj))
            {
                list_to_target.Add(obj);
            }
        }
    }

    public IEnumerator list_time_buffer()
    {
        while (TimeManager.current.essential_starter == true)
        {
            gather_groups("player", list_of_players);
            gather_groups("simple_fishing_rod", simple_rods);


            yield return new WaitForSeconds(1);
            //Debug.Log("still going");

        }
    }

    public void apply_changes_that_have_been_made(GameObject object_to_apply_to)
    {
        for (int i = 0; i < add_list.Count; i++)
        {
            //Debug.Log("times applied: " + i);
            object_to_apply_to.transform.GetComponentInChildren<Health_display>().health_max = object_to_apply_to.transform.GetComponentInChildren<Health_display>().health_max + (add_list[i]);

        }


        for (int i = 0; i < mult_list.Count; i++)
        {
            object_to_apply_to.transform.GetComponentInChildren<Health_display>().health_max = object_to_apply_to.transform.GetComponentInChildren<Health_display>().health_max * (mult_list[i]);
        }
        for (int i = 0; i < divide_list.Count; i++)
        {
            object_to_apply_to.transform.GetComponentInChildren<Health_display>().health_max = object_to_apply_to.transform.GetComponentInChildren<Health_display>().health_max / (divide_list[i]);
        }
    }


    public IEnumerator timed_untoggler(InventoryItem item)
    {
        InventorySystem.current.forceChange(); Debug.Log("change forced");
        item.data.been_middle_clicked_on = false;
        yield return new WaitForSeconds(0.5f);
        over_toggle_prevention = false;
    }
}
