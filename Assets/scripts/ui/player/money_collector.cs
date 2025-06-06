using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class money_collector : MonoBehaviour
{
    public Text money;
    public float money_value;

    public bool collectible;

    private GameObject money_spawner;

    

    private void Start()
    {
        money_spawner = GameObject.Find("money_spawner");
    }

    public void Update()
    {
        money.text = ":" + money_value;

        collectible = money_spawner.GetComponent<fish_seller_checkout>().collectible;
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (collectible == true)
        {

            if (other.gameObject.tag == "currency")
            {
                money_value += other.GetComponent<money_value_holder>().value;
                money.text = ":" + money_value;

                Destroy(other.gameObject);

                yield return new WaitForSeconds(1/money_spawner.GetComponent<fish_seller_checkout>().money_owed);
            }

        }
    }
}
