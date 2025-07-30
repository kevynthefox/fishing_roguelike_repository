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

    public float quantity;
    public float quality;



    private void Start()
    {
        StartCoroutine(fish_anim());
    }

    public void spawn_fish()
    {


        Debug.Log("spawning fish");

        fish = false;

        fish_to_spawn = bobber.GetComponent<auto_fisher_fish_getter>().fish_to_spawn;
        if (fish_to_spawn.Length > 0)
        {
            random_spawn = Random.Range(0, fish_to_spawn.Length);
        }
        var new_fish = Instantiate(fish_to_spawn[random_spawn], spawn_area.transform);
        new_fish.GetComponent<fish_variable_holder>().fish_quantity = quantity;
        new_fish.GetComponent<fish_variable_holder>().fish_quality = quality;

        new_fish.transform.parent = null;
        if (quantity >= 1)
        {
            new_fish.GetComponent<Transform>().localScale = new Vector3(quality, quality, quality);
        }
        else
        {
            new_fish.GetComponent<Transform>().localScale = new Vector3(quantity, quantity, quantity);
        }
        bobber.GetComponent<auto_fisher_fish_getter>().clearlist();

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

    

}
