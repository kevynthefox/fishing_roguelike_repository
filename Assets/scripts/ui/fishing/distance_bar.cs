using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class distance_bar : MonoBehaviour
{
    public Scrollbar dist_bar;
    public Text dist_text;

    public GameObject bone;

    public float initial_distance;
    public float current_distance;
    public float percent_distance;

    // Start is called before the first frame update
    void Start()
    {
        dist_text.text = "distance:" + bone.GetComponent<variable_length>().distance;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButton(0)))
        {
            initial_distance = bone.GetComponent<variable_length>().distance;
        }


        current_distance = bone.GetComponent<variable_length>().distance;
        if (current_distance / initial_distance > 0)
        {
            percent_distance = current_distance / initial_distance;

            dist_bar.value = (percent_distance);
            dist_text.text = "distance:" + bone.GetComponent<variable_length>().distance;// + (dist_bar.value / 100) + "%";
        }
    }
}
