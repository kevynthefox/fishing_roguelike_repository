using System.Collections;
using UnityEngine;

public class Auto_fisher : MonoBehaviour
{
    public float time_between_fishing;
    public bool able_to_fish;
    
    public bool fish;

    //public bool starter = true;

    public Animator animator;

    public GameObject[] fish_to_spawn;

    public GameObject bobber;
    public GameObject spawn_area;

    public int random_spawn;

    public int fish_counted;

    public int where_fish_is_in_list;

    public int spawn_at_all_controller; 
    public int expertise;
    public bool spawn_at_all;

    public float fish_quantity; //how many fish you caught (like, im imagining fish grabbing on to one another to help resist)
    public float fish_quality; //the quality of the fish you caught(the reasoning is that they're higher quality if they're less tired)

    public float fish_quantity_max; //maximum number of fish you can catch
    public float fish_quality_max; //maximum level of quality(minumum level of tiredness on the fish)

    public float fish_quantity_min;
    public float fish_quality_min;


    public float fish_quantity_buff_mult;
    public float fish_quality_buff_mult;

    public float fish_quantity_max_buff_mult;
    public float fish_quality_max_buff_mult;

    public float fish_quantity_min_buff_mult;
    public float fish_quality_min_buff_mult;


    public float fish_quantity_buff_add;
    public float fish_quality_buff_add;

    public float fish_quantity_max_buff_add;
    public float fish_quality_max_buff_add;

    public float fish_quantity_min_buff_add;
    public float fish_quality_min_buff_add;

    public float fish_potency_buff_mult;
    public float fish_potency_buff_add;



    private void Start()
    {
        StartCoroutine(fish_anim());
    }

    public IEnumerator spawn_fish()
    {
        Wavespawner.current.sources_of_fish.Add(this.gameObject);

        //Debug.Log("spawning fish");

        fish = false;
        able_to_fish = false;

        randomize_fish_variables();

        if (spawn_at_all == true)
    {
            
            for (int i = 0; i < fish_quantity; )
            {
                
                fish_to_spawn = bobber.GetComponent<auto_fisher_fish_getter>().fish_to_spawn;
                if (fish_to_spawn.Length > 0)
                {
                    random_spawn = Random.Range(0, fish_to_spawn.Length);
                }

                var new_fish = Instantiate(fish_to_spawn[random_spawn], spawn_area.transform);

                new_fish.GetComponent<fish_variable_holder>().fish_quality = fish_quality;

                new_fish.GetComponent<fish_variable_holder>().potentcy += fish_potency_buff_add; new_fish.GetComponent<fish_variable_holder>().potentcy *= fish_potency_buff_mult;


                

                

                new_fish.transform.parent = null;
                if (fish_quantity >= 1)
                {
                    new_fish.GetComponent<Transform>().localScale = new Vector3(fish_quality, fish_quality, fish_quality);
                    new_fish.name = "big fish";
                }
                else
                {
                    new_fish.GetComponent<Transform>().localScale = new Vector3(fish_quantity, fish_quantity, fish_quantity);
                    new_fish.name = "small fish";
                }

                


                if (fish_quantity >= 1000)
                {
                    fish_counted += Mathf.RoundToInt(fish_quantity/1000);
                    
                    new_fish.GetComponent<fish_variable_holder>().fish_quantity = fish_quantity/1000;

                    for (int f = 0; f < fish_quantity/1000; f++)
                    {
                        Wavespawner.current.Add_dead(Wavespawner.current.fishes[new_fish.GetComponent<fish_variable_holder>().fish_type]);
                    }

                    i += Mathf.RoundToInt(fish_quantity / 1000);
                    //Debug.Log("over 1000 " + i);
                    //Debug.Log("fish_quantity/1000 " + Mathf.RoundToInt(fish_quantity / 1000));
                }
                else
                {
                    fish_counted++;
                    
                    new_fish.GetComponent<fish_variable_holder>().fish_quantity = 1;
                    Wavespawner.current.Add_dead(Wavespawner.current.fishes[new_fish.GetComponent<fish_variable_holder>().fish_type]);
                    i++;
                    //Debug.Log("under 1000 " + i);
                    //Debug.Log("fish quantity " + fish_quantity);
                }


                yield return new WaitForSeconds(1 / fish_quantity);

                /*where_fish_is_in_list = Wavespawner.current.dead_fish.IndexOf(Wavespawner.current.Get(new_fish));
                //Wavespawner.current.fish_potency_buff_add += fish_potency_buff_add; Wavespawner.current.fish_potency_buff_mult += fish_potency_buff_mult;
                Wavespawner.current.dead_fish[Wavespawner.current.dead_fish.IndexOf(Wavespawner.current.Get(new_fish))].fish_potency_buff_add = fish_potency_buff_add;
                Wavespawner.current.dead_fish[Wavespawner.current.dead_fish.IndexOf(Wavespawner.current.Get(new_fish))].fish_potency_buff_mult = fish_potency_buff_mult;*/
                // you don't actually need this part because when you use this item... it's gonna affect the fish anyway.
                //unless you're gonna give them seperate inventories.. which kinda defeats the point?
                if (i >= fish_quantity)
                {
                    erase_values();
                    able_to_fish = true;
                }
            }
            

        }




    }
    private bool already_sent_starter_inactive;
    private bool already_sent_starter_active;
    public void Update()
    {
        if (TimeManager.current.update == true)
        {
            animator.SetBool("fishing", fish);
        }

        if (TimeManager.current.starter_reignitable == true)
        {
            if (TimeManager.current.starter == true)
            {
                if (already_sent_starter_active == false)
                {
                    already_sent_starter_inactive = false;
                    TimeManager.current.starters_inactive -= 1;
                    already_sent_starter_active = true;
                    StartCoroutine(fish_anim());
                }
            }
        }
        if (TimeManager.current.starter == false)
        {
            already_sent_starter_active = false;
            if (already_sent_starter_inactive == false)
            {
                TimeManager.current.starters_inactive += 1;
                already_sent_starter_inactive = true;
            }
        }
    }


    public void erase_values()
    {
        if (spawn_at_all == true)
        {
            //Debug.Log("erasing values");
            bobber.GetComponent<auto_fisher_fish_getter>().clearlist();
            Wavespawner.current.fish_total += fish_counted;
            fish_quantity_buff_mult = 1;
            fish_quality_buff_mult = 1;

            fish_quantity_max_buff_mult = 1;
            fish_quality_max_buff_mult = 1;

            fish_quantity_min_buff_mult = 1;
            fish_quality_min_buff_mult = 1;


            fish_quantity_buff_add = 0;
            fish_quality_buff_add = 0;

            fish_quantity_max_buff_add = 0;
            fish_quality_max_buff_add = 0;

            fish_quantity_min_buff_add = 0;
            fish_quality_min_buff_add = 0;

            fish_potency_buff_mult = 1;
            fish_potency_buff_add = 0;
            Wavespawner.current.sources_of_fish.Remove(this.gameObject);
        }
    }

    public IEnumerator fish_anim()
    {
        while (TimeManager.current.starter == true)
        {
            if (Wavespawner.current.stop_fishing == false)
            {
                if (fish == false && able_to_fish == true)
                {


                    fish = true;
                    //Debug.Log("setting fish true");
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                    //Debug.Log("nothing");
                }
            }
            else
            {
                fish = false;
                //Wavespawner.current.sources_of_fish.Remove(this.gameObject);
            }
            yield return new WaitForSeconds(time_between_fishing);
        }
    }

    public void randomize_fish_variables()
    {
        spawn_at_all_controller = Random.Range(0, 101);

        if (spawn_at_all_controller <= expertise)
        {
            spawn_at_all = false;
        }
        else
        {
            spawn_at_all = true;
        }   

        if (spawn_at_all == true)
        {
            fish_quantity = Random.Range((fish_quantity_min + fish_quantity_min_buff_add) * fish_quantity_min_buff_mult, (fish_quantity_max + fish_quantity_max_buff_add) * fish_quantity_max_buff_mult);
            fish_quality = Random.Range((fish_quality_min + fish_quality_min_buff_add) * fish_quality_min_buff_mult, (fish_quality_max + fish_quality_max_buff_add) * fish_quality_max_buff_mult);

        }
    }

}
