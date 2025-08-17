using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_abilities : MonoBehaviour
{
    public List<GameObject> spawnable_objects;

    public GameObject sight_obj;
    public GameObject looked_at_object;
    public GameObject hit_shower;
    public GameObject hit_shower_pos;

    public int placing;
    public float interactionRayLength;

    public LayerMask layerMask;

    public float speed;

    public bool starter = true;

    void Start()
    {
        sight_obj = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            placing = 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            placing = 2;
            
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            placing = 3;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (placing > 0)
            {
                placing = 0;
            }
        }

        if (placing > 0)
        {
            hit_shower.SetActive(true);
            //hit_shower.transform.rotation = Quaternion.AngleAxis(hit_shower.transform.rotation.y + sight_obj.transform.rotation.y, Vector3.up);

            hit_shower.transform.eulerAngles = new Vector3(0, sight_obj.transform.eulerAngles.y,0 );

            InteractRaycast();
            if (Input.GetMouseButtonDown(0))
            {
                spawn_object(placing);
            }

            sizechange();
        }
        else
        {
            hit_shower.SetActive(false);
            hit_shower.transform.position = sight_obj.transform.position;
        }

        //InteractRaycast();
    }

    private void sizechange()
    {
        if (placing == 1)
        {
            hit_shower.transform.localScale = new Vector3(9, 9, 9);
        }
        if (placing == 2)
        {
            hit_shower.transform.localScale = new Vector3(3, 3, 3);
        }
        if (placing == 3)
        {
            hit_shower.transform.localScale = new Vector3(4, 4, 4);
        }
    }

    void spawn_object(int object_to_spawn)
    {
        var new_obj = Instantiate(spawnable_objects[object_to_spawn], hit_shower.transform.position, Quaternion.identity);
        new_obj.transform.eulerAngles += hit_shower.transform.eulerAngles;
        new_obj.transform.parent = looked_at_object.transform;

        StartCoroutine(spawn_animation(new_obj));
    }

    IEnumerator spawn_animation(GameObject object_to_affect)
    {
        object_to_affect.transform.localScale = Vector3.zero;
        bool finished = false;
        bool finished2 = false;
        int repeat_time = 0;
        while (starter == true && finished2 == false)
        {
            if (object_to_affect.transform.localScale.x < 1 || finished == true || finished2 == true)
            {
                object_to_affect.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                finished = true;
            }

            if (finished == true)
            {
                if (repeat_time < 10)
                {
                    object_to_affect.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                    repeat_time++;
                }
                else
                {
                    object_to_affect.transform.localScale = Vector3.one;
                    finished = false;
                    finished2 = true;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    void InteractRaycast()
    {
        Vector3 playerPosition = sight_obj.transform.position;
        Vector3 forwardDirection = sight_obj.transform.forward;

        Ray interactionRay = new Ray(playerPosition, forwardDirection);
        RaycastHit interactionRayHit;
        

        Vector3 interactionRayEndpoint = forwardDirection * interactionRayLength;
        Debug.DrawRay(playerPosition, interactionRayEndpoint, color:Color.blue);// it has to be draw ray, otherwise it will draw it wrong

        bool hitFound = Physics.Raycast(interactionRay, out interactionRayHit, interactionRayLength, layerMask, QueryTriggerInteraction.Ignore);
        if (hitFound)
        {
            looked_at_object = interactionRayHit.transform.gameObject;
            //hit_shower.transform.position = new Vector3(interactionRayHit.point.x, interactionRayHit.point.y + hit_shower.transform.localScale.y /2, interactionRayHit.point.z); //this part offsets it so that it is on top of the object.
            //hit_shower.transform.position = new Vector3(interactionRayHit.point.x - hit_shower.transform.localScale.x / 2, interactionRayHit.point.y + hit_shower.transform.localScale.y / 2, interactionRayHit.point.z); //this tries to make it not halfway inside the object, but, it only works in one direction, the other direction is fully in the wall
            /*speed = Vector3.Distance(interactionRayHit.point, hit_shower.transform.position);
            hit_shower.transform.position = Vector3.MoveTowards(hit_shower.transform.position, interactionRayHit.point, speed * Time.deltaTime);*/


            
             if (hit_shower.transform.Find("up").GetComponent<basic_trigger_detection_3d>().triggered == true || hit_shower.transform.Find("down").GetComponent<basic_trigger_detection_3d>().triggered == true
                 || hit_shower.transform.Find("right").GetComponent<basic_trigger_detection_3d>().triggered == true || hit_shower.transform.Find("left").GetComponent<basic_trigger_detection_3d>().triggered == true
                 || hit_shower.transform.Find("front").GetComponent<basic_trigger_detection_3d>().triggered == true || hit_shower.transform.Find("back").GetComponent<basic_trigger_detection_3d>().triggered == true)
             {
                 if (hit_shower.transform.Find("up").GetComponent<basic_trigger_detection_3d>().triggered == true)
                 {
                    Debug.Log("pushing down");
                    hit_shower.transform.position = new Vector3(interactionRayHit.point.x, interactionRayHit.point.y + hit_shower.transform.localScale.y / 2, interactionRayHit.point.z);
                 }
                 if (hit_shower.transform.Find("down").GetComponent<basic_trigger_detection_3d>().triggered == true)
                 {
                    Debug.Log("pushing up");
                    hit_shower.transform.position = new Vector3(interactionRayHit.point.x, interactionRayHit.point.y - hit_shower.transform.localScale.y / 2, interactionRayHit.point.z);
                 }
                 if (hit_shower.transform.Find("right").GetComponent<basic_trigger_detection_3d>().triggered == true)
                 {
                    Debug.Log("pushing left");
                    hit_shower.transform.position = new Vector3(interactionRayHit.point.x - hit_shower.transform.localScale.x / 2, interactionRayHit.point.y, interactionRayHit.point.z);
                 }
                 if (hit_shower.transform.Find("left").GetComponent<basic_trigger_detection_3d>().triggered == true)
                 {
                    Debug.Log("pushing right");
                    hit_shower.transform.position = new Vector3(interactionRayHit.point.x + hit_shower.transform.localScale.x / 2, interactionRayHit.point.y, interactionRayHit.point.z);
                 }

                 if (hit_shower.transform.Find("front").GetComponent<basic_trigger_detection_3d>().triggered == true)
                 {
                    Debug.Log("pushing back");
                    hit_shower.transform.position = new Vector3(interactionRayHit.point.x, interactionRayHit.point.y, interactionRayHit.point.z - hit_shower.transform.localScale.z / 2);
                 }
                 if (hit_shower.transform.Find("back").GetComponent<basic_trigger_detection_3d>().triggered == true)
                 {
                    Debug.Log("pushing forward");
                    hit_shower.transform.position = new Vector3(interactionRayHit.point.x, interactionRayHit.point.y, interactionRayHit.point.z + hit_shower.transform.localScale.z / 2);
                 }
             }
             else
             {
                 hit_shower.transform.position = interactionRayHit.point;
             }

            //hit_shower.transform.position = interactionRayHit.point - hit_shower.transform.localScale/2;


            //hit_shower.transform.position = interactionRayHit.point;

            //Debug.Log(looked_at_object.name);
        }
        else
        {
            //Debug.Log("-");
        }
    }
}
