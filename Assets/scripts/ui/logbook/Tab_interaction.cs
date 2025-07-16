using NUnit.Framework;
using UnityEngine;

public class Tab_interaction : MonoBehaviour
{
    public GameObject tab_contents;

    public GameObject hover_over;

    public GameObject[] other_tab_list;

    public bool content_state = false;

    public void Start()
    {
        tab_contents.SetActive(false);
        hover_over.SetActive(false);
    }


    public void OnMouseEnter()
    {
        if (this.GetComponent<Tab_interaction>().enabled == true)
        {
            //Debug.Log("hovering over");
            hover_over.SetActive(true);
        }
    }
    public void OnMouseExit()
    {
        if (this.GetComponent<Tab_interaction>().enabled == true)
        {
            hover_over.SetActive(false);
        }
    }

    public void OnMouseDown()
    {
        if (this.GetComponent<Tab_interaction>().enabled == true)
        {
            content_state = !content_state;
            //this.gameObject.SetActive(false);
            tab_contents.SetActive(content_state);
            foreach (GameObject tab in other_tab_list)
            {
                Debug.Log("turning other tabs off");
                tab.SetActive(!content_state);
            }
            hover_over.SetActive(false);
        }
    }

    public void logging()
    {
        Debug.Log("clicked on");
    }
}
