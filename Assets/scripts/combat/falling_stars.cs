using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class falling_stars : MonoBehaviour
{
    public GameObject starfish;

    public bool day_night;

    private float random_x_pos;
    private float random_z_pos;

    public Vector3 spawn_pos;

    public float spawn_y_rotation;

    void Start()
    {
        
    }

    void Update()
    {
        day_night = this.GetComponent<day_cycle>().day_night;

        if (day_night == true)
        {
            spawn();
        }

        
    }

    public void spawn()
    {
        random_x_pos = UnityEngine.Random.Range(-2000, 2000);
        random_z_pos = UnityEngine.Random.Range(-2000, 2000);

        spawn_pos = new Vector3(random_x_pos, 1000, random_z_pos);

        spawn_y_rotation = UnityEngine.Random.Range(0, 360);

        var starfish_object = Instantiate(starfish, spawn_pos, quaternion.identity);

        starfish_object.GetComponent<Transform>().rotation = new quaternion(0, spawn_y_rotation, 0, 0);// = spawn_y_rotation;

        //starfish_object.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 10);
        
        
    }
    public static Quaternion rotation(float x, float y, float z)
    {
        return new Quaternion(x, y, z, 1);
    }

}
