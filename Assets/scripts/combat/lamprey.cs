using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class lamprey : MonoBehaviour
{
    public bool manual_fire;
    public bool always_fire_at_targets;

    public List<Transform> targets;
    public List<GameObject> heal_targets;

    public bool player_gun;

    public GameObject mouth;

    public float health_pool;

    public float stretch_rate;
    public float drain_rate;

    public bool healing;

    public void Start()
    {
        var rb = gameObject.AddComponent<Rigidbody>();
        Destroy(rb);
    }

    public void Update()
    {
        if (Starter.current.update == true)
        {



            if (targets.Count >= 1)
            {
                if (mouth.GetComponent<lamprey_mouth>().touching_something == false)
                {

                    mouth.transform.localScale += new Vector3(0, 0, stretch_rate);
                }
                mouth.transform.LookAt(targets[0]);
            }
            else
            {
                mouth.transform.localScale = Vector3.one;
            }


            //if (healing == true)
            //{
            foreach (GameObject heal_targ in heal_targets)
            {
                if (health_pool > 0)
                {
                    if (heal_targ.GetComponent<Damage>().invincibility == false)
                    {

                        if (heal_targ.transform.GetComponentInChildren<Health_display>().health < heal_targ.transform.GetComponentInChildren<Health_display>().health_max)
                        {
                            healing = true;
                            //Debug.Log("attempting heal on: " + heal_targ.name);
                            heal_targ.transform.GetComponentInChildren<Health_display>().health += drain_rate;
                            health_pool -= drain_rate;


                        }
                    }

                }
                else
                {
                    healing = false;
                    health_pool = 0;
                    //Debug.Log("can't because they're full.");
                }

            }
            //}

            CleanUpMyList();
        }
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {



        if (player_gun == true)
        {
            if (other.CompareTag("fish") || other.CompareTag("fish_enemy") || other.CompareTag("super_food_items"))
            {
                if (targets.Contains(other.transform) == false)
                {
                    if (other.GetComponent<heat_seeking_fishles>().enemy == true) targets.Add(other.transform);
                }
            }
        }
        else
        {
            if (other.CompareTag("player"))
            {
                if (targets.Contains(other.transform) == false)
                {
                    targets.Add(other.transform);
                }
            }
        }
        yield return null;
    }

    public IEnumerator OnTriggerExit(Collider other)
    {
        


        if (targets != null)
        {
            if (targets.Count > 0)
            {
                if (targets[0] != null)
                { 

                    if (targets.Contains(other.transform) == true)
                    {
                        targets.Remove(other.transform);
                    }
                }
                else
                {
                    Debug.Log("target 0 was null");
                }
            }
        }

        if (heal_targets != null)
        {
            if (heal_targets.Count > 0)
            {
                if (heal_targets[0] != null)
                {

                    if (heal_targets.Contains(other.gameObject) == true)
                    {
                        heal_targets.Remove(other.gameObject);
                    }
                }
                else
                {
                    Debug.Log("target 0 was null");
                }
            }
        }


        yield return null;
    }

    public void OnTriggerStay(Collider other)
    {
        

        if (other.CompareTag("player"))
        {
            //Debug.Log("toucjed anopther player");
            if (heal_targets.Contains(other.gameObject) == false)
            {
                heal_targets.Add(other.gameObject);
            }

            //other.GetComponent<Health_display>().health += 0.1f;
            if (other.GetComponent<Damage>().invincibility == false)
            {

                if (other.transform.GetComponentInChildren<Health_display>().health < other.transform.GetComponentInChildren<Health_display>().health_max)
                {
                    healing = true;
                    

                }
            }
        }

        if (player_gun == true)
        {
            if (other.CompareTag("fish") || other.CompareTag("fish_enemy") || other.CompareTag("super_food_items"))
            {
                if (targets.Contains(other.transform) == false)
                {
                    if (other.GetComponent<heat_seeking_fishles>().enemy == true) targets.Add(other.transform);
                }
            }
        }
        else
        {
            if (other.CompareTag("player"))
            {
                if (targets.Contains(other.transform) == false)
                {
                    targets.Add(other.transform);
                }
            }
        }
    }

    public void CleanUpMyList()
    {
        targets = targets.Where(t => t != null).ToList();
        heal_targets = heal_targets.Where(t => t != null).ToList();
    }
}
