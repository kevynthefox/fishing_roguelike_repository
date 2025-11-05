using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fish_variable_holder : MonoBehaviour
{
    public float fish_quantity;
    public float fish_quality;
    public float fish_counted;

    public int fish_type;

    public float potentcy;



    private void OnDestroy()
    {
        Wavespawner.current.active_fish.Remove(this.gameObject);
    }

}
