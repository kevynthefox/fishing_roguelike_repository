using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable_item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Transform parentAfterDrag;
    public Transform inventory_bar;

    public Transform current_parent;

    public InventoryItem self_inventory_item;

    public int spot_in_inventory;

    public void Awake()
    {
        inventory_bar = GameObject.Find("inventory_bar").transform; 
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(inventory_bar);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End drag");
        transform.SetParent(parentAfterDrag);
        spot_in_inventory = (parentAfterDrag.GetComponent<InventorySlot>().inventory_slot_position);
        image.raycastTarget = true;
    }

  

    public void relocate()
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
