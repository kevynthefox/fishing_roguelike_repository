using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickaxe_movement : MonoBehaviour
{

	[SerializeField] Animator animator;
	public GameObject Pickaxe;
	
      void Start()
    {
       

    }

    void Update()
    {
	
        if (Input.GetMouseButtonDown(0))
	 {
		animator.SetBool("IsInUse", true);
	 }
	 if (Input.GetMouseButtonUp(0))
	 {
		animator.SetBool("IsInUse", false);
	 }


    }
	
}
