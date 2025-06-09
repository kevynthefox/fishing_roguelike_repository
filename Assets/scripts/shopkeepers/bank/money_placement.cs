using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class money_placement : MonoBehaviour
{
    /*
    public GameObject spawn_location;

    public List<GameObject> money_exist;

    public GameObject player;

    public GameObject[] money;
    
    public bool starter;

    public float money_balance;
    public float money_balance_internal;

    public void Start()
    {
        
    }

    public void Update()
    {
        Vector3 spawnpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        /*foreach (var f in GameObject.FindGameObjectsWithTag("currency"))
        {
            money_exist.Add(f);
            //yield return new WaitForSeconds(0.00000000001f);
            //Debug.Log("money_exist_count: " + money_exist.Count);        
        }
        

        if (money_balance > 0)
        {
            

            if (money_balance >= 10)
            {
                var money_visual = Instantiate(money[0], spawnpos, Quaternion.identity);
                money_visual.GetComponent<BoxCollider>().isTrigger = false;
            }
            else
            {
                if (money_balance >= 1)
                {
                    var money_visual = Instantiate(money[0], spawnpos, Quaternion.identity);
                    money_visual.GetComponent<BoxCollider>().isTrigger = false;
                }
                else
                {
                    if (money_balance < 1)
                    {
                        var money_visual = Instantiate(money[0], spawnpos, Quaternion.identity);
                        money_visual.GetComponent<BoxCollider>().isTrigger = false;

                        money_visual.GetComponent<money_value_holder>().value = money_balance;
                        money_visual.GetComponent<Transform>().localPosition = new Vector3(money_balance, money_balance, money_balance);

                        
                    }
                }
            }
        }

    }


    public IEnumerator money_internal()
    {
        money_balance = player.GetComponent<money_collector>().money_value;
        money_balance_internal = money_balance;
    }
    */
}
