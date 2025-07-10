using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class fishing_script : MonoBehaviour
{
    
    

    [Header("fishing_script variables")]
    public bool string_on;
    public bool bobber_on;
    public bool rod_on;
    public GameObject fishing_rod;

    public bool left_clicked_down;
    public bool right_clicked_down;
    public bool left_clicked_hold;
    public bool right_clicked_hold;
    public bool left_clicked_up;
    public bool right_clicked_up;

    public bool actively_fishing;

    public bool won_failed_already;

    public GameObject object_holder;

    public GameObject COD;

    public int consecutive_wins;

    [Header("string variables")]
    public float distance;
    public bool enabled_fishing = false;
    public Vector3 velocity;

    public GameObject bone_master;

    public Scrollbar fishing_bar;

    public bool bobber_returned;

    [Header("fishing_bar variables")]

    public float fish_quantity; //how many fish you caught (like, im imagining fish grabbing on to one another to help resist)
    public float fish_quality; //the quality of the fish you caught(the reasoning is that they're higher quality if they're less tired)

    public float bar_pos;

    public float fish_quantity_max; //maximum number of fish you can catch
    public float fish_quality_max; //maximum level of quality(minumum level of tiredness on the fish)

    public float fish_quantity_min;
    public float fish_quality_min;

    public float effort;
    public float resistance;


    public float stopping_factor;

    public float area_difficulty;

    
    public Text quality;
    public Text quantity;

    public Text res;
    public Text res_2;
    public Text eff;

    public Scrollbar distance_bar;

    //public bool failure;
    //public bool success;
    public int win_state;

    public int direction;
    public int direction_max;
    public int direction_min;

    

    [Header("distance_bar variables")]
    
    public Text dist_text;

    public float initial_distance;
    //public float current_distance;
    public float percent_distance;

    public bool distance_set;

    [Header("bobber_impact variables")]

    //[SerializeField] Animator animator;
    [SerializeField] Animator rod_animator;

    public GameObject fishing_system;
    
    public GameObject[] fish;
    public GameObject bobber;

    public int randomIndex;

    [Header("fishing_rod_movement variables")]
    
    
    
    public float fishing_time;
    public float fishing_time_cool;
    public bool reel_able;
    
    public int fight_animation;
    

    public float loop_time_start;

    public bool blocking;
    public bool attacking;

    //public bool returned = false;

    
    public float fish_quantity_original;
    
    public int fish_counted;
    public int fish_removed;

    public bool fish_all_spawned;

    public bool resetting;



    public bool water_already;


    public bool starter;

    public bool spawning_fish;
    public GameObject already_fishing;

    public GameObject fish_spawner;

    [Header("combat variables")]

    public int damage;

    public Vector3 deflection_direction;
    public Vector3 direction_modified;

    public Camera cam;

    public int fish_ever;

    public void Awake()
    {
        object_holder = GameObject.Find("object_holder_object");
        bobber = object_holder.GetComponent<object_holder>().bobber;

        cam = Camera.main;
    }

    void Start()
    {
        

        bone_master = GameObject.Find("Bone.002");

        if (bobber_on == true)
        {

            



            quality.text = "quality:" + fish_quality.ToString("0.0") + "     max:" + fish_quality_max + "     min:" + fish_quality_min;
            quantity.text = "quanity:" + fish_quantity.ToString("0.0") + "    max:" + fish_quantity_max + "     min:" + fish_quantity_min;

            
            eff.text = "force:" + effort;

            direction_max += 1;


            dist_text.text = "distance:" + distance;

            
            initial_distance = distance;

            //current_distance = distance;
            //percent_distance = distance / initial_distance;
            //distance_bar.value = (percent_distance);


            fishing_system.SetActive(false);
            starter = true;
        }
    }

    public void Update()
    {
        if (rod_on == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                left_clicked_down = true;
            }
            else
            {
                left_clicked_down = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                right_clicked_down = true;
            }
            else
            {
                right_clicked_down = false;
            }

            if (Input.GetMouseButton(0))
            {
                left_clicked_hold = true;
            }
            else
            {
                left_clicked_hold = false;
            }

            if (Input.GetMouseButton(1))
            {
                right_clicked_hold = true;
            }
            else
            {
                right_clicked_hold = false;
            }

            if (Input.GetMouseButtonUp(0))
            {
                left_clicked_up = true;
            }
            else
            {
                left_clicked_up = false;
            }

            if (Input.GetMouseButtonUp(1))
            {
                right_clicked_up = true;
            }
            else
            {
                right_clicked_up = false;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                enabled_fishing = !enabled_fishing;
                if (bobber_on == true)
                {
                    StartCoroutine(reset_animations());
                }
                //Debug.Log("enabled fishing2: " + enabled_fishing);
                //GetComponent<Rigidbody>().isKinematic = !enabled_fishing;
            }

            if (enabled_fishing == true)
            {
                if (left_clicked_hold) //(Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    distance += 1;
                }

                if (right_clicked_hold) //(Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    distance -= 1;
                }

            }
            else
            {
                distance = 0;
            }
            if (bobber_on == true)
            {
                if (distance > 0)
                {
                    actively_fishing = true;
                }
                else
                {
                    if (bobber_returned == true)
                    {
                        actively_fishing = false;
                    }

                }
            }


            //bobber_returned = bobber.GetComponent<bobber_impact>().returned;

            if (bobber_returned == true)
            {
                StartCoroutine(wait_then_reset());
            }
            /*if (TryGetComponent<return_to_start>(out return_to_start start))
            {
                start.enabled = !enabled_fishing;
            }*/

            if (this.GetComponent<fishing_script>().enabled_fishing != bobber.GetComponent<fishing_script>().enabled_fishing)
            {
                //Debug.Log("whipped into shape");
                this.GetComponent<fishing_script>().enabled_fishing = bobber.GetComponent<fishing_script>().enabled_fishing;
            }

            

            if (string_on == true)
            {
                GetComponent<SpringJoint>().maxDistance = distance / 10;
                velocity = GetComponent<Rigidbody>().linearVelocity;
                GetComponent<SpringJoint>().spring = 1000 - (distance * 1);
                GetComponent<SpringJoint>().damper = 1 + (distance * 1);
                GetComponent<Rigidbody>().useGravity = enabled_fishing;

            }
            GetComponent<return_to_start>().enabled = !enabled_fishing;
            GetComponent<BoxCollider>().enabled = enabled_fishing;

            if (enabled_fishing == true)
            {
                //Debug.Log("enabled fishing is true");

                GetComponent<bobber_launch>().factor = distance;

                if (left_clicked_down == true)
                {
                    //Debug.Log("enabled fishing is true");
                    //rod_animator.SetBool("is_in_use", true);
                    reel_able = true;
                    GetComponent<bobber_launch>().factor = distance;
                    GetComponent<bobber_launch>().enabled = true;
                    bobber_returned = false;
                    //animator.SetBool("is_hoooked", false);
                    //rod_animator.SetBool("is_waiting", false);
                }
                if (left_clicked_up == true)
                {
                    //animator.SetBool("is_in_use", false);
                    //reel_able = false;
                    GetComponent<bobber_launch>().factor = 0;
                    GetComponent<bobber_launch>().enabled = false;
                }


            }


            if (bobber_on == true)
            {
                //Debug.Log(bar_pos);



                //floors the numbers to be at minumum of 1
                if (fish_quantity_original == 0)
                {
                    fish_quantity = fish_quantity_max * bar_pos; //the more the bar goes up, the more fish are caught
                    fish_quality = fish_quality_max * (1 - bar_pos); // the more the bar goes down, the higher quality of the fish caught.

                    if (fish_quantity <= fish_quantity_min) fish_quantity = fish_quantity_min;
                    if (fish_quality <= fish_quality_min) fish_quality = fish_quality_min;
                }


                direction = Random.Range(direction_min, direction_max);
                //Debug.Log("direction:" + direction);

                StartCoroutine(reel_mechanic());

                fishing_bar.value = bar_pos;

                StopCoroutine(reel_mechanic());

                quality.text = "quality:" + fish_quality.ToString("0.0") + "     max:" + fish_quality_max + "     min:" + fish_quality_min;
                quantity.text = "quanity:" + fish_quantity.ToString("0.0") + "    max:" + fish_quantity_max + "     min:" + fish_quantity_min;

                res.text = resistance.ToString("0.0");
                res_2.text = "" + area_difficulty;

                if (resetting == false && enabled_fishing == true && actively_fishing == true)
                {
                    if (bar_pos <= 0 + (resistance * Time.deltaTime * direction) || bar_pos >= 1 - (effort * Time.deltaTime * direction))
                    {
                        if (won_failed_already == false)
                        {
                            win_state = -1;
                            //Debug.Log("failure");
                            consecutive_wins = 0;
                            won_failed_already = true;
                            StartCoroutine(waiting_after_win_state());

                            resetting = true;
                            fish_all_spawned = false;
                            enabled_fishing = false;
                            Debug.Log("disabled fishing, waiting to re-enable");
                            StartCoroutine(re_enable_fishing_after_win());
                            Debug.Log("re-enabled fishing");
                        }

                        //enabled_fishing = false;
                    }
                    else
                    {
                        if (distance <= 0)
                        {
                            if (won_failed_already == false)
                            {
                                win_state = 1;
                                //Debug.Log("success");
                                consecutive_wins += 1;
                                won_failed_already = true;
                                fish_quantity_original = fish_quantity;
                                StartCoroutine(waiting_after_win_state());

                                resetting = true;
                                spawning_fish = true;

                                //Debug.Log("progressing 2");

                                StartCoroutine(spawn_fish());

                                StartCoroutine(disable_reset_after_win());
                            }

                        }

                    }

                }

                if (won_failed_already == true || enabled_fishing == false)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                }

                if (fish_all_spawned == true)
                {

                    //yield return new WaitForSeconds(2f);


                    //Debug.Log("reset");

                    StopCoroutine(spawn_fish());



                    fish_counted = 0;
                    resetting = false;
                    win_state = 0;


                    win_state = 0;

                    spawning_fish = false;
                    //Debug.Log("set spawning fish to false");
                    fish_all_spawned = false;
                    //Debug.Log("set fish_all spawned to false");
                }


                if (left_clicked_down && distance_set == false)
                {
                    initial_distance = distance;
                }

                if (right_clicked_down && distance_set == false)
                {
                    distance_set = true;
                }



                //current_distance = distance;
                if (distance / initial_distance > 0)
                {
                    percent_distance = distance / initial_distance;

                    distance_bar.value = (percent_distance);
                    dist_text.text = "distance:" + distance;// + (dist_bar.value / 100) + "%";
                }
                else
                {
                    distance_set = false;
                }

                //Debug.Log("enabled fishing: " + enabled_fishing);

                //Debug.Log("blocking_state: " + blocking);

                if (enabled_fishing == true)
                {
                    //Debug.Log("enabled fishing is true");

                    bobber.GetComponent<bobber_launch>().factor = distance;

                    if (left_clicked_down == true)
                    {
                        //Debug.Log("enabled fishing is true");
                        rod_animator.SetBool("is_in_use", true);
                        reel_able = true;
                        GetComponent<bobber_launch>().factor = distance;
                        GetComponent<bobber_launch>().enabled = true;
                        bobber_returned = false;
                        //animator.SetBool("is_hoooked", false);
                        rod_animator.SetBool("is_waiting", false);
                    }
                    if (left_clicked_up == true)
                    {
                        //animator.SetBool("is_in_use", false);
                        //reel_able = false;
                        GetComponent<bobber_launch>().factor = 0;
                        GetComponent<bobber_launch>().enabled = false;
                    }


                }
                if (enabled_fishing == false)
                {
                    //Debug.Log("enabled fishing is false");
                    reel_able = false;

                    rod_animator.SetBool("is_in_use", false);
                    rod_animator.SetBool("is_waiting", false);



                    if (left_clicked_hold == true)
                    {

                        attacking = true;

                        fight_animation = Random.Range(0, 2);

                        if (fight_animation == 0)
                        {
                            //Debug.Log("enabled fishing is false");
                            rod_animator.SetBool("fighting_1", true);
                        }
                        if (fight_animation == 1)
                        {
                            //Debug.Log("enabled fishing is false");
                            rod_animator.SetBool("fighting_2", true);
                        }
                    }
                    if (right_clicked_hold == true)
                    {
                        blocking = true;

                        fight_animation = Random.Range(0, 2);

                        if (fight_animation == 0)
                        {
                            rod_animator.SetBool("blocking_1", true);
                        }
                        if (fight_animation == 1)
                        {
                            rod_animator.SetBool("blocking_2", true);
                        }

                    }
                    if (left_clicked_up)
                    {
                        attacking = false;
                        rod_animator.SetBool("fighting_1", false);
                        rod_animator.SetBool("fighting_2", false);
                    }
                    if (right_clicked_up)
                    {
                        blocking = false;
                        rod_animator.SetBool("blocking_1", false);
                        rod_animator.SetBool("blocking_2", false);
                    }
                }


            }
        }
        else
        {
            if (this.GetComponent<fishing_script>().attacking != bobber.GetComponent<fishing_script>().attacking)
            {
                this.GetComponent<fishing_script>().attacking = bobber.GetComponent<fishing_script>().attacking;
            }

            if (this.GetComponent<fishing_script>().blocking != bobber.GetComponent<fishing_script>().blocking)
            {
                this.GetComponent<fishing_script>().blocking = bobber.GetComponent<fishing_script>().blocking;
            }
        }

    }

    

    IEnumerator wait_then_reset()
    {
        yield return new WaitForSeconds(fishing_time_cool);
        distance = 0;
    }

    public IEnumerator reel_mechanic()
    {
        if (bobber_on == true && resetting == false && water_already == true)
        {
            
            if (right_clicked_hold == true) 
            {


                if (bar_pos <= 1 - (effort * Time.deltaTime))
                {
                    bar_pos += effort * Time.deltaTime * (Mathf.Abs(direction) / (2 * stopping_factor));
                }

                    

                yield return new WaitForSeconds(1f);
            }
            if (left_clicked_hold == true) 
            {


                if (bar_pos >= 0 + (effort * Time.deltaTime))
                {
                    bar_pos += effort * Time.deltaTime * -(Mathf.Abs(direction) / (2 * stopping_factor));
                }

                    
                yield return new WaitForSeconds(1f);
            }

            if (left_clicked_hold == true || right_clicked_hold == true)
            {
                if (bar_pos <= 1 - (effort * Time.deltaTime) && direction == 1)
                {
                    bar_pos -= effort * Time.deltaTime;
                }
            }
            else
            {


                if (bar_pos >= 0 + (resistance * Time.deltaTime))// && direction == 0)
                {
                    bar_pos += resistance * direction * Time.deltaTime;
                }

                    

                yield return new WaitForSeconds(1f);
            }

            yield return new WaitForSeconds(100f);

            
        }
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (bobber_on == true)
            {
                //Debug.Log("is success true:" + success);
                //Debug.Log(other.name);

                //while (starter == true)
                //{
                



                yield return new WaitForSeconds(0.1f);
                if (bobber_returned == false)
                {
                    
                    if (spawning_fish == false)
                    {
                        already_fishing.SetActive(false);

                        if (other.gameObject.tag == "water")
                        {
                            GetComponent<Rigidbody>().isKinematic = true;
                            initial_distance = distance;

                            water_already = true;

                            area_difficulty = other.GetComponent<fishing_area_value_holder>().area_difficulty;
                            fish = other.GetComponent<fishing_area_value_holder>().fish;

                            resistance = Random.Range(0.1f, area_difficulty);
                            //Debug.Log(water_already);

                            fish_quantity = 0;
                            fish_quantity_original = 0;
                            bar_pos = 0.5f;
                            fishing_system.SetActive(true);
                            fishing_bar.value = bar_pos;

                            //Debug.Log("water");
                            yield return new WaitForSeconds(.5f);
                            rod_animator.SetBool("is_waiting", true);
                            //animator.SetBool("is_hooked", false);

                            yield return new WaitForSeconds(fishing_time);
                            //possibly just remove the hooked animation and all that to make the game more fast and fun
                            //animator.SetBool("is_hooked", true);



                            //Debug.Log("progressing 1");






                        }
                    }
                    else
                    {
                        if (other.gameObject.tag == "water")
                        {
                            already_fishing.SetActive(true);
                        }
                    }
                    

                }
                else
                {
                    StartCoroutine(reset_animations());
                    fishing_system.SetActive(false);
                    yield return new WaitForSeconds(10f);
                    StopCoroutine(reset_animations());
                    //fish_all_spawned = false;
                }

                if (other.gameObject.tag == "fishing_rod")
                {
                    //Debug.Log(other.name);

                    //Debug.Log("rod");
                    yield return new WaitForSeconds(.5f);
                    reel_able = false;
                    StartCoroutine(reset_animations());
                    bobber_returned = true;
                    water_already = false;


                    //StartCoroutine(winlose());
                }
                else
                {
                    yield return new WaitForSeconds(100f);
                    //Debug.Log("air");
                    rod_animator.SetBool("is_waiting", false);
                    //animator.SetBool("is_hooked", false);
                }
            }
        }
        //}


        
        //Debug.Log("i exist and am touching something");

        if (other.gameObject.CompareTag("fish") || other.gameObject.CompareTag("projectile"))
        {
            //Debug.Log("touching the fishing rod. block state: " + other.gameObject.GetComponent<fishing_rod_movement>().blocking + " attack state: " + other.gameObject.GetComponent<fishing_rod_movement>().attacking);
            
            if (blocking == false && attacking == true)
            {
                //Debug.Log("touched the rod. not blocking");
                other.tag = "super_food_items";
                Wavespawner.current.Remove_alive(this.gameObject);
                other.GetComponent<heat_seeking_fishles>().home = null;
            }
            if (blocking == true && attacking == false)
            {
                Debug.Log("deflected");
                deflection_direction = cam.GetComponent<Transform>().forward;
                direction_modified = deflection_direction * Time.deltaTime * 5000;

                other.GetComponent<Rigidbody>().AddForce(direction_modified, ForceMode.Impulse);
            }
            
        }

        
    }

    public IEnumerator reset_animations()
    {
        rod_animator.SetBool("is_in_use", false);
        rod_animator.SetBool("is_in_reel", false);
        rod_animator.SetBool("is_waiting", false);

        rod_animator.SetBool("fighting_1", false);
        rod_animator.SetBool("fighting_2", false);
        blocking = false;
        attacking = false;
        rod_animator.SetBool("blocking_1", false);
        rod_animator.SetBool("blocking_2", false);
        //animator.SetBool("is_hooked", false);
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator spawn_fish()
    {
        if (bobber_on == true)
        {
            randomIndex = Random.Range(0, fish.Length);
            Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);


            Vector3 randomPosition = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));



            while (resetting == true && fish_all_spawned == false)// && win_state == 1)
            {
                //Debug.Log("spawning fish");

                if (fish_quantity <= 0)
                {
                    //Debug.Log("out of fish");
                    //COD.GetComponent<COD>().size += fish_counted;
                    fish_all_spawned = true;
                    fish_ever += fish_counted;
                }
                else
                {

                    randomIndex = Random.Range(0, fish.Length);

                    Vector3 SpawnPosition_2 = new Vector3(fish_quantity + fish_counted, fish_quantity, fish_quantity);

                    Vector3 SpawnPosition_3 = new Vector3(fish_spawner.transform.position.x, fish_spawner.transform.position.y, fish_spawner.transform.position.z);

                    //transform.position = SpawnPosition_2;


                    var fish_object = Instantiate(fish[randomIndex], SpawnPosition_3, Quaternion.identity);

                    fish_object.GetComponent<heat_seeking_fishles>().home = GameObject.Find("sell guy");

                    fish_counted += 1;
                    
                    //wave_spawner.GetComponent<Wavespawner>().dead_fish.Add(fish_object.GetComponent<fish_variable_holder>().fish_type);
                    Wavespawner.current.Add_dead(fish_object.GetComponent<fish_variable_holder>().fish_type.GetComponent<fish_variable_holder>().fish_type);
                    Wavespawner.current.fish_total = fish_counted;

                    // this part changes the scale of the fish. if there is more than 1 of fish(1.2) then it makes the (.2) its own fish


                    if (fish_quantity >= 1)
                    {

                        fish_object.GetComponent<Transform>().localScale = new Vector3(fish_quality, fish_quality, fish_quality);
                        fish_object.name = "big fish";//  + "     fish remaining:" + fish_quantity + " out of: " + fish_quantity_original + "  quality:" + fish_quality;

                        fish_object.GetComponent<fish_variable_holder>().fish_quantity = 1;
                    }
                    else
                    {
                        if (fish_quantity > 0)
                        {
                            fish_object.GetComponent<Transform>().localScale = new Vector3(fish_quantity, fish_quantity, fish_quantity);
                            fish_object.name = "small fish";//  + "     fish remaining:" + fish_quantity + " out of: " + fish_quantity_original + "  quality:" + fish_quality;
                            fish_object.GetComponent<fish_variable_holder>().fish_quantity = fish_quantity;
                        }


                    }

                    
                    fish_object.GetComponent<fish_variable_holder>().fish_quality = fish_quality;
                    fish_object.GetComponent<fish_variable_holder>().fish_counted = fish_counted;




                }

                fish_quantity -= Mathf.Min(fish_quantity, 1); //subtracts 1 until it can't and then subtracts what's left
                //Debug.Log("subtracted fish quantity, current amount: " + fish_quantity);
                yield return new WaitForSeconds(1 / fish_quantity_original);
            }

        }
    }

    public IEnumerator wait(float wait)
    {
        //Debug.Log("waiting for " + wait + " seconds");
        yield return new WaitForSeconds(wait);
    }

    public IEnumerator re_enable_fishing_after_win()
    {
        yield return new WaitForSeconds(3f);
        enabled_fishing = true;
        bar_pos = 0.5f;
        //Debug.Log("reset. E has been pressed to try again");
        resetting = false;
        win_state = 0;
    }

    public IEnumerator disable_reset_after_win()
    {
        yield return new WaitForSeconds(3f);
        bar_pos = 0.5f;
        resetting = false;
        win_state = 0;
    }

    public IEnumerator waiting_after_win_state()
    {
        yield return new WaitForSeconds(10f);
        won_failed_already = false;
    }

    
    
}
