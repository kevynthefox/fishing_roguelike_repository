using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_movement : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    private void Awake()
    {
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
    }

    #region dragging_items[depreceated]
    /*public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);

        transform.position = canvas.transform.TransformPoint(position);
        Debug.Log("drag handler active");
    }*/
    #endregion
}
