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

    public bool fish_all_spawned;

    public bool resetting;

    /*public float quantity_holder_1;
    public float quantity_holder_2;

    public List<GameObject> fish_exist;
    public List<GameObject> ghost_fish_exist;

    public GameObject fish_that_exists;
    */
    private void Start()
    {
        fishing_time = fishing_rod_1.GetComponent<fishing_rod_movement>().fishing_time;
        fishing_time_cool = fishing_rod_1.GetComponent<fishing_rod_movement>().fishing_time_cool;
        fishing_system.SetActive(false);
    }

    IEnumerator OnTriggerEnter(Collider other)
    {

        if (returned == false)
        {
            if (other.gameObject.tag == "water")
            {
                fish_quantity = 0;
                fishing_system.SetActive(true);
                fishing_bar.value = 0.5f;
                //Debug.Log("water");
                yield return new WaitForSeconds(.5f);
                animator.SetBool("is_waiting", true);
                animator.SetBool("is_hooked", false);

                yield return new WaitForSeconds(fishing_time);

                animator.SetBool("is_hooked", true);
                if (fishing_bar.GetComponent<fishing_bar>().success == true)
                {
                    resetting = true;

                    animator.SetBool("is_hooked", false); //spawn a fish here
                    fish_quality = fishing_bar.GetComponent<fishing_bar>().fish_quality;
                    fish_quantity = fishing_bar.GetComponent<fishing_bar>().fish_num;
                    fish_quantity_original = fishing_bar.GetComponent<fishing_bar>().fish_num;

                    StartCoroutine(spawn_fish());
                    //StartCoroutine(timemachine());

                    fishing_bar.GetComponent<fishing_bar>().bar_pos = 0.5f;


                    fish_all_spawned = false;

                    if (fish_all_spawned == true)
                    {
                        yield return new WaitForSeconds(2f);
                        fishing_bar.GetComponent<fishing_bar>().success = false;
                        fishing_bar.GetComponent<fishing_bar>().failure = false;

                        //Debug.Log("reset");
                        yield return new WaitForSeconds(0.1f);
                        StopCoroutine(spawn_fish());
                        //StopCoroutine(timemachine());
                        fish_counted = 0;
                        fish_all_spawned = false;
                        resetting = false;

                    }
                }
                else
                {
                    resetting = true;
                    animator.SetBool("is_hooked", false);

                    yield return new WaitForSeconds(2f);
                    fishing_bar.GetComponent<fishing_bar>().success = false;
                    fishing_bar.GetComponent<fishing_bar>().failure = false;
                    bone_master.GetComponent<variable_length>().enabled_fishing = true;
                    fishing_bar.GetComponent<fishing_bar>().bar_pos = 0.5f;
                    //Debug.Log("reset. E has been pressed to try again");
                    resetting = false;
                }
            }
            else
            {
                if (other.gameObject.tag == "fishing_rod")
                {

                    //Debug.Log("rod");
                    yield return new WaitForSeconds(.5f);
                    fishing_rod_1.GetComponent<fishing_rod_movement>().reel_able = false;
                    reset_animations();
                    returned = true;
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
            //fishing_rod_1.GetComponent<fishing_rod_movement>().reel_able = false;
        }
        /*
        if (other.gameObject.tag == "fishing_rod")
        {
            Debug.Log("rod");
            yield return new WaitForSeconds(.5f);
            fishing_rod_1.GetComponent<fishing_rod_movement>().reel_able = false;
            animator.SetBool("is_in_use", false);

        }
        */
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
        while (resetting == true && fish_all_spawned == false)
        {

            if (fish_quantity <= 0)
            {
            }
            else
            {
                Vector3 SpawnPosition_2 = new Vector3(fish_quantity + fish_counted, fish_quantity, fish_quantity);
                transform.position = SpawnPosition;
                Instantiate(fish[randomIndex], SpawnPosition, Quaternion.identity);
                fish_counted += 1;

                /*var fish_quantities = new List<string>();


                Debug.Log(fish[randomIndex].name);

                fish_quantities.Add(fish[fish_counted].name);*/

                //checks if it equals a state that was a few seconds ago. replace with if it equals a state at all using a list.

                fish[randomIndex].GetComponent<fish_variable_holder>().duplicate = false;

                /*if (fish_quantity <= quantity_holder_1 || fish_quantity <= quantity_holder_2)
                {
                    Debug.Log("original");
                    fish[randomIndex].GetComponent<fish_variable_holder>().duplicate = false;
                    
                    //break;
                }
                else
                {
                    Debug.Log("duplicate");
                    fish[randomIndex].GetComponent<fish_variable_holder>().duplicate = true;

                }*/

                /*if (fish[randomIndex].GetComponent<fish_variable_holder>().duplicate == true)
                {
                    yield return new WaitForSeconds(2f);
                    Debug.Log("teleporting naughty ones");
                    fish[randomIndex].transform.position = new Vector3(0, 10, 0);
                }*/

                //try making a list of each quantity and then compare the quantity to the previous one in the list or even anywhere lse in the list

                //yield return new WaitForSeconds(2f);
                // this part changes the scale of the fish. it should however spawn more fish than 1 if the value is greater than that and for the ones where it's less than one, spawn a smaller fish.

                //Debug.Log("fish spawned:" + randomIndex);

                //Debug.Log("big fish spawned" + "fish remaining:" + fish_quantity + " out of:" + fish_quantity_original);

                //Debug.Log("fish quanitity:" + fish_quantity);

                //fish[randomIndex].name = "big fish" + fish_counted + "   quantity:" + fish_quantity.ToString() + "     fish remaining:" + fish_quantity + " out of: " + fish_quantity_original;

                if (fish_quantity >= 1)
                {

                    fish[randomIndex].GetComponent<Transform>().localScale = new Vector3(fish_quality, fish_quality, fish_quality);
                    fish[randomIndex].name = "   quantity:" + fish_quantity.ToString() + "     fish remaining:" + fish_quantity + " out of: " + fish_quantity_original;

                    fish[randomIndex].GetComponent<fish_variable_holder>().fish_quantity = fish_quantity;
                    fish[randomIndex].GetComponent<fish_variable_holder>().fish_quality = fish_quality;
                    fish[randomIndex].GetComponent<fish_variable_holder>().fish_counted = fish_counted;



                    //fish_quantities.Add(fish[randomIndex].name);
                }
                else
                {

                    fish[randomIndex].GetComponent<Transform>().localScale = new Vector3(fish_quantity, fish_quantity, fish_quantity);
                    fish[randomIndex].name = "   quantity:" + fish_quantity.ToString() + "     fish remaining:" + fish_quantity + " out of: " + fish_quantity_original;

                    fish[randomIndex].GetComponent<fish_variable_holder>().fish_quantity = fish_quantity;
                    fish[randomIndex].GetComponent<fish_variable_holder>().fish_quality = fish_quality;
                    fish[randomIndex].GetComponent<fish_variable_holder>().fish_counted = fish_counted;
                    //fish_quantity -= fish_quantity;
                    //fish_quantities.Add(fish[randomIndex].name);

                    //Debug.Log("small fish spawned");
                    /*
                    foreach (GameObject f in GameObject.FindGameObjectsWithTag("fish"))
                    {
                        //Debug.Log(f.name);
                        
                        fish_exist.Add(f);
                    }

                    //foreach(GameObject f in fish_exist)
                    //{
                        /*if (f.name == f.name)
                        {
                            Debug.Log("duplicate");
                            f.GetComponent<fish_variable_holder>().duplicate = true;
                        }

                    var groups = fish_exist.GroupBy(f => f.name);
                    foreach (var group in groups)
                    {
                        if (group.Count() > 1)
                        {
                            Debug.Log(group.Key + ": " + group.Count());
                                
                            int group_count_minus_one = group.Count() - 1;

                            for (int i = 0; i < group.Key.Count() ; i++)
                            {
                                //Debug.Log(group.Key + ": " + group.Count());
                                //fish_exist.Remove(GameObject.Find(group.Key));
                                //ghost_fish_exist.Add(GameObject.Find(group.Key));

                                //Destroy(GameObject.Find(group.Key));

                                Debug.Log(group.Key + ": " + group.Count());
                                GameObject.Find(group.Key).GetComponent<fish_variable_holder>().duplicate = true;
                                //GameObject.Find(group.Key).GetComponent<Color>().Equals(Color.red);

                                //Debug.Log("i:" + i);

                                

                                    i++;
                            }

                        }
                    }    
                        
                    //}*/
                    var names = new HashSet<string>();
                    foreach (var f in GameObject.FindGameObjectsWithTag("fish"))
                    {
                        if (names.Contains(f.name)) Destroy(f);
                        else names.Add(f.name);
                    }


                    fish_all_spawned = true;
                    Debug.Log("fish all spawned");

                }


            }
            if (fish_quantity >= 1)
            {
                fish_quantity -= 1;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                fish_quantity -= fish_quantity;
                yield return new WaitForSeconds(1f);
            }


            /*if (fish_quantities[fish_counted] == fish_quantities[fish_counted -= 1] && fish_counted - 1 > 0)
            {
                yield return new WaitForSeconds(fish_quantity);
            }
            else
            {
                Debug.Log("identical");
                fish[randomIndex].transform.position = new Vector3(0, 10, 0);
            }*/

            //fish_that_exists = GameObject.FindGameObjectsWithTag("fish");

            //fish_exist = GameObject.FindGameObjectsWithTag("fish");
            //Debug.Log(fish_exist.Count);
            yield return new WaitForSeconds(1);
        }

        while (resetting == true && fish_all_spawned == false)
        {
            
        }
        

       
        
        

        /*fish[randomIndex].GetComponent<attach_to_object>().Object_b = self;
        fish[randomIndex].GetComponent<attach_to_object>().attachment = true;

        Debug.Log("fish attached");
        yield return new WaitForSeconds(1f);
        fish[randomIndex].GetComponent<attach_to_object>().attachment = false;
        Debug.Log("fish unattatched");*/

        //yield return new WaitForSeconds(0.1f);

    }

    /*public IEnumerator timemachine()
    {

        


        while (resetting == true && fish_all_spawned == false)
        {
            quantity_holder_1 = fish_quantity;
            yield return new WaitForSeconds(2f);

        }
        while (resetting == true && fish_all_spawned == false)
        {
            quantity_holder_2 = fish_quantity;
            yield return new WaitForSeconds(6f);
        }
    }*/
}
