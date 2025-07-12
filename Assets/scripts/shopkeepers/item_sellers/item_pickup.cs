using System.Collections;
using UnityEngine;


public class item_pickup : MonoBehaviour
{
    public bool left_clicked;

    public InventoryItemData self_item;

    public IEnumerator OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {

            InventorySystem.current.Add(this.self_item);
            Destroy(this.gameObject);
        }
        yield return null;
    }
}
