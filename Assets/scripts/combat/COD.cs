using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COD : MonoBehaviour
{

    public float size;

    public Vector3 scale;

    public bool growing_complete;

    public GameObject bobber;

    private bool starter = true;

    void Start()
    {
        scale = new Vector3(size, size * 0.3663297f, size * 0.2306069f);
        GetComponent<Transform>().localScale += scale;

        bobber = GameObject.Find("bobber (1)");

        StartCoroutine(counter());
    }


    void Update()
    {
        
        
        if (growing_complete == true)
        {
            scale = new Vector3(size, size * 0.3663297f, size * 0.2306069f);
            GetComponent<Transform>().localScale = scale;
            growing_complete = false;
        }

        

    }


    public IEnumerator counter()
    {
        var feesh = new HashSet<GameObject>();
        while (starter == true)
        {
            
            foreach (var fish in GameObject.FindGameObjectsWithTag("fish"))
            {
                if (feesh.Contains(fish))
                {

                }
                else
                {
                    Debug.Log(fish.name);
                    feesh.Add(fish);
                    size += fish.gameObject.GetComponent<fish_variable_holder>().fish_quality;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
