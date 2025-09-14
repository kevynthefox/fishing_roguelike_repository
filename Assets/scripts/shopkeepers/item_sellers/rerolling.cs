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

    //public bool starter;

    public float item_cost;
    public float item_original_cost;
    public float cost_percent;

    public void Start()
    {
        //starter = true;
        StartCoroutine(rotater());
    }

    public void LateUpdate()
    {
        if (TimeManager.current.update == true)
        {
            cost_percent = gamesettings.GetComponent<settings>().cost_percent / 100;

            item_cost = cost_percent * item_original_cost;

            cost_text.text = item_cost.ToString();
        }
    }

    private bool already_sent_starter_inactive;
    private bool already_sent_starter_active;
    private void Update()
    {
        if (TimeManager.current.starter_reignitable == true)
        {
            if (TimeManager.current.starter == true)
            {
                if (already_sent_starter_active == false)
                {
                    already_sent_starter_inactive = false;
                    TimeManager.current.starters_inactive -= 1;
                    already_sent_starter_active = true;
                    StartCoroutine(rotater());
                }
            }
        }
        if (TimeManager.current.starter == false)
        {
            already_sent_starter_active = false;
            if (already_sent_starter_inactive == false)
            {
                TimeManager.current.starters_inactive += 1;
                already_sent_starter_inactive = true;
            }
        }
    }

    public IEnumerator rotater()
    {
        while (TimeManager.current.starter == true)
        {
            if (whole_shop.GetComponent<item_manifestation>().checking_out == false)
            {
                if (player.GetComponent<money_collector>().money_value >= item_cost)
                {

                    if (self.GetComponent<object_click_detector>().left_clicked == true)
                    {
                        //self.GetComponent<object_click_detector>().click_override = true;
                        player.GetComponent<money_collector>().money_value -= item_cost;

                        //Debug.Log("recieved 2");
                        whole_shop.GetComponent<item_manifestation>().item_unmaker();
                        animator.SetBool("reroll", true);
                        yield return new WaitForSeconds(1f);
                        animator.SetBool("reroll", false);
                        yield return new WaitForSeconds(1f);
                        whole_shop.GetComponent<item_manifestation>().item_maker();
                        //self.GetComponent<object_click_detector>().click_override = false;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("impact");
        if (other.tag == "currency_transit")
        {
            //Debug.Log("impact");
            //money_owed -= other.GetComponent<money_value_holder>().value;
            Destroy(other.gameObject);
        }
        //yield return new WaitForSeconds(0.1f);
    }


}
