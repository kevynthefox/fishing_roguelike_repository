using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health_display : MonoBehaviour
{


    public float health,health_max;
    //public float health_max_buffed;


    public Image HealthBar;
    public Text healthText;

    public GameObject target;
    public GameObject respawn_point;

    private Coroutine recharge;

    public GameObject death_system;

    public bool dead;

    public bool is_player;

    public bool is_turret;

    public bool freezeframe_or_movement;
    public bool initiate_freezeframe;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.current.update == true)
        {

            if (is_player == true)
            {
                if (health <= 0)
                {
                    dead = true;
                    death();
                    //put something here to restart the whole scene
                }
            }
            else
            {
                if (health <= 0)
                {
                    if (is_turret == true)
                    {
                        Wavespawner.current.targets.Remove(target);
                    }
                    Destroy(target);
                }
            }
            if (health >= health_max)
            {
                health = health_max;
            }



            /*if (health_max_buffed > health_max)
            {
                healthText.text = "health : " + health + " / " + health_max_buffed;
                HealthBar.fillAmount = health / health_max_buffed;
            }
            else
            {*/
            healthText.text = "health : " + health + " / " + health_max;
            HealthBar.fillAmount = health / health_max;
            //}

            if (Input.GetKey(KeyCode.K))
            {
                health = 0;
            }
        }
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered a fish");
        if (other.CompareTag("fish"))
        {
            health -= 1;
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator OnCollisionEnter(Collision collision)
    {
        Debug.Log("touched a fish");
        if (collision.gameObject.CompareTag("fish"))
        {
            health -= 1;
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator get_rid_of_these_fish()
    {
        foreach (GameObject fish in GameObject.FindGameObjectsWithTag("fish"))
        {
            fish.GetComponent<heat_seeking_fishles>().disable_water = false;
            Debug.Log("destroying 1 fish");
            Wavespawner.current.Remove_alive(fish);
            Destroy(fish);
            yield return new WaitForSeconds(0.000000000000000000000001f);
        }
    }
    //

    public void death()
    {
        death_system.SetActive(true);
        if (freezeframe_or_movement == false)
        {
            initiate_freezeframe = true;
        }
        

    }
}
