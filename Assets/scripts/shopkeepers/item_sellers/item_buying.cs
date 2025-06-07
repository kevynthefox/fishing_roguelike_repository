using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_buying : MonoBehaviour
{
    public bool left_clicked;

    public GameObject self;
    public GameObject self_item;

    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("player");
    }

    public IEnumerator OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {

            player.GetComponent<item_display>().items.Add(self_item);
            Destroy(self);
        }

        
        yield return null;
    }

}
