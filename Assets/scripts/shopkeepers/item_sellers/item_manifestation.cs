using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class item_manifestation : MonoBehaviour
{
    public GameObject[] items;

    public GameObject[] item_positions;

    /*public GameObject item_pos_1;
    public GameObject item_pos_2;
    public GameObject item_pos_3;
    */
    public string specialty;

    public bool un_make;

    public void Start()
    {
        item_maker();
    }

    public void Update()
    {
        if (un_make == true)
        {
            item_unmaker();
        }
    }

    public void item_maker()
    {
        /*
        int randomIndex_1 = Random.Range(0, items.Length);
        int randomIndex_2 = Random.Range(0, items.Length);
        int randomIndex_3 = Random.Range(0, items.Length);

        Vector3 item_position_1 = new Vector3(item_pos_1.transform.position.x, item_pos_1.transform.position.y, item_pos_1.transform.position.z);
        Vector3 item_position_2 = new Vector3(item_pos_2.transform.position.x, item_pos_2.transform.position.y, item_pos_2.transform.position.z);
        Vector3 item_position_3 = new Vector3(item_pos_3.transform.position.x, item_pos_3.transform.position.y, item_pos_3.transform.position.z);

        var item_1 = Instantiate(items[randomIndex_1], item_position_1, Quaternion.identity);
        var item_2 = Instantiate(items[randomIndex_2], item_position_2, Quaternion.identity);
        var item_3 = Instantiate(items[randomIndex_3], item_position_3, Quaternion.identity);
        */

        if (un_make == false)
        {
            foreach (GameObject l in item_positions)
            {
                //Debug.Log(l);
                int randomIndex = Random.Range(0, items.Length);
                Vector3 item_position = new Vector3(l.transform.position.x, l.transform.position.y, l.transform.position.z);
                var item = Instantiate(items[randomIndex], item_position, Quaternion.identity);
            }
        }
    }

    public void item_unmaker()
    {
        foreach (var i in GameObject.FindGameObjectsWithTag(specialty))
        {
            Destroy(i);
        }
    }    
}
