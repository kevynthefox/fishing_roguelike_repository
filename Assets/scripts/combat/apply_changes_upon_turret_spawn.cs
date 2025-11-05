using UnityEngine;

public class apply_changes_upon_turret_spawn : MonoBehaviour
{
    public void Start()
    {
        InventorySystem.current.gameObject.GetComponent<Item_behavior>().apply_changes_that_have_been_made(this.gameObject);
    }
}
