using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobber_launch : MonoBehaviour
{
   
    public GameObject bone;
    public GameObject player;

    public float factor;
        public Vector3 direction;
        public Vector3 direction_modified;
    

    public Camera cam;

    //void Update()
    //{
    //    launch();
    //}

    public void Start()
    {
        StartCoroutine(launch());
    }

    public void LateUpdate()
    {
        //factor = bone.GetComponent<variable_length>().distance;
        //if (Input.GetMouseButtonDown(0))
        //{
            //Debug.Log("fling");
            //yield return new WaitForSeconds(0.3f);
            //GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, Mathf.Abs(factor)), ForceMode.Impulse);
            launch();
        StartCoroutine(launch());
        //direction = player.GetComponent<movement>(MovePlayerRelativeToCamera).cameraRelativeMovement;
        //durection = player.getcomponen


        //direction = GetComponent<move_relative_to_camera>().cameraRelativeMovement;
        direction = cam.GetComponent<Transform>().forward;
        direction_modified = direction * Time.deltaTime * factor;
        

        //Vector3 cameraForward = Camera.main.transform.forward;

        //}
    }

    private IEnumerator launch()
    {
        //Debug.Log("fling");
        yield return new WaitForSeconds(0.6f);
        if (factor >= 0)
        {
            GetComponent<Rigidbody>().AddForce(direction_modified, ForceMode.Impulse);
            //GetComponent<Rigidbody>().transform.Translate(direction_modified);
        }
    }
}
