using System.Collections;
using UnityEngine;

public class Auto_fisher : MonoBehaviour
{
    public float time_between_fishing;
    
    public bool fish;

    public bool starter = true;

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


        //Debug.Log("spawning fish");

        fish = false;

        randomize_fish_variables();

        if (spawn_at_all == true)
        {
            for (int i = 0; i < fish_quantity; i++)
            {
                Debug.Log(i);
                fish_to_spawn = bobber.GetComponent<auto_fisher_fish_getter>().fish_to_spawn;
                if (fish_to_spawn.Length > 0)
                {
                    random_spawn = Random.Range(0, fish_to_spawn.Length);
                }

                var new_fish = Instantiate(fish_to_spawn[random_spawn], spawn_area.transform);
                
                new_fish.GetComponent<fish_variable_holder>().fish_quality = fish_quality;

                new_fish.GetComponent<fish_variable_holder>().potentcy += fish_potency_buff_add; new_fish.GetComponent<fish_variable_holder>().potentcy *= fish_potency_buff_mult;

                if (fish_quantity > 1000)
                {

                }
                else
                {
                    fish_counted++;
                }


                new_fish.transform.parent = null;
                if (fish_quantity >= 1)
                {
                    new_fish.GetComponent<Transform>().localScale = new Vector3(fish_quality, fish_quality, fish_quality);
                }
                else
                {
                    new_fish.GetComponent<Transform>().localScale = new Vector3(fish_quantity, fish_quantity, fish_quantity);
                }

                Wavespawner.current.Add_dead(Wavespawner.current.fishes[new_fish.GetComponent<fish_variable_holder>().fish_type]);
                
                Wavespawner.current.fish_total = fish_counted;
                new_fish.GetComponent<fish_variable_holder>().fish_quantity = 1;

                yield return new WaitForSeconds(1 / fish_quantity);

                /*where_fish_is_in_list = Wavespawner.current.dead_fish.IndexOf(Wavespawner.current.Get(new_fish));
                //Wavespawner.current.fish_potency_buff_add += fish_potency_buff_add; Wavespawner.current.fish_potency_buff_mult += fish_potency_buff_mult;
                Wavespawner.current.dead_fish[Wavespawner.current.dead_fish.IndexOf(Wavespawner.current.Get(new_fish))].fish_potency_buff_add = fish_potency_buff_add;
                Wavespawner.current.dead_fish[Wavespawner.current.dead_fish.IndexOf(Wavespawner.current.Get(new_fish))].fish_potency_buff_mult = fish_potency_buff_mult;*/
                // you don't actually need this part because when you use this item... it's gonna affect the fish anyway.
                //unless you're gonna give them seperate inventories.. which kinda defeats the point?
            }
        }
        bobber.GetComponent<auto_fisher_fish_getter>().clearlist();

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

}

    public void Update()
    {
        animator.SetBool("fishing", fish);
    }

    public IEnumerator fish_anim()
    {
        while (starter == true)
        {
            if (fish == false)
            {
                yield return new WaitForSeconds(time_between_fishing);
                fish = true;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }    
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
