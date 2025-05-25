using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class camera_controls : MonoBehaviour
{

    public Camera cam1;
    public Camera cam2;
    public Vector2 look;
   
   
    // Start is called before the first frame update
    void Start()
    {
        cam1.enabled = true;
    	cam2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
	if (Input.GetKeyDown(KeyCode.F))
	 {
		cam1.enabled = !cam1.enabled;
		cam2.enabled = !cam2.enabled;
	 }
	
    }
    

    void OnLook(InputValue value )
    {
        look = value.Get<Vector2>();
    }
}
