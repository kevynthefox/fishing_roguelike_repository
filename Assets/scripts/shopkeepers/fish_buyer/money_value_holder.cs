using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class money_value_holder : MonoBehaviour
{
    public float value;

    public bool collectible;

    private GameObject money_spawner;

    public GameObject self;

    /*private float pollingTime = 1f;
    private float time;
    private int frameCount;
    */
    public int frameRate;

    public GameObject frame_holder;
    //yes, the money IS spying on you. this is *purely* for performance reasons as if the framerate goes too low because there's too many fish money things, we want to get rid of them by flinging them at you. :3

    private void Start()
    {
        money_spawner = GameObject.Find("money_touch_spawner");

        frame_holder = GameObject.Find("frame_rate");
    }
    private void Update()
    {
        collectible = money_spawner.GetComponent<fish_seller_checkout>().collectible;

        frameRate = frame_holder.GetComponent<frame_display>().frameRate;

        //Debug.Log(frameRate);
        /*
        if (frameRate <= 20)
        {
            money_spawner.GetComponent<fish_seller_checkout>().collectible = true;
            money_spawner.GetComponent<fish_seller_checkout>().starter = false;
            self.GetComponent<heat_seeking_money>().enabled = true;

            //self.GetComponent<BoxCollider>().size = new Vector3(50,50, 50);
        }
        else
        {
            self.GetComponent<heat_seeking_money>().enabled = false;
            money_spawner.GetComponent<fish_seller_checkout>().starter = true;
        }
        */

        //self.GetComponent<heat_seeking_money>().enabled = false;

        //GetComponent<Rigidbody>().isKinematic = !collectible;



    }

    
}
