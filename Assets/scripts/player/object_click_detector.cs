using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class object_click_detector : MonoBehaviour
{
    public bool left_clicked;
    public bool right_clicked;

    public bool click_override;
    //public bool broadcast;

    // Update is called once per frame
    void Update()
    {
        /*if (broadcast == true)
        {
            Debug.Log("click_override: " + click_override);
        }*/
        
    }

    public IEnumerator OnMouseOver()
    {
        /*if (broadcast == true)
        {
            Debug.Log("over");
        }*/

        if (click_override == false)
        {

            if (Input.GetMouseButton(0))
            {
                left_clicked = true;
            }
            else
            {
                left_clicked = false;
            }

            if (Input.GetMouseButton(1))
            {
                right_clicked = true;
            }
            else
            {
                right_clicked = false;
            }
            
        }
        else
        {
            left_clicked = false;
            right_clicked = false;
        }
        yield return null;
    }

    public IEnumerator OnMouseExit()
    {
        /*if (broadcast == true)
        {
            Debug.Log("exit");
        }*/
        left_clicked = false;
        right_clicked = false;
        yield return null;
    }
}
