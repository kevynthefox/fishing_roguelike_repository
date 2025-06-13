using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Stamina_display : MonoBehaviour
{


public float stamina,stamina_max;

public float SprintCost;
public float ChargeRate;
private float forwardInput;
private float horizontalInput;
public Image StaminaBar;
public Text staminaText;
public GameObject player;
public GameObject hunger;
public float hunger_;
public bool recharging;

private Coroutine recharge;

private float sprint_speed;
public float sprint_speed_mult = 2;

    // Start is called before the first frame update
    public void Start()
    {
        staminaText.text = "stamina : " + stamina;
        StaminaBar.fillAmount = stamina / stamina_max;
		sprint_speed = sprint_speed_mult * player.GetComponent<movement>().speed;
    }

    // Update is called once per frame
    void Update()
	{
		horizontalInput = player.GetComponent<movement>().horizontalInput;
		forwardInput = player.GetComponent<movement>().forwardInput;
		hunger_ = hunger.GetComponent<Hunger_display>().hunger;



        

		if (stamina <= 0) { stamina = 0; }

		if (Input.GetKey(KeyCode.LeftShift) && (forwardInput !=0 || horizontalInput !=0) && stamina >0)
        {
			stamina -= SprintCost * Time.deltaTime;
			if (recharge != null) StopCoroutine(recharge);
			recharge = StartCoroutine(RechargeStamina());
			player.GetComponent<movement>().speed = sprint_speed;
            staminaText.text = "stamina : " + stamina;
            StaminaBar.fillAmount = stamina / stamina_max;

        } 
		else
		{
			player.GetComponent<movement>().speed *= 1;
		}

    }


	private IEnumerator RechargeStamina()
	{

		yield return new WaitForSeconds(1f);
        recharging = true;
		if (hunger_ > 0)
		{ 
			while (stamina < stamina_max)
			{

			
					stamina += ChargeRate;
					hunger.GetComponent<Hunger_display>().hunger -= ChargeRate / 10;
					if (stamina > stamina_max) { stamina = stamina_max; } else { recharging = false; }
					yield return new WaitForSeconds(.1f);
					StaminaBar.fillAmount = stamina / stamina_max;
					hunger.GetComponent<Hunger_display>().HungerBar.fillAmount = hunger.GetComponent<Hunger_display>().hunger / hunger.GetComponent<Hunger_display>().hunger_max;
					staminaText.text = "stamina : " + stamina;
					hunger.GetComponent<Hunger_display>().hungerText.text = "hunger : " + hunger.GetComponent<Hunger_display>().hunger;
			}
		}
        
	}
}
