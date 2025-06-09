using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class rerolling : MonoBehaviour
{
    //public GameObject reroll;
    public GameObject whole_shop;

    public GameObject self;

    public Text cost_text;

    public GameObject gamesettings;

    [SerializeField] Animator animator;

    public GameObject player;

    public bool starter;

    public float item_cost;
    public float item_original_cost;
    public float cost_percent;

    public void Start()
    {
        starter = true;
        StartCoroutine(rotater());
    }

    public void LateUpdate()
    {
        cost_percent = gamesettings.GetComponent<settings>().cost_percent / 100;

        item_cost = cost_percent * item_original_cost;

        cost_text.text = item_cost.ToString();
    }

    public IEnumerator rotater()
    {
        while (starter == true)
        {
            if (player.GetComponent<money_collector>().money_value >= item_cost)
            {

                if (self.GetComponent<object_click_detector>().left_clicked == true)
                {
                    player.GetComponent<money_collector>().money_value -= item_cost;

                    Debug.Log("recieved 2");
                    whole_shop.GetComponent<item_manifestation>().item_unmaker();
                    animator.SetBool("reroll", true);
                    yield return new WaitForSeconds(1f);
                    animator.SetBool("reroll", false);
                    yield return new WaitForSeconds(1f);
                    whole_shop.GetComponent<item_manifestation>().item_maker();
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }    

    

    
}
