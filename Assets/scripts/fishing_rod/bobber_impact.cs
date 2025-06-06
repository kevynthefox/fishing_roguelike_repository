using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class bobber_impact : MonoBehaviour
{
    [SerializeField] Animator animator;
    public GameObject fishing_rod_1;

    public GameObject fishing_system;
    public Scrollbar fishing_bar;
    public Scrollbar distance_bar;
    public GameObject bone_master;

    public GameObject[] fish;
    public GameObject self;

    public float fishing_time;
    public float fishing_time_cool;

    public bool returned = false;

    public float fish_quantity;
    public float fish_quantity_original;
    public float fish_quality;

    public int fish_counted;
    public int fish_removed;

    public bool fish_all_spawned;

    public bool resetting;

    

    public bool water_already;

    public bool success;
    public bool failure;

    public bool starter;

    public bool spawning_fish;
    public GameObject already_fishing;

    public GameObject fish_spawner;

    private void Start()
    {
        fishing_time = fishing_rod_1.GetComponent<fishing_rod_movement>().fishing_time;
        fishing_time_cool = fishing_rod_1.GetComponent<fishing_rod_movement>().fishing_time_cool;
        fishing_system.SetActive(false);
        starter = true;
        
    }

    IEnumerator OnTriggerEnter(Collider other)
    {

        //Debug.Log("is success true:" + success);
        //Debug.Log(other.name);

        //while (starter == true)
        //{
        if (fish_all_spawned == true)
        {

            //yield return new WaitForSeconds(2f);


            //Debug.Log("reset");
            yield return new WaitForSeconds(0.1f);
            StopCoroutine(spawn_fish());



            fish_counted = 0;
            resetting = false;
            success = false;
            yield return new WaitForSeconds(1f);
            fish_all_spawned = false;
            fishing_bar.GetComponent<fishing_bar>().success = false;
            fishing_bar.GetComponent<fishing_bar>().failure = false;

            spawning_fish = false;
        }

        

        yield return new WaitForSeconds(0.1f);
        if (returned == false)
        {
            if (water_already == false)
            {
                if (spawning_fish == false)
                { 
                    already_fishing.SetActive(false);

                    if (other.gameObject.tag == "water")
                    {
                        water_already = true;
                        //Debug.Log(water_already);

                        fish_quantity = 0;
                        fish_quantity_original = 0;
                        fishing_system.SetActive(true);
                        fishing_bar.value = 0.5f;

                        //Debug.Log("water");
                        yield return new WaitForSeconds(.5f);
                        animator.SetBool("is_waiting", true);
                        animator.SetBool("is_hooked", false);

                        yield return new WaitForSeconds(fishing_time);
                        //possibly just remove the hooked animation and all that to make the game more fast and fun
                        animator.SetBool("is_hooked", true);

                        success = fishing_bar.GetComponent<fishing_bar>().success;
                        failure = fishing_bar.GetComponent<fishing_bar>().failure;
                            
                        Debug.Log("progressing 1");

                        if (success == true && failure == false)
                        {
                            resetting = true;
                            spawning_fish = true;

                            //animator.SetBool("is_hooked", false); //spawn a fish here
                            fish_quality = fishing_bar.GetComponent<fishing_bar>().fish_quality;
                            fish_quantity = fishing_bar.GetComponent<fishing_bar>().fish_num;
                            fish_quantity_original = fishing_bar.GetComponent<fishing_bar>().fish_num;

                                


                            Debug.Log("progressing 2");

                            StartCoroutine(spawn_fish());


                            fishing_bar.GetComponent<fishing_bar>().bar_pos = 0.5f;


                            

                            
                        }
                        else
                        {
                            Debug.Log("progressing 3");

                            //if (failure == true)
                            //{

                                Debug.Log("progressing 4");

                                resetting = true;

                                animator.SetBool("is_hooked", false);


                                if (returned == true)
                                {
                                    
                                }
                            //}

                            if (failure == true && success == true)
                            {
                                Debug.Log("progressing 5");
                            }
                        }    
                                
                        
                            
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
                if (other.gameObject.tag == "fishing_rod")
                {
                    //Debug.Log(other.name);
                    var rods = new HashSet<string>();
                        
                    //Debug.Log("rod");
                    yield return new WaitForSeconds(.5f);
                    fishing_rod_1.GetComponent<fishing_rod_movement>().reel_able = false;
                    reset_animations();
                    returned = true;
                    water_already = false;
                    if (success == false)
                    {
                        fish_all_spawned = false;

                        yield return new WaitForSeconds(3f);

                        fishing_bar.GetComponent<fishing_bar>().success = false;
                        fishing_bar.GetComponent<fishing_bar>().failure = false;
                        bone_master.GetComponent<variable_length>().enabled_fishing = true;
                        fishing_bar.GetComponent<fishing_bar>().bar_pos = 0.5f;
                        //Debug.Log("reset. E has been pressed to try again");
                        resetting = false;
                        Debug.Log("progressing 4.5");
                    }
                }
                else
                {
                    yield return new WaitForSeconds(100f);
                    //Debug.Log("air");
                    animator.SetBool("is_waiting", false);
                    animator.SetBool("is_hooked", false);
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
    //}
    }


    public IEnumerator reset_animations()
    {
        animator.SetBool("is_in_use", false);
        animator.SetBool("is_in_reel", false);
        animator.SetBool("is_waiting", false);
        animator.SetBool("is_hooked", false);
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator spawn_fish()
    {
        int randomIndex = Random.Range(0, fish.Length);
        Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        
        Vector3 randomPosition = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));

        

        while (resetting == true && fish_all_spawned == false && success == true)
        {
            

            if (fish_quantity <= 0)
            {
                fish_all_spawned = true;
            }
            else
            {
                
                randomIndex = Random.Range(0, fish.Length);

                Vector3 SpawnPosition_2 = new Vector3(fish_quantity + fish_counted, fish_quantity, fish_quantity);

                Vector3 SpawnPosition_3 = new Vector3(fish_spawner.transform.position.x, fish_spawner.transform.position.y, fish_spawner.transform.position.z);

                //transform.position = SpawnPosition_2;
                
                var fish_object = Instantiate(fish[randomIndex], SpawnPosition_3, Quaternion.identity);
                
                fish_counted += 1;

                // this part changes the scale of the fish. if there is more than 1 of fish(1.2) then it makes the (.2) its own fish


                if (fish_quantity >= 1)
                {

                    fish_object.GetComponent<Transform>().localScale = new Vector3(fish_quality, fish_quality, fish_quality);
                    fish_object.name = "big fish";//  + "     fish remaining:" + fish_quantity + " out of: " + fish_quantity_original + "  quality:" + fish_quality;

                }
                else
                {
                    if (fish_quantity > 0)
                    {
                        fish_object.GetComponent<Transform>().localScale = new Vector3(fish_quantity, fish_quantity, fish_quantity);
                        fish_object.name = "small fish";//  + "     fish remaining:" + fish_quantity + " out of: " + fish_quantity_original + "  quality:" + fish_quality;
                    }


                }

                fish_object.GetComponent<fish_variable_holder>().fish_quantity = fish_quantity;
                fish_object.GetComponent<fish_variable_holder>().fish_quality = fish_quality;
                fish_object.GetComponent<fish_variable_holder>().fish_counted = fish_counted;

                

                FISHDESTROYAAA();

            }

            fish_quantity -= Mathf.Min(fish_quantity, 1);

            yield return new WaitForSeconds(1 / fish_quantity_original);
        }

    }

    public void FISHDESTROYAAA()
    {
        //this part deletes any fish with the same name as ones that already exist
        var names = new HashSet<string>();
        foreach (var f in GameObject.FindGameObjectsWithTag("fish"))
        {
            if (names.Contains(f.name))
            {
                //Destroy(f);

                //f.GetComponent<MeshRenderer>().material.color = Color.red;

                Color32 fish_color = new Color(f.GetComponent<Transform>().position.x, f.GetComponent<Transform>().position.y, f.GetComponent<Transform>().position.z, fish_counted);

                f.GetComponent<MeshRenderer>().material.color = fish_color;

                fish_removed += 1;
            }
            else names.Add(f.name);

        }
        //this solution was developed by Ricky the Emerald Wolf(discord username)

        
        
    }

}
