using UnityEngine;

public class basic_mouseover_detection : MonoBehaviour
{
    public bool triggered;
    public void OnMouseEnter()
    {
        //Debug.Log("triggered");
        if (this.GetComponent<basic_mouseover_detection>().enabled == true)
        {
            triggered = true;
        }
    }
    public void OnMouseExit()
    {
        if (this.GetComponent<basic_mouseover_detection>().enabled == true)
        {
            triggered = false;
        }
    }
}
