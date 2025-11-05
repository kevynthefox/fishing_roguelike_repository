using UnityEngine;

public class logbook_interaction : MonoBehaviour
{
    public static logbook_interaction current;
    public GameObject logbook;

    public bool book_open = false;
    public bool inventory_open;

    public GameObject health;

    private void Awake()
    {
        current = this;
    }
    private void Update()
    {
        if (health.GetComponent<Health_display>().dead == false)
        {

            if (Input.GetKeyDown(KeyCode.B))
            {
                book_open = !book_open;
            }

            logbook.SetActive(book_open);

        }
    }
}
