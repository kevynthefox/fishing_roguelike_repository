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
        if (collision.gameObject.name == "water")
        {
            Destroy(this.gameObject);
        }
    }
}
