using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class main_menu_script : MonoBehaviour
{
    public void back_to_game()
    {
        SceneManager.LoadScene("game_scene", LoadSceneMode.Single);
    }

    public void exit_game()
    {
        Debug.Log("quitting game");
        Application.Quit(); // note, this does not work inside the unity editor.
    }
}
