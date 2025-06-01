using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish_variable_holder : MonoBehaviour
{
    public float fish_quantity;
    public float fish_quality;
    public float fish_counted;
    public bool duplicate;

    public void Update()
    {
        if (duplicate == true)
        {
            transform.position= Vector3.zero;
        }    
    }

}
