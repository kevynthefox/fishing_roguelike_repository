using System;
using System.Collections.Generic;
using NUnit.Framework;
//using Unity.Mathematics;
using UnityEngine;

public class effect_manager : MonoBehaviour
{
    

    public GameObject affected;
    public List<GameObject> effects;
    public List<InventoryItemData> effect_data;
    


    public Vector3 randomPos;
    public float x_radius;
    public float y_radius;
    public float z_radius;

    public Quaternion randomRotation;


    private void Update()
    {
        if (Input.GetKey(KeyCode.End))
        {
            spawnBuffs(3);
        }

        if (transform.childCount > 0)
        {
            if (!Item_behavior.current.effect_manager_queue.Contains(this.gameObject))
            {
                Item_behavior.current.effect_manager_queue.Add(this.gameObject);
            }
        }
        else
        {
            if (Item_behavior.current.effect_manager_queue.Contains(this.gameObject))
            {
                Item_behavior.current.effect_manager_queue.Remove(this.gameObject);
            }
        }
    }

    public void spawnBuffs(int buff)
    {
        randomPos.x = UnityEngine.Random.Range(-x_radius, x_radius);
        randomPos.y = UnityEngine.Random.Range(-y_radius, y_radius);
        randomPos.z = UnityEngine.Random.Range(-z_radius, z_radius);
        randomRotation = new Quaternion(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), 0f);

        var new_buff = Instantiate(effect_catalouge.current.effects[buff], Vector3.zero, Quaternion.identity);
        new_buff.transform.parent = this.transform;
        new_buff.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        new_buff.transform.localPosition = randomPos; new_buff.transform.localRotation = randomRotation;
        effects.Add(new_buff);
        effect_data.Add(new_buff.GetComponent<item_pickup>().self_item);
    }

    public void removeBuffs(int buff_to_remove)
    {
        
        effects.RemoveAt(buff_to_remove);
        effect_data.RemoveAt(buff_to_remove);
        
    }

    public void removeVisualBuffs(string buff_to_remove_name)
    {
        var names = new HashSet<string>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == buff_to_remove_name)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        

        
    }

}


