using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishing_rod_movement : MonoBehaviour
{

	[SerializeField] Animator animator;
	public GameObject fishing_rod_1;
    public GameObject bone;
    public GameObject bobber;

    public float fishing_time;
    public float fishing_time_cool;
    public bool reel_able;
    public bool fishable;

    public int fight_animation;
    public bool loop_animation;

    public float loop_time_start;

    public bool blocking;

	
      void Start()
    {
       

    }

	void Update()
	{
        fishable = bone.GetComponent<variable_length>().enabled_fishing;
        //Debug.Log("blocking_state: " + blocking);

        if (fishable == true)
        {

            reset_animations();
            bobber.GetComponent<bobber_launch>().factor = bone.GetComponent<variable_length>().distance;

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("is_in_use", true);
                reel_able = true;
                bobber.GetComponent<bobber_launch>().factor = bone.GetComponent<variable_length>().distance;
                bobber.GetComponent<bobber_launch>().enabled = true;
                bobber.GetComponent<bobber_impact>().returned = false;
                //animator.SetBool("is_hoooked", false);
                animator.SetBool("is_waiting", false);
            }
            if (Input.GetMouseButtonUp(0))
            {
                //animator.SetBool("is_in_use", false);
                //reel_able = false;
                bobber.GetComponent<bobber_launch>().factor = 0;
                bobber.GetComponent<bobber_launch>().enabled = false;
            }

            if (reel_able)
            {

                if (Input.GetMouseButtonDown(1))
                {
                    //animator.SetBool("is_in_use", false);
                    //animator.SetBool("is_in_reel", true);
                    /*animator.SetBool("is_hooked", false);
                    animator.SetBool("is_waiting", false);
                    */
                    //StartCoroutine(reel_in());
                }
            }
            else
            {
                //animator.SetBool("is_in_reel", false);
            }
        }
        else
        {
            reel_able = false;
            fishable = false;
            animator.SetBool("is_in_use", false);
            //animator.SetBool("is_in_reel", false);
            //animator.SetBool("is_hooked", false);
            animator.SetBool("is_waiting", false);

            

            if (Input.GetMouseButtonDown(0))
            {
                fight_animation = Random.Range(0, 2);
                if (fight_animation == 0)
                {
                    animator.SetBool("fighting_1", true);
                }
                if (fight_animation == 1)
                {
                    animator.SetBool("fighting_2", true);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                blocking = true;
                loop_animation = true;
                fight_animation = Random.Range(0, 2);
                
                if (fight_animation == 0)
                {
                    animator.SetBool("blocking_1", true);
                }
                if (fight_animation == 1)
                {
                    animator.SetBool("blocking_2", true);
                }
                
            }
            if (Input.GetMouseButtonUp(0))
            {
                animator.SetBool("fighting_1", false);
                animator.SetBool("fighting_2", false);
            }
            if (Input.GetMouseButtonUp(1))
            {
                loop_animation = false;
                blocking = false;
                animator.SetBool("blocking_1", false);
                animator.SetBool("blocking_2", false);
            }
        }

    }

    public void reset_animations()
    {
        animator.SetBool("fighting_1", false);
        animator.SetBool("fighting_2", false);
        loop_animation = false;
        blocking = false;
        animator.SetBool("blocking_1", false);
        animator.SetBool("blocking_2", false);
    }


    /*public void rewind_animation()
    {
        if (loop_animation == true)
        {
            
            animator.playbackTime = loop_time_start;
            //Debug.Log("playbacktime now: " + animator.playbackTime);
        }
    }

    public void get_time_animation()
    {
        loop_time_start = animator.playbackTime;

        //Debug.Log("playbacktime loop start: " + animator.playbackTime);
    }   */
    
    public void broadcast()
    {
        Debug.Log("this position has been reached");
    }

    


}
    

    