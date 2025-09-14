using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class day_cycle : MonoBehaviour
{
    public float time;

    public Vector3 rot = Vector3.zero;
    public float degpersec = 6;
    public float cycle_length; //a deg per sec of 6 means it takes 1 minute to fully rotate

    //public bool starter = true;

    public bool day_night; //day is false night is true

    public void Start()
    {
        StartCoroutine(time_keeper());
    }

    public void Update()
    {
        if (TimeManager.current.update == true)
        {
            degpersec = (1 / cycle_length) * 6 * 60;

            rot.x = degpersec * Time.deltaTime;
            transform.Rotate(rot, Space.World);


            //GetComponent<Transform>().rotation.x += time;
        }

        if (TimeManager.current.starter_reignitable == true)
        {
            if (TimeManager.current.starter == true)
            {
                if (already_sent_starter_active == false)
                {
                    already_sent_starter_inactive = false;
                    TimeManager.current.starters_inactive -= 1;
                    already_sent_starter_active = true;
                    StartCoroutine(time_keeper());
                }
            }
        }
        if (TimeManager.current.starter == false)
        {
            already_sent_starter_active = false;
            if (already_sent_starter_inactive == false)
            {
                TimeManager.current.starters_inactive += 1;
                already_sent_starter_inactive = true;
            }
        }
    }
    private bool already_sent_starter_inactive;
    private bool already_sent_starter_active;

    public static Quaternion rotation(float x, float y, float z)
    {
        return new Quaternion(x, y, z, 1);
    }

    public IEnumerator time_keeper()
    {
        while (TimeManager.current.starter == true)
        {
            if (time >= cycle_length)
            {
                time = 0;
            }
            else
            {
                time += 1;
            }

            if (time >= 0.5 * cycle_length)
            {
                day_night = true;
            }
            else
            {
                day_night = false;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
