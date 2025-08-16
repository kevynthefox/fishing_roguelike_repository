using Unity.Cinemachine;
using UnityEngine;

public class camera_holder : MonoBehaviour
{
    public static camera_holder current;

    public Camera first_person;
    public Camera third_person;
    public Camera main;
    public CinemachineCamera CinemachineCamera;

    public void Awake()
    {
        main = Camera.main;
    }
}
