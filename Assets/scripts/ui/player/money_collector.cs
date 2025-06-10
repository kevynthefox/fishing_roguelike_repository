using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class money_collector : MonoBehaviour
{
    public Text money;
    public float money_value;
    public float others_value;

    public float others_value_2;
    public float others_value_divider;

    public GameObject self;

    //public bool collectible;

    private GameObject money_spawner;
    private GameObject money_spawner_island;
    private GameObject bobber;

    public float wait;

    private void Start()
    {
        money_spawner = GameObject.Find("money_touch_spawner");
        money_spawner_island = GameObject.Find("money_island_spawner");
        bobber = GameObject.Find("bobber (1)");
    }

    public void Update()
    {
        money.text = ":" + money_value;
        wait = 1 / bobber.GetComponent<bobber_impact>().fish_quantity_original;
        others_value_2 = money_value / others_value_divider;

        self.GetComponent<Transform>().localScale = new Vector3(1f,1f,1f) + new Vector3(others_value_2 / others_value_divider, others_value_2 / others_value_divider, others_value_2);
        self.GetComponent<Transform>().position = new Vector3(106.85f, 11.942f, -324.53f) + new Vector3(0f, others_value_2, 0f);
        money_spawner_island.GetComponent<Transform>().position = new Vector3(106.85f, 300f, -324.53f) + new Vector3(0, others_value_2 * 2, 0f);
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currency")
        {
            if (other != null)
            {
                others_value = other.GetComponent<money_value_holder>().value;
                money_value += others_value;                
                Destroy(other.gameObject);
            }
            /*self.GetComponent<Transform>().localScale += new Vector3(others_value_2 / others_value_divider, others_value_2 / others_value_divider, others_value_2);
            self.GetComponent<Transform>().position += new Vector3(0f, others_value_2, 0f);
            money_spawner_island.GetComponent<Transform>().position += new Vector3(0, others_value_2 * 2, 0f);*/
            yield return new WaitForSeconds(wait);
        }
    }
}
