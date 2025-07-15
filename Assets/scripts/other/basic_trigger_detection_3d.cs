using UnityEngine;

public class basic_trigger_detection_3d : MonoBehaviour
{
    public bool triggered;
    public void OnTriggerEnter(Collider other)
    {
        triggered = true;
    }

    public void OnTriggerExit(Collider other)
    {
        triggered = false;
    }
}
