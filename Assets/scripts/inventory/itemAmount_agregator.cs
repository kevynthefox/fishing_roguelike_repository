using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class itemAmount_agregator : MonoBehaviour
{
    [Header("text_stacks")]
    public TextMeshProUGUI itemAmount_s;
    public TextMeshProUGUI buffAmount_s;
    public TextMeshProUGUI potionAmount_s;
    public TextMeshProUGUI equipmentAmount_s;
    public TextMeshProUGUI heirloomAmount_s;
    public TextMeshProUGUI total_amount_s;
    [Header("text_totals")]
    public TextMeshProUGUI itemAmount_t;
    public TextMeshProUGUI buffAmount_t;
    public TextMeshProUGUI potionAmount_t;
    public TextMeshProUGUI equipmentAmount_t;
    public TextMeshProUGUI heirloomAmount_t;
    public TextMeshProUGUI total_amount_t;
    [Header("bars")]
    public GameObject itemBar;
    public GameObject buffBar;
    public GameObject potionBar;
    public GameObject equipmentBar;
    public GameObject heirloomBar;

    

    void Update()
    {
       if (TimeManager.current.essential_starter == true)
       {
            //stacks
            itemAmount_s.text = "item stacks: " + itemBar.transform.childCount;
            buffAmount_s.text = "buff stacks: " + buffBar.transform.childCount;
            potionAmount_s.text = "buff stacks in potion: " + potionBar.transform.childCount;
            equipmentAmount_s.text = "equipment stacks: " + equipmentBar.transform.childCount;
            heirloomAmount_s.text = "heirloom stacks: " + heirloomBar.transform.childCount;
            total_amount_s.text = "total stacks: " + (itemBar.transform.childCount+buffBar.transform.childCount+potionBar.transform.childCount+equipmentBar.transform.childCount+ heirloomBar.transform.childCount);
            //totals(as in, the amount of items in there)
            itemAmount_t.text = "items: " + InventorySystem.current.item_count;
            buffAmount_t.text = "buffs: " + InventorySystem.current.buff_count;
            potionAmount_t.text = "buffs in potion: " + InventorySystem.current.buffs_in_potion_count;
            equipmentAmount_t.text = "equipments: " + InventorySystem.current.equipment_count;
            heirloomAmount_t.text = "heirlooms: " + InventorySystem.current.heirloom_count;
            total_amount_t.text = "total: " + InventorySystem.current.total_count;
       }
    }
}
