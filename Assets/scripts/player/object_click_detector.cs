using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class object_click_detector : MonoBehaviour
{
    public bool left_clicked;
    public bool right_clicked;
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            left_clicked = true;
        }
        else
        {
            left_clicked= false;
        }

        if (Input.GetMouseButton(1))
        {
            right_clicked = true;
        }
        else
        {
            right_clicked = false;
        }
        yield return null;
    }

    public IEnumerator OnMouseExit()
    {
        left_clicked = false;
        right_clicked = false;
        yield return null;
    }
}
