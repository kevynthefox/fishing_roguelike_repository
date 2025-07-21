using UnityEngine;

public class item_price_holder : MonoBehaviour
{
    public float item_cost;
    public float item_original_cost;
    public float cost_percent;

    private GameObject gamesettings;

    private void Start()
    {
        gamesettings = GameObject.Find("game_settings");

        cost_percent = gamesettings.GetComponent<settings>().cost_percent / 100;
        item_cost = cost_percent * item_original_cost;
    }
}
