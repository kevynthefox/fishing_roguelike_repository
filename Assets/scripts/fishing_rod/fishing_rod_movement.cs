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

	
      void Start()
    {
       

    }

	void Update()
	{
        fishable = bone.GetComponent<variable_length>().enabled_fishing;

        if (fishable == true)
        {
            bobber.GetComponent<bobber_launch>().factor = bone.GetComponent<variable_length>().distance;

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("is_in_use", true);
                reel_able = true;
                bobber.GetComponent<bobber_launch>().factor = bone.GetComponent<variable_length>().distance;
                bobber.GetComponent<bobber_launch>().enabled = true;
                bobber.GetComponent<bobber_impact>().returned = false;
                animator.SetBool("is_hoooked", false);
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
                    animator.SetBool("is_in_reel", true);
                    /*animator.SetBool("is_hooked", false);
                    animator.SetBool("is_waiting", false);
                    */
                    //StartCoroutine(reel_in());
                }
            }
            else
            {
                animator.SetBool("is_in_reel", false);
            }
        }
        else
        {
            reel_able = false;
            fishable = false;
            animator.SetBool("is_in_use", false);
            animator.SetBool("is_in_reel", false);
            animator.SetBool("is_hooked", false);
            animator.SetBool("is_waiting", false);
        }

    }

    //IEnumerator OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "water")
    //    {
    //        Debug.Log("water");
    //        yield return new WaitForSeconds(.5f);
    //        animator.SetBool("is_waiting", true);

    //        yield return new WaitForSeconds(fishing_time);

    //        animator.SetBool("is_hooked", true);
    //        yield return new WaitForSeconds(fishing_time); //placeholder for finishing the fishing game
    //        animator.SetBool("is_hooked", false);
    //    }
    //}



   
}
    

    