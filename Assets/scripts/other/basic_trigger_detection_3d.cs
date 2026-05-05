using UnityEngine;

public class basic_trigger_detection_3d : MonoBehaviour
{
    public bool triggered;
    public bool filter_by_layer;
    public int layer;
    
    public void OnTriggerEnter(Collider other)
    {
        if (GetComponent<basic_trigger_detection_3d>().enabled == true)
        {
            if (filter_by_layer == false)
            {
                triggered = true;
            }

            if (filter_by_layer == true)
            {
                if (other.gameObject.layer == layer)
                {
                    triggered = true;
                }
            }
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
