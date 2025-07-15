using UnityEngine;

public class basic_mouseover_detection : MonoBehaviour
{
    public bool triggered;
    public void OnMouseEnter()
    {
        //Debug.Log("triggered");
        triggered = true;
    }
    public void OnMouseExit()
    {
        triggered = false;
    }
}
