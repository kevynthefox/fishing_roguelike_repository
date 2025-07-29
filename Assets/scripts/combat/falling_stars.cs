using System;
using System.Collections;
using System.Collections.Generic;
using GDK;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class falling_stars : MonoBehaviour
{
    public GameObject starfish;

    public bool day_night;
    public bool manual_fire;

    private float random_x_pos;
    private float random_z_pos;

    public Vector3 spawn_pos;

    public float spawn_y_rotation;

    public GameObject player;

    public float range;

    public GameObject starfish_pool_object;
    private GameObject sun;

    [SerializeField] private ObjectPoolSO starfish_pool;

    void Awake()
    {
        starfish_pool.parent = starfish_pool_object.transform;
        sun = GameObject.Find("sun");
    }

    void Update()
    {
        day_night = sun.GetComponent<day_cycle>().day_night;

        if ((day_night || manual_fire) == true)
        {
            spawn();
        }

        
    }

    public void spawn()
    {
        random_x_pos = UnityEngine.Random.Range(-range + player.transform.position.x, range + player.transform.position.x);
        random_z_pos = UnityEngine.Random.Range(-range + player.transform.position.z, range + player.transform.position.z);

        spawn_pos = new Vector3(random_x_pos, 1000, random_z_pos);

        spawn_y_rotation = UnityEngine.Random.Range(0, 360);

        //var starfish_object = Instantiate(starfish, spawn_pos, quaternion.identity);

        //starfish_object.GetComponent<Transform>().rotation = new quaternion(0, spawn_y_rotation, 0, 0);// = spawn_y_rotation;

        //starfish_object.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 10);

        GameObject projectile = starfish_pool.Get();
        starfish_pool.defaultSpawnLocation = spawn_pos;
        starfish_pool.defaultSpawnRotation = new quaternion(0, spawn_y_rotation, 0, 0);
        //spawn_pos.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
        projectile.transform.SetPositionAndRotation(spawn_pos, new quaternion(0,spawn_y_rotation,0,0));

        if (projectile.TryGetComponent(out projectile_controller proj))
        {
            proj.Reset_momentum();
            proj.shoot(-transform.up);
        }


    }
    public static Quaternion rotation(float x, float y, float z)
    {
        return new Quaternion(x, y, z, 1);
    }

}
