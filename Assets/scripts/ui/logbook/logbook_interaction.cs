using UnityEngine;

public class logbook_interaction : MonoBehaviour
{
    public GameObject logbook;

    public bool book_open = false;
    public bool inventory_open;

    public GameObject health;

    private void Update()
    {
        if (health.GetComponent<Health_display>().dead == false)
        {

            if (Input.GetKeyDown(KeyCode.B))
            {
                book_open = !book_open;
            }

            logbook.SetActive(book_open);

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                inventory_open = !inventory_open;
            }

            if (inventory_open == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = book_open;
            }
            if (book_open == false)
            {
                if (inventory_open == false)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                Time.timeScale = 1;
            }
            else
            {
                if (inventory_open == false)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0.1f;
                }
            }
        }
    }
}
