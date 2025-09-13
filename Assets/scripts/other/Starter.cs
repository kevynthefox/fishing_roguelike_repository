using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Starter : MonoBehaviour
{
    public static Starter current;
    public bool starter = true;
    public bool update = true;

    public List<MeshFilter> shatter_holders;

    public void Awake()
    {
        current = this;
        foreach (MeshFilter toEnable in shatter_holders)
        {

            for (int i = 0; i < toEnable.transform.childCount; i++)
            {  
                toEnable.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        starter = false;
        update = false;
        foreach (MeshFilter tobreak in shatter_holders)
        {
            Destroy(tobreak.GetComponent<Animator>());
            Destroy(tobreak);
        }

        foreach (MeshFilter toEnable in shatter_holders)
        {

            for (int i = 0; i < toEnable.transform.childCount; i++)
            {
                toEnable.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
