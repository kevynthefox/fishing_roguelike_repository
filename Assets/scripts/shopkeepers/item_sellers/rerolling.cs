using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class rerolling : MonoBehaviour
{
    //public GameObject reroll;
    public GameObject whole_shop;

    public GameObject self;

    [SerializeField] Animator animator;

    public bool starter;

    public void Start()
    {
        starter = true;
        StartCoroutine(rotater());
    }


    public IEnumerator rotater()
    {
        while (starter == true)
        {
            if (self.GetComponent<object_click_detector>().left_clicked == true)
            {
                Debug.Log("recieved 2");
                whole_shop.GetComponent<item_manifestation>().item_unmaker();
                animator.SetBool("reroll", true);
                yield return new WaitForSeconds(1f);
                animator.SetBool("reroll", false);
                yield return new WaitForSeconds(1f);
                whole_shop.GetComponent<item_manifestation>().item_maker();
            }
            yield return new WaitForSeconds(1f);
        }
    }    

    

    
}
