using UnityEngine;

public class equipment_system : MonoBehaviour
{
    public static equipment_system current;

    [Header("left hand")]
    public GameObject left_hand_equipment;
    public GameObject left_hand_holder;
    public GameObject left_hand;
    [Header("right hand")]
    public GameObject right_hand_equipment;
    public GameObject right_hand_holder;
    public GameObject right_hand;

    private void Awake()
    {
        current = this;
    }

    //0 hand change = neither, 1 is left, 2 is right, 3 is both.
    public void spawn_equipment(int hand, InventoryItemData item, Vector3 position, Quaternion rotation, int hand_change, Vector3 left_hand_position, Quaternion left_hand_rotation, Vector3 right_hand_position, Quaternion right_hand_rotation)
    {
        //if (left_hand_holder.transform.childCount == 1)
        //{
            if (hand == 1)
            {
                left_hand_equipment = Instantiate(item.prefab, Vector3.zero, Quaternion.identity);
                left_hand_equipment.transform.parent = left_hand_holder.transform;
                left_hand_equipment.transform.localPosition = position;
                left_hand_equipment.transform.localRotation = rotation;
            }
            if (hand_change == 1 || hand_change == 3)
            {
                left_hand.transform.localPosition = left_hand_position;
                left_hand.transform.localRotation = left_hand_rotation;
            }
        //}
        //else
        //{
            
        //}
        //if (right_hand_holder.transform.childCount == 1)
        //{
            if (hand_change == 2 || hand_change == 3)
            {
                right_hand.transform.localPosition = right_hand_position;
                right_hand.transform.localRotation = right_hand_rotation;
            }
            if (hand == 2)
            {
                right_hand_equipment = Instantiate(item.prefab, Vector3.zero, Quaternion.identity);
                right_hand_equipment.transform.parent = right_hand_holder.transform;
                right_hand_equipment.transform.localPosition = position;
                right_hand_equipment.transform.localRotation = rotation;
            }
        //}
        //else
        //{
            
        //}
    }
        

    public void despwan_equipment(int hand)
    {
        if (hand == 1)
        {
            left_hand.transform.localPosition = Vector3.zero;
            Destroy(left_hand_equipment);
        }
        if (hand == 2)
        {
            right_hand.transform.localPosition = Vector3.zero;
            Destroy(left_hand_equipment);
        }
    }

}
