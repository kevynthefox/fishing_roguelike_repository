using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GDK;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class gun : MonoBehaviour
{
    //public bool fire;
    public GameObject projectile;
    public Transform spawn_point;

    public bool manual_fire;
    public bool always_fire_at_targets;

    public List<Transform> targets;

    public float distance;
    public float movementStrength;

    public GameObject barrel;

    public float launch_angle;

    public float distance_correction;

    public bool player_gun;

    public GameObject target;
    public GameObject target_backup;

    public bool starter = true;

    public float fire_rate; //not how many bullets per second, how long it takes for another shot.
    public float fire_timer;

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


        StartCoroutine(fire_tracking());
    }

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.T))
        {
            StartCoroutine(fire());
        }

        if (manual_fire == true)
        {

            StartCoroutine(fire());

            manual_fire = false;
        }

        if (always_fire_at_targets == true)
        {
            if (targets.Count > 0)
            {
                
                StartCoroutine(fire());
                
            }
            if (targets.Count > 1 && fire_timer >= fire_rate / 2) //if gun has other targets but isn't shooting, get rid of the clog and try again.
            {
                Debug.Log("gun was jammed. beginning clear");
                StopCoroutine(fire());
                //targets = null;
                //targets.RemoveAt(0);
                targets.RemoveAt(0);

                Debug.Log("jam resolved");
            }
            else
            {
                //Debug.Log("set fire time to 0");
                //fire_timer = 0;
            }


        }
        if (targets.Count > 0)
        {
            if (targets[0].transform != null)
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
            }
            

            if (targets[0].TryGetComponent<heat_seeking_fishles>(out heat_seeking_fishles heat_seeking))
            {
                if (heat_seeking.health == 0)
                {
                    Debug.Log("neutralized");
                    targets.RemoveAt(0);
                }
            }
            if (targets[0].gameObject.TryGetComponent<behavior_for_ranged_fish>(out behavior_for_ranged_fish ranged_behavior))
            {
                if (ranged_behavior.GetComponent<Health_display>().health == 0)
                {
                    Debug.Log("neutralized");
                    targets.RemoveAt(0);
                }
            }
            

            if (targets[0].GameObject().IsDestroyed())
            {
                targets.RemoveAt(0);
            }

            


            
            //targets.RemoveAll(g => g == null); 
        }

        if (target == null)
        {
            Instantiate(target_backup, Vector3.zero, Quaternion.identity);
        }
    }

    public IEnumerator fire()
    {




        if (targets[0] != null)
        {
            yield return new WaitForSeconds(fire_rate);

            GameObject projectile = fish_bullet_pool.Get();
            spawn_point.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
            projectile.transform.SetPositionAndRotation(pos, rot);
            //Debug.Log(pos); Debug.Log(rot);
            Debug.Log("pew");

            fire_timer = 0;

            if (targets.Count > 0)
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


    public IEnumerator OnTriggerEnter(Collider other)
    {

        if (player_gun == true)
        {
            if (other.CompareTag("fish") || other.CompareTag("fish_enemy"))
            {
                if (targets.Contains(other.transform) == false)
                {
                    targets.Add(other.transform);
                }
            }
        }
        else
        {
            if (other.CompareTag("player"))
            {
                if (targets.Contains(other.transform) == false)
                {
                    targets.Add(other.transform);
                }
            }
        }
            yield return null;
    }

    public IEnumerator OnTriggerExit(Collider other)
    {
        if (target != null)
        {
            target.SetActive(false);
        }

        if (targets != null)
        {
            if (other.transform == targets[0].transform)
            {
                var targ = Instantiate(target, targets[0].transform);
                targ.transform.parent = targets[0].transform;
            }

            if (targets.Contains(other.transform) == true)
            {
                targets.Remove(other.transform);
            }
        }


        yield return null;
    }


    public IEnumerator OnTriggerStay(Collider other)
    {
        if (target != null)
        {
            target.SetActive(true);
        }

        if (targets.Count > 0)
        {
            if (targets[0].transform != null)
            {
                if (other.transform == targets[0].transform)
                {
                    target.transform.parent = targets[0].transform;
                    target.transform.rotation = targets[0].transform.rotation;
                    target.transform.localPosition = Vector3.zero;
                }
            }
        }
        yield return null;
    }

    public IEnumerator fire_tracking()
    {
        while (starter == true)
        {
            fire_timer += ( fire_rate / 2);
            yield return new WaitForSeconds(fire_rate);
        }
    }
}


