using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishing_rod_movement : MonoBehaviour
{

	[SerializeField] Animator animator;
	public GameObject fishing_rod_1;
	public float fishing_time;
    public bool reel_able;
	
      void Start()
    {
       

    }

	void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			animator.SetBool("is_in_use", true);
            reel_able = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			animator.SetBool("is_in_use", false);
            reel_able = false;
		}
        if (reel_able)
        {
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetBool("is_in_reel", true);
            }
        }
        else
        {
            animator.SetBool("is_in_reel", false);
        }
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            yield return new WaitForSeconds(.5f);
            animator.SetBool("is_waiting", true);

            yield return new WaitForSeconds(fishing_time);

            animator.SetBool("is_hooked", true);
            yield return new WaitForSeconds(fishing_time); //placeholder for finishing the fishing game
            animator.SetBool("is_hooked", false);
        }
    }

   
}
    

    