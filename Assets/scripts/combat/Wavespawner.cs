using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.Progress;

public class Wavespawner : MonoBehaviour
{
    [Header("waves")]
    public bool spawning_time;
    //public List<GameObject> fish_actual;

    public float spawn_left_right;
    public float spawn_forward_back;
    public int family_size;
    //public List<GameObject> dead_fish;

    public bool starter = true;

    public int time_left;

    public int fish_total;

    public int fish_alive;

    public bool fish_have_been_alive;

    public GameObject water;
    public GameObject sell_guy;

    public int family_max;

    [Header("encounters")]
    public List<enemy_encounter_data> encounters;
    public GameObject rod;

    public List<GameObject> encounter_enemies_alive;

    public GameObject navmesh;

    public void Start()
    {
        StartCoroutine(timer());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            spawning_time = true;
        }
        
        spawn_left_right = UnityEngine.Random.Range(-1000, 1001);
        spawn_forward_back = UnityEngine.Random.Range(30, 2001);
        family_size = UnityEngine.Random.Range(0, family_max);
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
                        Add_alive(fish_object);
                        fish_have_been_alive = true;
                    }

                    Remove_dead(f.data);
                }

                
            }

            foreach (enemy_encounter_data encounter in encounters)
            {
                
                var rod_script = rod.GetComponent<fishing_script>();
                Debug.Log("going through encounters");
                if (encounter.requirement_type == 1)
                {
                    Debug.Log("encounters that equal type 1");
                    if (rod_script.consecutive_wins >= encounter.requirement_amount)
                    {
                        spawn_encounter(encounter);
                        encounters.Remove(encounter);
                    }
                }
                if(encounter.requirement_type == 2)
                {
                    Debug.Log("encounters that equal type 2");
                    if (rod_script.fish_ever >= encounter.requirement_amount)
                    {
                        spawn_encounter(encounter);
                        encounters.Remove(encounter);
                    }
                }
             
            }
        }


        if (alive_fish.Count != 0 || encounter_enemies_alive.Count != 0)
        {

            sell_guy.SetActive(false);
            if (encounter_enemies_alive.Count > 0)
            {
                navmesh.SetActive(true);
            }

            if (fish_have_been_alive == true)
            {
                foreach (GameObject wat in GameObject.FindGameObjectsWithTag("water"))
                {
                    wat.tag = "water_off";
                }
            }
        }
        else
        {
            sell_guy.SetActive(true);
            navmesh.SetActive(false);
            //Debug.Log("no more fish alive");
            
            foreach (GameObject wat in GameObject.FindGameObjectsWithTag("water_off"))
            {
                wat.tag = "water";
                fish_have_been_alive = false;
            }
            
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

    public void spawn_encounter(enemy_encounter_data encounter)
    {
        
        var rod_script = rod.GetComponent<fishing_script>();
        Debug.Log("started encounter spawn");
        foreach (GameObject enemy in encounter.enemies)
        {
            float spawn_rand = UnityEngine.Random.Range(-encounter.spawn_radius, encounter.spawn_radius);

            Vector3 spawn_position = new Vector3(spawn_rand, 0, 400 + spawn_rand);
            var anenemy = Instantiate(enemy, spawn_position, quaternion.identity);
            if (anenemy.TryGetComponent<heat_seeking_fishles>(out heat_seeking_fishles fishle))
            {
                fishle.home = GameObject.Find("player");
            }
            Debug.Log("spawned enemy");
            encounter_enemies_alive.Add(enemy);
        }
    }
    
    #region fish dead list
    public static Wavespawner current;
    public Dictionary<GameObject, fish_dead> m_itemDictionary;
    public List<fish_dead> dead_fish;//{ get; private set; }
    public List<fish_dead> alive_fish;
    private void Awake()
    {
        current = this;
        dead_fish = new List<fish_dead>();
        alive_fish = new List<fish_dead>();
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

    public void Add_dead(GameObject referenceData)
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

    public void Remove_dead(GameObject referenceData)
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

    public void Add_alive(GameObject referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out fish_dead value))
        {
            value.AddToStack();
        }
        else
        {
            fish_dead newItem = new fish_dead(referenceData);
            alive_fish.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
        InventoryChanged();
    }

    public void Remove_alive(GameObject referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out fish_dead value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                alive_fish.Remove(value);
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