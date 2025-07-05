using System.Collections;
using UnityEngine;

public class projectile_controller : MonoBehaviour
{
    public float damage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "water" || collision.gameObject.tag == "water_off" || collision.gameObject.name == "ground")
        {
            Destroy(this.gameObject);
        }
    }
}
