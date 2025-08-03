using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lamprey : MonoBehaviour
{
    public bool manual_fire;
    public bool always_fire_at_targets;

    public List<Transform> targets;

    public bool player_gun;

    public GameObject mouth;

    public float health_pool;

    public float time_since_firing;
    public int max_time_since_firing;

    public void Update()
    {
        mouth.transform.LookAt(targets[0]);
        if (mouth.GetComponent<lamprey_mouth>().touching_something == false)
        {

            mouth.transform.localScale += new Vector3(0, 0, 0.1f);
        }
        else
        {
            health_pool += 0.1f;
            time_since_firing = 0;
        }


        if (targets.Count > 0 && time_since_firing >= max_time_since_firing) //if gun has other targets but isn't shooting, get rid of the clog and try again.
        {
            //Debug.Log("gun was jammed. beginning clear");
            
            //targets = null;
            //targets.RemoveAt(0);
            targets.RemoveAt(0);

            //Debug.Log("jam resolved");
        }

        time_since_firing += 0.1f;
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {

        if (player_gun == true)
        {
            if (other.CompareTag("fish") || other.CompareTag("fish_enemy") || other.CompareTag("super_food_items"))
            {
                if (targets.Contains(other.transform) == false)
                {
                    targets.Add(other.transform);
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


        yield return null;
    }
}
