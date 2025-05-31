using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hunger_display : MonoBehaviour
{


public float hunger,hunger_max;

public float HungerCost;
public float ChargeRate;

//public float health_;
//public float health_max_;

public Image HungerBar;
public Text hungerText;
public GameObject stamina;
//public GameObject health;
public bool recharging;
public bool charging_stamina;
public bool is_eating;
public GameObject player;
public GameObject GameOverScreen;
public GameObject camera1;
public GameObject camera2;

private Coroutine recharge;

    // Start is called before the first frame update
    public void Start()
    {
      //  health_ = health.GetComponent<Health_display>().health;
      //  health_max_ = health.GetComponent<Health_display>().health_max;
        hungerText.text = "hunger : " + hunger;
        HungerBar.fillAmount = hunger / hunger_max;
        player = GameObject.Find("player");

    }


    // Update is called once per frame
    void Update()
    {

        HungerCost = stamina.GetComponent<Stamina_display>().ChargeRate;


        if (hunger <= 0)
        {
            hunger = 0;
            GameOverScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            
            //this is the part that makes the player unable to move and etc, when they run out of hunger
            
            player.GetComponent<movement>().enabled = false;
            player.GetComponent<camera_controls>().enabled = false;
            camera1.GetComponent<camera_rotate>().enabled = false;
            camera2.GetComponent<camera_rotate>().enabled = false;
        }
        else
        { 
            GameOverScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;

            player.GetComponent<movement>().enabled = true;
            player.GetComponent<camera_controls>().enabled = true;
            camera1.GetComponent<camera_rotate>().enabled = true;
            camera2.GetComponent<camera_rotate>().enabled = true;
        }


        if (stamina.GetComponent<Stamina_display>().recharging == true)
		{
            
            
            
            charging_stamina = true;
            hungerText.text = "hunger : " + hunger;
            HungerBar.fillAmount = hunger / hunger_max;
        }





    }

 

}
