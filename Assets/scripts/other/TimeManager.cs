using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager current;
    public bool starter = true;
    public bool update = true;

    public bool essential_starter = true; // do not turn this off. this is for things like the inventory.

    public bool starter_reignitable = false;
    public int starters_inactive;

    public bool destroyed;

    public List<MeshFilter> shatter_holders;

    public GameObject player;

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
        
        starter_reignitable = true;

        destroyed = true;

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

    private void Update()
    {
        if (starter == true)
        {
            if (starters_inactive == 0)
            {
                starter_reignitable = false;
            }
        }
        //add something here to pause the physics time. do this when you make the global time stop script

        if (InventoryController.current.inventory_enabled == true || logbook_interaction.current.book_open == true || player.GetComponentInChildren<Health_display>().dead == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (InventoryController.current.inventory_enabled == true)
            {
                player.GetComponent<Rigidbody>().isKinematic = true;
                freeze_most_time();
            }
            if (logbook_interaction.current.book_open == true)
            {
                freeze_all_time();
            }
            if (player.GetComponentInChildren<Health_display>().initiate_freezeframe == true)
            {
                freeze_all_time();
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            un_freeze_all_time();
        }
    }

    public void freeze_all_time()
    {
        Time.timeScale = 0;
        starter = false; starter_reignitable = true;
        update = false;
    }
    public void freeze_most_time()
    {
        Time.timeScale = 1;
        starter = false; starter_reignitable = true;
        update = false;
    }

    public void un_freeze_all_time()
    {
        player.GetComponent<Rigidbody>().isKinematic = false;
        Time.timeScale = 1f;
        starter = true;
        update = true;
    }
}
/*
    
    for turning things with starter back on after starter is turned on again.    

    private bool already_sent_starter_inactive;
    private bool already_sent_starter_active;
    private void Update()
    {
        if (TimeManager.current.starter_reignitable == true)
        {
            if (TimeManager.current.starter == true)
            {
                if (already_sent_starter_active == false)
                {
                    already_sent_starter_inactive = false;
                    TimeManager.current.starters_inactive -= 1;
                    already_sent_starter_active = true;
                    StartCoroutine(UpdateForItemTp());
                }
            }
        }
        if (TimeManager.current.starter == false)
        {
            already_sent_starter_active = false;
            if (already_sent_starter_inactive == false)
            {
                TimeManager.current.starters_inactive += 1;
                already_sent_starter_inactive = true;
            }    
        }
    } 



    this one is for turning updates off/on depending on if update is on or off.
    if (TimeManager.current.update == true)
        {

    }

  */