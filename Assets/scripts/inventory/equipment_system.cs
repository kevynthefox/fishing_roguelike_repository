using UnityEngine;

public class equipment_system : MonoBehaviour
{
    public static equipment_system current;

    [Header("left hand")]
    public GameObject left_hand_equipment;
    public GameObject left_hand_holder;
    public GameObject left_hand;
    public Animator left_hand_animator;
    [Header("right hand")]
    public GameObject right_hand_equipment;
    public GameObject right_hand_holder;
    public GameObject right_hand;
    public Animator right_hand_animator;

    private void Awake()
    {
        current = this;
    }

    //0 hand change = neither, 1 is left, 2 is right, 3 is both, 4 is whichever hand it is in.
    public void spawn_equipment(int hand, InventoryItemData item, Vector3 position, Quaternion rotation, Vector3 scale, int hand_change, Vector3 left_hand_position, Quaternion left_hand_rotation, Vector3 right_hand_position, Quaternion right_hand_rotation)
    {
        
        if (hand == 1)
        {
            left_hand_equipment = Instantiate(item.prefab, Vector3.zero, Quaternion.identity);
            left_hand_equipment.transform.localScale = scale;
            left_hand_equipment.transform.parent = left_hand_holder.transform;
            left_hand_equipment.transform.localPosition = position;
            left_hand_equipment.transform.localRotation = rotation;

            if (hand_change == 1 || hand_change == 3)
            {
                left_hand.transform.localPosition = left_hand_position;
                left_hand.transform.localRotation = left_hand_rotation;
            }

            if (item.animator != null)
            {
                left_hand_animator.runtimeAnimatorController = item.animator;
            }
        }

        
        if (hand == 2)
        {
            right_hand_equipment = Instantiate(item.prefab, Vector3.zero, Quaternion.identity);
            right_hand_equipment.transform.localScale = scale;
            right_hand_equipment.transform.parent = right_hand_holder.transform;
            right_hand_equipment.transform.localPosition = position;
            right_hand_equipment.transform.localRotation = rotation;

            if (hand_change == 2 || hand_change == 3)
            {
                right_hand.transform.localPosition = right_hand_position;
                right_hand.transform.localRotation = right_hand_rotation;
            }

            if (item.animator != null)
            {
                right_hand_animator.runtimeAnimatorController = item.animator;
            }
        }

        if (hand_change == 3)
        {
            left_hand.transform.localPosition = left_hand_position;
            left_hand.transform.localRotation = left_hand_rotation;
            right_hand.transform.localPosition = right_hand_position;
            right_hand.transform.localRotation = right_hand_rotation;
        }

    }
        

    public void despwan_equipment(int hand)
    {
        if (hand == 1)
        {
            left_hand_animator.runtimeAnimatorController = null;
            left_hand_holder.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            left_hand_holder.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            
            left_hand.transform.localPosition = Vector3.zero;
            left_hand.transform.localEulerAngles = new Vector3(0, 0, -90);

            left_hand_holder.transform.localPosition = new Vector3(0, 2.9149f, 1.69847f);
            left_hand_holder.transform.localEulerAngles = new Vector3(0, -90, 0);

            Destroy(left_hand_equipment);
        }
        if (hand == 2)
        {
            right_hand_animator.runtimeAnimatorController = null;
            right_hand_holder.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            right_hand_holder.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            

            right_hand.transform.localPosition = Vector3.zero;
            right_hand.transform.localEulerAngles = new Vector3(0, 0, 30);

            right_hand_holder.transform.localPosition = new Vector3(0, 2.9149f, -1.69847f);
            right_hand_holder.transform.localEulerAngles = new Vector3(0, -90, 0);

            Destroy(right_hand_equipment);
        }
    }

}
