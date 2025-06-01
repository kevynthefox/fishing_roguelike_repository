using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class fishing_bar : MonoBehaviour
{
    public float fish_num; //how many fish you caught (like, im imagining fish grabbing on to one another to help resist)
    public float fish_quality; //the quality of the fish you caught(the reasoning is that they're higher quality if they're less tired)
    
    public float bar_pos;

    public float fish_num_max; //maximum number of fish you can catch
    public float fish_quality_min; //maximum level of quality(minumum level of tiredness on the fish)

    public float effort;
    public float resistance;

    public float stopping_factor;

    public float area_difficulty;

    public Scrollbar fish_bar;
    public Text quality;
    public Text quantity;

    public Text res;
    public Text res_2;
    public Text eff;

    public Scrollbar distance_bar;

    public GameObject bone_master;

    public GameObject bobber;

    public bool failure;
    public bool success;

    public float distance;


    public void Start()
    {
        resistance = Random.Range(0.1f, area_difficulty);

        quality.text = "quality:" + fish_quality + "     max:" + fish_quality_min;
        quantity.text = "quanity:" + fish_num + "     max:" + fish_num_max;

        res.text = resistance.ToString("0.0");
        res_2.text = "" + area_difficulty;
        eff.text = "force:" + effort;
    }

    void Update()
    {

        fish_num = fish_num_max * bar_pos; //the more the bar goes up, the more fish are caught
        fish_quality = fish_quality_min * (1-bar_pos); // the more the bar goes down, the higher quality of the fish caught.

       
        //floors the numbers to be at minumum of 1
        if (fish_num <= 0.5) fish_num = 0.5f;
        if (fish_quality <= 0.5) fish_quality = 0.5f;
        
        

        StartCoroutine(catch_mechanic());

        fish_bar.value = bar_pos;
        
        StopCoroutine(catch_mechanic());

        quality.text = "quality:" + fish_quality.ToString("0.0") + "     max:" + fish_quality_min;
        quantity.text = "quanity:" + fish_num.ToString("0.0") + "     max:" + fish_num_max;

        distance = distance_bar.GetComponent<distance_bar>().current_distance;

        if (bobber.GetComponent<bobber_impact>().resetting == false)
        {
            if (bar_pos <= 0 + (resistance * Time.deltaTime) || bar_pos >= 1 - (effort * Time.deltaTime))
            {
                failure = true;
                success = false;
                Debug.Log("failure");
            }
            else
            {
                if (distance <= 0)
                {
                    failure = false;
                    success = true;
                    Debug.Log("success");
                }
            }
        }
        if (failure == true)
        {
            bone_master.GetComponent<variable_length>().enabled_fishing = false;
        }
    }
    
    public IEnumerator catch_mechanic()
    {
        

        if ((Input.GetMouseButton(1))) //e is just a placeholder. it will later be changed to the reel in mouse button
        {
            if (bar_pos <= 1 - (effort * Time.deltaTime))
            {
                bar_pos += effort * Time.deltaTime;
            }

            if (bar_pos >= 0 + (resistance * Time.deltaTime))
            {
                bar_pos -= resistance * Time.deltaTime / stopping_factor;
            }

        }
        else
        {
            if (bar_pos >= 0 + (resistance * Time.deltaTime))
            {
                bar_pos -= resistance * Time.deltaTime;
            }

        }

        yield return new WaitForSeconds(100f);

        
    }
}
