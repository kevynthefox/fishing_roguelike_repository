using System.Collections;
using UnityEngine;

public class lamprey_mouth : MonoBehaviour
{
    public bool touching_something;
    public int draining_health;

    public GameObject master;
    public float drain_rate;

    public void OnTriggerEnter(Collider other)
    {
        touching_something = true;
    }

    public void OnTriggerExit(Collider other)
    {
        touching_something = false;
        draining_health = 0;
    }

    public IEnumerator OnTriggerStay(Collider other)
    {
        if (draining_health == 0)
        {
            drain_rate = master.GetComponent<lamprey>().drain_rate;
        }

        if (other.CompareTag("fish"))
        {
            if (other.gameObject.transform.localScale.x > 0) other.gameObject.transform.localScale -= new Vector3(drain_rate, drain_rate, drain_rate);
            other.GetComponent<heat_seeking_fishles>().health -= drain_rate;
            draining_health = 1;
        }
        if (other.CompareTag("fish_enemy"))
        {
            if (other.gameObject.transform.localScale.x > 0) other.gameObject.transform.localScale -= new Vector3(drain_rate, drain_rate, drain_rate);
            other.GetComponent<behavior_for_ranged_fish>().TakeDamage(drain_rate);

            draining_health = 1;
        }

        if (other.CompareTag("super_food_items"))
        {
            if (other.gameObject.transform.localScale.x > 0) other.gameObject.transform.localScale -= new Vector3(drain_rate, drain_rate, drain_rate);
            draining_health = 2;
        }

        if (!other.CompareTag("fish") && !other.CompareTag("fish_enemy") && !other.CompareTag("super_food_items"))
        {
            draining_health = 0;
        }

        if (draining_health == 1)
        {
            master.GetComponent<lamprey>().health_pool += drain_rate;
        }
        if (draining_health == 2)
        {
            master.GetComponent<lamprey>().health_pool += drain_rate * 2;
        }

        if (other.gameObject.transform.localScale.x <= 0)
        {
            Destroy(other.gameObject);
            this.transform.localScale = Vector3.one;
            touching_something = false;
            draining_health = 0;
        }
        yield return new WaitForSeconds(0.1f * Time.deltaTime);
    }
}
