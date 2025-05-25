using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_screen : MonoBehaviour
{

    public GameObject player;
    public GameObject respawn_point;
    
    public GameObject hunger;
    public GameObject stamina;
    private float hunger_val;
    private float hunger_val_max;
    
    public void Start()
    {
        hunger = GameObject.Find("HungerBar");
        stamina = GameObject.Find("StaminaBar");
    }

    public void RestartButton()
    {

        player.transform.position = respawn_point.transform.position;
        hunger.GetComponent<Hunger_display>().hunger = hunger.GetComponent<Hunger_display>().hunger_max;
        stamina.GetComponent<Stamina_display>().stamina = stamina.GetComponent<Stamina_display>().stamina_max;
        
    }
    public void exitButton()
    {
    //    SceneManager.LoadScene("MainMenu");
    }
}
