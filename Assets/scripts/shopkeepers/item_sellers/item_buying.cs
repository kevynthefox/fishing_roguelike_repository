using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class item_buying : MonoBehaviour
{
    public bool left_clicked;

    public GameObject self;
    public GameObject self_item;

    public GameObject player;
    public GameObject player_model;


    public float item_cost;
    public float item_original_cost;
    public float cost_percent;

    public Canvas Canvas;
    public Text cost_text;

    public GameObject gamesettings;

    public GameObject shop;
    public string group;

    private void Start()
    {
        player = GameObject.Find("player");
        player_model = GameObject.Find("bone_pile");
        shop = GameObject.Find(group);

        Canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void LateUpdate()
    {
        gamesettings = GameObject.Find("game_settings");

        cost_percent = gamesettings.GetComponent<settings>().cost_percent / 100;

        item_cost = cost_percent * item_original_cost;

        cost_text.text = item_cost.ToString();
    }

    public IEnumerator OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            if (player_model.GetComponent<money_collector>().money_value >= item_cost)
            {
                shop.GetComponent<item_manifestation>().money_owed += item_cost;

                shop.GetComponent<item_manifestation>().items_owed.Add(self_item);
                Destroy(self);
            }
        }

        
        yield return null;
    }

}
