using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attach_to_object : MonoBehaviour
{

public GameObject Object_b;
public Vector3 offset;
public bool attachment;
public bool offset_attachment;
public bool rotate_with_Object_b;
public bool tidally_lock_with_Object_b;
public bool offset_rotation;
public Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	//sets the position of the first object to the position of the second object, plus offset, or not.
        if (attachment == true)
		{
			if (offset_attachment == true)
			{
				transform.position = Object_b.transform.position + offset;
				//transform.rotation = rotation;
			}
			else
			{
				transform.position = Object_b.transform.position;
			}
 	 }
	//rotates the first object with the second object or not
	if (rotate_with_Object_b == true)
	{
		
		if (offset_rotation == true)
		{
                transform.rotation =  rotation;
			
		}
		else
		{
               transform.rotation = Object_b.transform.rotation;
        }
	}
	
    }
}
