using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class day_cycle : MonoBehaviour
{
    public float time;

    public Vector3 rot = Vector3.zero;
    public float degpersec = 6;
    public float cycle_length; //a deg per sec of 6 means it takes 1 minute to fully rotate

    public bool starter = true;

    public void Start()
    {
        StartCoroutine(time_keeper());
    }

    public void Update()
    {
        degpersec = (1 / cycle_length) * 6 * 60; 

        rot.x = degpersec * Time.deltaTime;
        transform.Rotate(rot,Space.World);

        
        //GetComponent<Transform>().rotation.x += time;
    }

    public static Quaternion rotation(float x, float y, float z)
    {
        return new Quaternion(x, y, z, 1);
    }

    public IEnumerator time_keeper()
    {
        while (starter == true)
        {
            time += 1;
            yield return new WaitForSeconds(1);
        }
    }
}
