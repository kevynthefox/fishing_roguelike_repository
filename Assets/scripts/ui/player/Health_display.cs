using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_display : MonoBehaviour
{


public float health,health_max;

public float FatigueCost;
public float ChargeRate;

public Image HealthBar;
public Text healthText;
public GameObject hunger;
public GameObject player;
public GameObject respawn_point;

private Coroutine recharge;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
//    void Update()
//    {
//
//        FatigueCost = hunger.GetComponent<Hunger_display>().HungerCost;
//
//
//        healthText.text = "health : " + health;
//        HealthBar.fillAmount = health / health_max;
//
//        if (health <= 0) 
//        {
//            health = 0;
//            player.transform.position = respawn_point.transform.position;
//            health = health_max;
//        }

//		if (hunger.GetComponent<Hunger_display>().hunger <= 0)
//		{
//            health -= FatigueCost;
//            hunger.GetComponent<Hunger_display>().hunger += hunger.GetComponent<Hunger_display>().hunger_max;	
//		}
		

//    }
//


}
