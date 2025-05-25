using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_rotate : MonoBehaviour
{
    public float x;
    public float y;
    public float sensitivity = -5f;
    private Vector3 rotate;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //camera rotation!
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");

        //prevents player from going upside down
        if (transform.rotation.z >= 90) transform.Rotate(0, 0, 90);
        if (transform.rotation.z <= -90) transform.Rotate(0, 0, -90);

        rotate = new Vector3(x, y * sensitivity, 0);
        transform.eulerAngles = transform.eulerAngles - rotate;

    }
}
