using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bobber_impact : MonoBehaviour
{
    [SerializeField] Animator animator;
    public GameObject fishing_rod_1;

    public GameObject fishing_system;
    public Scrollbar fishing_bar;
    public Scrollbar distance_bar;
    public GameObject bone_master;

    public float fishing_time;
    public float fishing_time_cool;

    public bool returned = false;

    private void Start()
    {
        fishing_time = fishing_rod_1.GetComponent<fishing_rod_movement>().fishing_time;
        fishing_time_cool = fishing_rod_1.GetComponent<fishing_rod_movement>().fishing_time_cool;
        fishing_system.SetActive(false);
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        
        if (returned == false)
        {
            if (other.gameObject.tag == "water")
            {
                fishing_system.SetActive(true);
                fishing_bar.value = 0.5f;
                Debug.Log("water");
                yield return new WaitForSeconds(.5f);
                animator.SetBool("is_waiting", true);
                animator.SetBool("is_hooked", false);

                yield return new WaitForSeconds(fishing_time);

                animator.SetBool("is_hooked", true);
                if (fishing_bar.GetComponent<fishing_bar>().success == true)
                {
                    animator.SetBool("is_hooked", false); //spawn a fish here
                    Debug.Log("success");
                    yield return new WaitForSeconds(2f);
                    fishing_bar.GetComponent<fishing_bar>().success = false;
                    fishing_bar.GetComponent<fishing_bar>().failure = false;
                    fishing_bar.value = 0.5f;
                    Debug.Log("reset");
                }
                else
                {
                    animator.SetBool("is_hooked", false);
                    Debug.Log("failure");
                    yield return new WaitForSeconds(2f);
                    fishing_bar.GetComponent<fishing_bar>().success = false;
                    fishing_bar.GetComponent<fishing_bar>().failure = false;
                    bone_master.GetComponent<variable_length>().enabled_fishing = true;
                    fishing_bar.value = 0.5f;
                    Debug.Log("reset. E has been pressed to try again");
                }
            }
            else
            {
                if (other.gameObject.tag == "fishing_rod")
                {
                    
                    Debug.Log("rod");
                    yield return new WaitForSeconds(.5f);
                    fishing_rod_1.GetComponent<fishing_rod_movement>().reel_able = false;
                    reset_animations();
                    returned = true;
                }
                else
                {
                    yield return new WaitForSeconds(100f);
                    Debug.Log("air");
                    animator.SetBool("is_waiting", false);
                    animator.SetBool("is_hooked", false);
                }
            }
        }
        else
        {
            StartCoroutine(reset_animations());
            fishing_system.SetActive(false);
            yield return new WaitForSeconds(10f);
            StopCoroutine(reset_animations());
            //fishing_rod_1.GetComponent<fishing_rod_movement>().reel_able = false;
        }
        /*
        if (other.gameObject.tag == "fishing_rod")
        {
            Debug.Log("rod");
            yield return new WaitForSeconds(.5f);
            fishing_rod_1.GetComponent<fishing_rod_movement>().reel_able = false;
            animator.SetBool("is_in_use", false);

        }
        */
    }


    public IEnumerator reset_animations()
    {
        animator.SetBool("is_in_use", false);
        animator.SetBool("is_in_reel", false);
        animator.SetBool("is_waiting", false);
        animator.SetBool("is_hooked", false);
        yield return new WaitForSeconds(0.1f);
    }
}
