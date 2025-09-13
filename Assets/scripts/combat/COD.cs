using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COD : MonoBehaviour
{

    public float size;

    public Vector3 scale;

    public bool day_night;

    public GameObject sun;

    public GameObject home;

    //private bool starter = true;

    public float speed;

    public GameObject wavespawner;

    public bool collided_with_wall;

    private GameObject player;

    void Start()
    {
        scale = new Vector3(size, size * 0.3663297f, size * 0.2306069f);
        GetComponent<Transform>().localScale += scale;

        StartCoroutine(counter());
        StartCoroutine(counter_stopper());
        player = GameObject.Find("player");
    }


    void Update()
    {
        if (Starter.current.update == true)
        {
            day_night = sun.GetComponent<day_cycle>().day_night;


            if (day_night == true)
            {
                //size control section
                scale = new Vector3(size, size * 0.3663297f, size * 0.2306069f);
                GetComponent<Transform>().localScale = scale;
                //movement control section
                if (collided_with_wall == false)
                {
                    speed = Vector3.Distance(home.transform.position, transform.position);

                }
                else
                {
                    //speed = 0;
                }

                this.transform.position = Vector3.MoveTowards(transform.position, home.transform.position, speed * Time.deltaTime);

                this.GetComponent<MeshRenderer>().enabled = true;
                this.GetComponent<CapsuleCollider>().enabled = true;
                this.GetComponent<Rigidbody>().isKinematic = false;

            }

            //escape section
            if (size <= 1)
            {
                transform.position = new Vector3(player.transform.position.x + 2000, player.transform.position.y + 10000, player.transform.position.z + 2000);
                this.GetComponent<MeshRenderer>().enabled = false;
                this.GetComponent<CapsuleCollider>().enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = true;

            }

            this.GetComponent<fish_variable_holder>().potentcy = size;


        }
    }

    public IEnumerator counter_stopper()
    {
        while (Starter.current.starter == true)
        {
            if (day_night == true)
            {
                StopCoroutine(counter());
                wavespawner.GetComponent<Wavespawner>().Add_alive(this.gameObject);
                this.gameObject.tag = "fish";
            }
            else
            {
                StartCoroutine(counter());
                wavespawner.GetComponent<Wavespawner>().Remove_alive(this.gameObject);
                this.gameObject.tag = "Untagged";
            }
            yield return new WaitForSeconds(1);
        }
    }


    public IEnumerator counter()
    {
        var feesh = new HashSet<GameObject>();
        //while (starter == true)
        //{
            
        foreach (var fish in GameObject.FindGameObjectsWithTag("fish"))
        {
            if (feesh.Contains(fish))
            {

            }
            else
            {
                //Debug.Log(fish.name);
                feesh.Add(fish);
                size += (fish.gameObject.GetComponent<fish_variable_holder>().fish_quality * fish.gameObject.GetComponent<fish_variable_holder>().fish_quantity) / 10;
            }
        }
        yield return new WaitForSeconds(1f);
            
        //}
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "safety_wall")
        {
            speed = -speed * 3;
            collided_with_wall = true;
        }
        yield return new WaitForSeconds(1);
    }

    public IEnumerator OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "safety_wall")
        {
            collided_with_wall = false;
        }
        yield return new WaitForSeconds(1);
    }
}
