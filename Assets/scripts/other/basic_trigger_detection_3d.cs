using UnityEngine;

public class basic_trigger_detection_3d : MonoBehaviour
{
    public bool triggered;
    public void OnTriggerEnter(Collider other)
    {
        if (GetComponent<basic_trigger_detection_3d>().enabled == true)
        {
            triggered = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (GetComponent<basic_trigger_detection_3d>().enabled == true)
        {
            triggered = false;
        }
    }
}
