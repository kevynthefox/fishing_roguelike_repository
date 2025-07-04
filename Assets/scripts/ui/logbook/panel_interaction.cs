using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class panel_interaction : MonoBehaviour
{
    public GameObject panel_contents;

    public GameObject hover_over;

    public GameObject contents_hover_over;

    public GameObject tab;


    public bool content_state = false;

    public void Start()
    {
        panel_contents.SetActive(false);
        hover_over.SetActive(false);
    }


    public void OnMouseEnter_panel()
    {
        //Debug.Log("hovering over");
        hover_over.SetActive(true);
    }
    public void OnMouseExit_panel()
    {
        hover_over.SetActive(false);
    }

    public void OnMouseEnter_contents()
    {
        //Debug.Log("hovering over");
        contents_hover_over.SetActive(true);
    }
    public void OnMouseExit_contents()
    {
        contents_hover_over.SetActive(false);
    }

    public void OnMouseDown()
    {
        content_state = !content_state;
        //this.gameObject.SetActive(false);
        
        

        hover_over.SetActive(false);
    }

    public void Update()
    {
        panel_contents.SetActive(content_state);
        if (content_state == true)
        {
            tab.transform.SetAsLastSibling();
        }
        else
        {
            tab.transform.SetAsFirstSibling();
        }
    }

    public void logging()
    {
        Debug.Log("clicked on");
    }
}
