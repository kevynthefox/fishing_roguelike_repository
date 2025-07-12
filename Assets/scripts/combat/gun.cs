using System;
using GDK;
using UnityEngine;
using UnityEngine.UI;

public class gun : MonoBehaviour
{
    //public bool fire;
    public GameObject projectile;
    public Transform spawn_point;

    public bool manual_fire;

    public Transform player;

    public float distance;
    public float movementStrength;

    public GameObject barrel;

    public float launch_angle;

    public float distance_correction;

    [Header("Object Pools")]
    [SerializeField] private ObjectPoolSO fish_bullet_pool;

    public GameObject bullet_pool;

    

    [Header("Gun Types")]
    //public bool artillery;
    [SerializeField]
    Type types = new Type();
    public enum Type
    {
        gun,
        artillery
    }

    

    public void Awake()
    {
        player = GameObject.Find("player").transform;

        bullet_pool = GameObject.Find("bullet_pool");

        fish_bullet_pool.parent = bullet_pool.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            fire();
        }

        if (manual_fire == true)
        {
            
            fire();
            
            manual_fire = false;
        }

        distance = Vector3.Distance(player.transform.position, this.transform.position) * distance_correction;
        if (types == Type.artillery)
        {
            //x = (vi * cos(0)) *
            //transform.rotation = Quaternion.AngleAxis(player.transform.position.x / (distance/0), Vector3.down);
            Vector3 lookDir = transform.position - player.position;
            float radians = Mathf.Atan2(lookDir.x, lookDir.z);
            float degrees = radians * Mathf.Rad2Deg;

            float str = Mathf.Min(movementStrength * Time.deltaTime, 1);
            Quaternion targetRotation = Quaternion.Euler(0, degrees, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, str);


            launch_angle =

                0.5f * MathF.Asin(
                    (9.81f * distance)
                    / (projectile.GetComponent<projectile_controller>().s_to_m * projectile.GetComponent<projectile_controller>().s_to_m)
                );
            
            barrel.transform.rotation = new Quaternion(launch_angle,transform.rotation.y,transform.rotation.z,transform.rotation.w);
        }
    }

    public void fire()
    {
        GameObject projectile = fish_bullet_pool.Get();
        spawn_point.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
        projectile.transform.SetPositionAndRotation(pos, rot);
        Debug.Log(pos); Debug.Log(rot);
        Debug.Log("pew");
        if (types == Type.gun)
        {
            if (player != null)
            {
                transform.LookAt(player);
            }
            if (projectile.TryGetComponent(out projectile_controller proj))
            {
                proj.Reset_momentum();
                proj.shoot(transform.forward);
            }
        }
        if (types == Type.artillery)
        {
            if (projectile.TryGetComponent(out projectile_controller proj))
            {
                proj.Reset_momentum();
                proj.shoot(spawn_point.transform.forward);
            }
        }    
        

        
    }

    
}


