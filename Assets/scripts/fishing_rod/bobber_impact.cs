using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobber_impact : MonoBehaviour
{
    [SerializeField] Animator animator;
    public GameObject fishing_rod_1;

    public float fishing_time;
    public float fishing_time_cool;

    public bool returned = false;

    private void Start()
    {
        fishing_time = fishing_rod_1.GetComponent<fishing_rod_movement>().fishing_time;
        fishing_time_cool = fishing_rod_1.GetComponent<fishing_rod_movement>().fishing_time_cool;
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (returned == false)
        {
            if (other.gameObject.tag == "water")
            {
                Debug.Log("water");
                yield return new WaitForSeconds(.5f);
                animator.SetBool("is_waiting", true);
                animator.SetBool("is_hooked", false);

                yield return new WaitForSeconds(fishing_time);

                animator.SetBool("is_hooked", true);
                yield return new WaitForSeconds(fishing_time); //placeholder for finishing the fishing game
                animator.SetBool("is_hooked", false);
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
            yield return new WaitForSeconds(10f);
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
