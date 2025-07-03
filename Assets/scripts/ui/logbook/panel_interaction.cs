using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class panel_interaction : MonoBehaviour
{
    public GameObject panel_contents;

    public GameObject hover_over;

    public bool content_state = false;

    public void Start()
    {
        panel_contents.SetActive(false);
        hover_over.SetActive(false);
    }


    public void OnMouseEnter()
    {
        //Debug.Log("hovering over");
        hover_over.SetActive(true);
    }
    public void OnMouseExit()
    {
        hover_over.SetActive(false);
    }

    public void OnMouseDown()
    {
        content_state = !content_state;
        //this.gameObject.SetActive(false);
        panel_contents.SetActive(content_state);
        hover_over.SetActive(false);
    }

    public void logging()
    {
        Debug.Log("clicked on");
    }
}
