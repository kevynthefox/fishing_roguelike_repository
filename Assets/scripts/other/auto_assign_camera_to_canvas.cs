using Unity.Cinemachine;
using UnityEngine;

public class auto_assign_camera_to_canvas : MonoBehaviour
{


    private void Awake()
    {

        this.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
