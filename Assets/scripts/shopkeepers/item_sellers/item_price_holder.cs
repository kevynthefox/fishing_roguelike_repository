using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class item_price_holder : MonoBehaviour
{
    public float item_cost;
    public float item_original_cost;
    public float cost_percent;

    private GameObject gamesettings;

    public bool buy_or_pickup;

    public Canvas Canvas;
    public Text cost_text;
    public GameObject image;


    public void Start()
    {
        gamesettings = GameObject.Find("game_settings");

        if (this.gameObject.transform.parent != gamesettings.GetComponent<settings>().player_model.transform)
        {


            Canvas.GetComponent<Canvas>().worldCamera = Camera.main;


            cost_percent = gamesettings.GetComponent<settings>().cost_percent / 100;
            item_cost = cost_percent * item_original_cost;


            /*cost_text.text = item_cost.ToString();
            image.SetActive(true);
            cost_text.gameObject.SetActive(true);*/

            if (buy_or_pickup == false)
            {
                this.gameObject.GetComponent<item_pickup>().enabled = false;
                this.gameObject.GetComponent<item_buying>().enabled = true;

                cost_text.text = item_cost.ToString();
                image.SetActive(true);
                cost_text.gameObject.SetActive(true);
            }

            if (buy_or_pickup == true)
            {
                this.gameObject.GetComponent<item_pickup>().enabled = true;
                this.gameObject.GetComponent<item_buying>().enabled = false;

                cost_text.text = null;
                image.SetActive(false);
                cost_text.gameObject.SetActive(false);
            }
        }
        else
        {
            image.SetActive(false);
            cost_text.gameObject.SetActive(false);
        }

    }
}
