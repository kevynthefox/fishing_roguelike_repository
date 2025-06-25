using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class Health_display : MonoBehaviour
{


public float health,health_max;


public Image HealthBar;
public Text healthText;

public GameObject player;
public GameObject respawn_point;

private Coroutine recharge;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        if (health <= 0)
        {
            health = 0;
            player.transform.position = respawn_point.transform.position;
            health = health_max;
            //put something here to restart the whole scene
        }

        if (health >= health_max)
        {
            health = health_max;
        }

        healthText.text = "health : " + health;
        HealthBar.fillAmount = health / health_max;


    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered");
        if (other.tag == "fish")
        {
            health -= 1;
            yield return new WaitForSeconds(1f);
        }
    }
    //


}
