using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Wavespawner : MonoBehaviour
{
    public bool spawning_time;
    //public List<GameObject> fish_actual;

    public float spawn_left_right;
    public float spawn_forward_back;
    public int family_size;
    //public List<GameObject> dead_fish;

    public bool starter = true;

    public int time_left;

    public int fish_left;

    public void Start()
    {
        StartCoroutine(timer());
    }

    public void Update()
    {
        /*var objects = new HashSet<GameObject>();
        foreach (var f in dead_fish)
        {
            if (f.name == "fish 1")
            {
                //f.GetComponent<fish_variable_holder>().fish_type = fish_actual[0];
            }
        } 
        */
        spawn_left_right = UnityEngine.Random.Range(-1000, 1001);
        spawn_forward_back = UnityEngine.Random.Range(30, 2001);
        family_size = UnityEngine.Random.Range(0, 10);
        if (spawning_time == true)
        {
            if (dead_fish != null)
            {
                foreach (fish_dead f in dead_fish)
                {
                    //fish_left += f.stackSize;
                    for (int i = 0; i < family_size; i++)
                    {
                        var fish_object = Instantiate(f.data, new Vector3(spawn_left_right, 0, spawn_forward_back), Quaternion.identity);
                        fish_object.GetComponent<heat_seeking_fishles>().home = GameObject.Find("player");
                        fish_object.GetComponent<heat_seeking_fishles>().disable_water = true;
                        fish_object.GetComponent<move_relative_to_camera>().Camera = Camera.main;
                    }

                    Remove(f.data);
                }
            }
        }

        foreach (fish_dead f in dead_fish)
        {
            //fish_left += f.stackSize;
        }
    }

    public IEnumerator timer()
    {
        while (starter == true)
        {
            if (time_left >= 1 && spawning_time == false)
            {
                time_left -= 1;
            }
            else
            {

                if (dead_fish.Count == 0)
                {
                    spawning_time = false;
                    time_left = 20;
                }
                else
                {
                    //Debug.Log("not null");
                    spawning_time = true;
                }
            }
            yield return new WaitForSeconds(1f);
        }
        
    }
    
    #region fish dead list
    public static Wavespawner current;
    public Dictionary<GameObject, fish_dead> m_itemDictionary;
    public List<fish_dead> dead_fish;//{ get; private set; }
    private void Awake()
    {
        current = this;
        dead_fish = new List<fish_dead>();
        m_itemDictionary = new Dictionary<GameObject, fish_dead>();

    }

    public event Action onDeadFishChangedEvent;

    public void InventoryChanged()
    {
        if (onDeadFishChangedEvent != null)
        {
            onDeadFishChangedEvent();
        }
    }

    public fish_dead Get(GameObject referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out fish_dead value))
        {
            return value;
        }
        return null;
    }

    public void Add(GameObject referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out fish_dead value))
        {
            value.AddToStack();
        }
        else
        {
            fish_dead newItem = new fish_dead(referenceData);
            dead_fish.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
        InventoryChanged();
    }

    public void Remove(GameObject referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out fish_dead value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                dead_fish.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
    }
    #endregion
}
[Serializable]
public class fish_dead
{
    public GameObject data;// {  get; private set; }
    public int stackSize;// { get; private set; }

    public fish_dead(GameObject source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}