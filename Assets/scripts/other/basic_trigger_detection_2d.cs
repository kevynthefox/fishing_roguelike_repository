using UnityEngine;

public class basic_trigger_detection_2d : MonoBehaviour
{
    public bool triggered;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("triggered");
        triggered = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        triggered = false;
    }
}
