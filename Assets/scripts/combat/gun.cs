using System;
using System.Linq;
using GDK;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Reflection;

public class gun : MonoBehaviour
{
    //public bool fire;
    public GameObject projectile;
    public Transform spawn_point;

    public bool manual_fire;

    public List<Transform> targets;

    public float distance;
    public float movementStrength;

    public GameObject barrel;

    public float launch_angle;

    public float distance_correction;

    public bool player_gun;

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
        if (player_gun == true)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("fish").ToList())
            {
                targets.Add(enemy.transform);
            }

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("fish_enemy").ToList())
            {
                targets.Add(enemy.transform);
            }
        }
        else
        {
            targets[0] = GameObject.Find("player").transform;
        }

        bullet_pool = GameObject.Find("bullet_pool");

        fish_bullet_pool.parent = bullet_pool.transform;

        

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            fire();
        }

        if (manual_fire == true)
        {
            
            fire();
            
            manual_fire = false;
        }
        if (targets.Count != 0)
        {
            distance = Vector3.Distance(targets[0].transform.position, this.transform.position) * distance_correction;
            if (types == Type.artillery)
            {
                //x = (vi * cos(0)) *
                //transform.rotation = Quaternion.AngleAxis(player.transform.position.x / (distance/0), Vector3.down);
                Vector3 lookDir = transform.position - targets[0].position;
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

                barrel.transform.rotation = new Quaternion(launch_angle, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            }

            if (targets[0].TryGetComponent<heat_seeking_fishles>(out heat_seeking_fishles heat_seeking))
            {
                if (heat_seeking.health == 0)
                {
                    Debug.Log("neutralized");
                    targets.RemoveAt(0);
                }
            }
            if (targets[0].TryGetComponent<behavior_for_ranged_fish>(out behavior_for_ranged_fish ranged_behavior))
            {
                if (ranged_behavior.GetComponent<Health_display>().health == 0)
                {
                    Debug.Log("neutralized");
                    targets.RemoveAt(0);
                }
            }

            targets.RemoveAll(g => g == null); 
        }
    }

    public void fire()
    {
        if (player_gun == true)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("fish").ToList())
            {
                if (targets.Contains(enemy.transform) == false)
                {
                    targets.Add(enemy.transform);
                }
                
            }

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("fish_enemy").ToList())
            {
                if (targets.Contains(enemy.transform) == false)
                {
                    targets.Add(enemy.transform);
                }
            }
        }

        GameObject projectile = fish_bullet_pool.Get();
        spawn_point.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
        projectile.transform.SetPositionAndRotation(pos, rot);
        //Debug.Log(pos); Debug.Log(rot);
        //Debug.Log("pew");

        if (targets.Count != 0)
        {
            if (types == Type.gun)
            {
                if (targets[0] != null)
                {
                    transform.LookAt(targets[0]);
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

    
}


