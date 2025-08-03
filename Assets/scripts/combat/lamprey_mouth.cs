using System.Collections;
using UnityEngine;

public class lamprey_mouth : MonoBehaviour
{
    public bool touching_something;
    public bool draining_health;

    public void OnTriggerEnter(Collider other)
    {
        touching_something = true;
    }

    public void OnTriggerExit(Collider other)
    {
        touching_something = false;
        draining_health = false;
    }

    public IEnumerator OnTriggerStay(Collider other)
    {
        if (other.CompareTag("fish"))
        {
            if (other.gameObject.transform.localScale.x > 0) other.gameObject.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            other.GetComponent<heat_seeking_fishles>().health -= 0.1f;
            draining_health = true;
        }
        if (other.CompareTag("fish_enemy"))
        {
            if (other.gameObject.transform.localScale.x > 0) other.gameObject.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            other.GetComponent<behavior_for_ranged_fish>().TakeDamage(0.1f);
            draining_health = false;
        }

        if (other.CompareTag("super_food_items"))
        {
            if (other.gameObject.transform.localScale.x > 0) other.gameObject.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            draining_health = true;
        }

        if (!other.CompareTag("fish") && !other.CompareTag("fish_enemy") && !other.CompareTag("super_food_items"))
        {
            draining_health = false;
        }

        if (other.gameObject.transform.localScale.x <= 0)
        {
            Destroy(other.gameObject);
            this.transform.localScale = Vector3.one;
        }
        yield return new WaitForSeconds(0.1f * Time.deltaTime);
    }
}
