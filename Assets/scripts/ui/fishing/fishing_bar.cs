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
    public float fish_quality_max; //maximum level of quality(minumum level of tiredness on the fish)

    public float fish_num_min;
    public float fish_quality_min;

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

    public int direction;
    public int direction_max;
    public int direction_min;


    public void Start()
    {
        resistance = Random.Range(0.1f, area_difficulty);

        //direction = Random.Range(1, 2);
        //Debug.Log("direction:" + direction);

        quality.text = "quality:" + fish_quality.ToString("0.0") + "     max:" + fish_quality_max + "     min:" + fish_quality_min;
        quantity.text = "quanity:" + fish_num.ToString("0.0") + "    max:" + fish_num_max + "     min:" + fish_num_min;

        res.text = resistance.ToString("0.0");
        res_2.text = "" + area_difficulty;
        eff.text = "force:" + effort;

        direction_max += 1;
    }

    void Update()
    {

        fish_num = fish_num_max * bar_pos; //the more the bar goes up, the more fish are caught
        fish_quality = fish_quality_max * (1-bar_pos); // the more the bar goes down, the higher quality of the fish caught.

       
        //floors the numbers to be at minumum of 1
        if (fish_num <= fish_num_min) fish_num = fish_num_min;
        if (fish_quality <= fish_quality_min) fish_quality = fish_quality_min;

        direction = Random.Range(direction_min,direction_max);
        //Debug.Log("direction:" + direction);

        StartCoroutine(catch_mechanic());

        fish_bar.value = bar_pos;
        
        StopCoroutine(catch_mechanic());

        quality.text = "quality:" + fish_quality.ToString("0.0") + "     max:" + fish_quality_max + "     min:" + fish_quality_min;
        quantity.text = "quanity:" + fish_num.ToString("0.0") + "    max:" + fish_num_max + "     min:" + fish_num_min;

        distance = distance_bar.GetComponent<distance_bar>().current_distance;

        if (bobber.GetComponent<bobber_impact>().resetting == false)
        {
            if (bar_pos <= 0 + (resistance * Time.deltaTime * direction) || bar_pos >= 1 - (effort * Time.deltaTime * direction))
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
            success = false;
        }
    }
    
    public IEnumerator catch_mechanic()
    {
        if (bobber.GetComponent<bobber_impact>().spawning_fish == false)
        {
            if ((Input.GetMouseButton(1))) //e is just a placeholder. it will later be changed to the reel in mouse button
            {


                if (bar_pos <= 1 - (effort * Time.deltaTime))// && direction == 0)
                {
                    bar_pos += effort * Time.deltaTime * (Mathf.Abs(direction) / (2 * stopping_factor));
                }

                /*if (bar_pos <= 1 - (effort * Time.deltaTime) && direction == 1)
                {
                    bar_pos -= effort * Time.deltaTime;
                }*/

                /*if (bar_pos >= 0 + (resistance * Time.deltaTime))// && direction == 0)
                 {
                     bar_pos -= resistance * (Time.deltaTime * direction)/ stopping_factor;
                 }*/

                /*if (bar_pos >= 0 + (resistance * Time.deltaTime) && direction == 1)
                {
                    bar_pos += resistance * Time.deltaTime / stopping_factor;
                }*/

                //direction = Random.Range(-2, 2);
                //Debug.Log("direction:" + direction);

                yield return new WaitForSeconds(1f);
            }
            if ((Input.GetMouseButton(0))) //e is just a placeholder. it will later be changed to the reel in mouse button
            {


                if (bar_pos >= 0 + (effort * Time.deltaTime))// && direction == 0)
                {
                    bar_pos += effort * Time.deltaTime * -(Mathf.Abs(direction) / (2 * stopping_factor));
                }

                /*if (bar_pos <= 1 - (effort * Time.deltaTime) && direction == 1)
                {
                    bar_pos -= effort * Time.deltaTime;
                }*/

                /*if (bar_pos >= 0 + (resistance * Time.deltaTime))// && direction == 0)
                {
                    bar_pos -= resistance * (Time.deltaTime * direction)/ stopping_factor;
                }*/

                /*if (bar_pos >= 0 + (resistance * Time.deltaTime) && direction == 1)
                {
                    bar_pos += resistance * Time.deltaTime / stopping_factor;
                }*/

                //direction = Random.Range(-2, 2);
                //Debug.Log("direction:" + direction);

                yield return new WaitForSeconds(1f);
            }

            if ((Input.GetMouseButton(0)) || (Input.GetMouseButton(1)))
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

                /*if (bar_pos >= 0 + (resistance * Time.deltaTime) && direction == 1)
                {
                    bar_pos += resistance * Time.deltaTime;
                }*/

                //direction = Random.Range(0, 2);
                //Debug.Log("direction:" + direction);

                yield return new WaitForSeconds(1f);
            }

            yield return new WaitForSeconds(100f);

        }
    }
}
