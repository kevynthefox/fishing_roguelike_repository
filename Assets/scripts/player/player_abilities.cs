using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_abilities : MonoBehaviour
{
    public List<GameObject> spawnable_objects;

    public GameObject sight_obj;
    public GameObject looked_at_object;
    public GameObject hit_shower;

    public int placing;
    public float interactionRayLength;

    

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
    }

    void InteractRaycast()
    {
        Vector3 playerPosition = sight_obj.transform.position;
        Vector3 forwardDirection = sight_obj.transform.forward;

        Ray interactionRay = new Ray(playerPosition, forwardDirection);
        RaycastHit interactionRayHit;
        

        Vector3 interactionRayEndpoint = forwardDirection * interactionRayLength;
        Debug.DrawRay(playerPosition, interactionRayEndpoint, color:Color.blue);// it has to be draw ray, otherwise it will draw it wrong

        bool hitFound = Physics.Raycast(interactionRay, out interactionRayHit, interactionRayLength, ~0, QueryTriggerInteraction.Ignore);
        if (hitFound)
        {
            looked_at_object = interactionRayHit.transform.gameObject;
            hit_shower.transform.position = interactionRayHit.point;
            Debug.Log(looked_at_object.name);
        }
        else
        {
            Debug.Log("-");
        }
    }
}
