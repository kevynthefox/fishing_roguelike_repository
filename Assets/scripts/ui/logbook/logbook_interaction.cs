using UnityEngine;

public class logbook_interaction : MonoBehaviour
{
    public GameObject logbook;

    public bool book_open = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            book_open = !book_open;
        }

        logbook.SetActive(book_open);


        Cursor.visible = book_open;
        if (book_open == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }
}
