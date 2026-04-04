using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.Progress;

public class Wavespawner : MonoBehaviour
{
    [Header("waves")]
    public bool spawning_time;
    public bool spawnable;
    public bool stop_fishing;
    //public List<GameObject> fish_actual;

    //public float spawn_left_right;
    //public float spawn_forward_back;
    public Transform spawn_rad_dist;
    public int family_size;
    //public List<GameObject> dead_fish;

    //public bool starter = true;

    public int time_start;
    public int time_left;

    public int fish_total;

    public int fish_alive;

    public bool fish_have_been_alive;

    //public GameObject water;
    public GameObject sell_guy;

    public float fish_quality;

    public int family_max;

    public float fish_potency_buff_mult;
    public float fish_potency_buff_add;

    public List<GameObject> targets;


    public GameObject[] fishes;

    public List<GameObject> sources_of_fish;
    public List<GameObject> active_fish;

    [Header("encounters")]
    public List<enemy_encounter_data> encounters;
    public GameObject rod;

    public List<GameObject> encounter_enemies_alive;

    //public GameObject navmesh;

    public void Start()
    {
        StartCoroutine(timer());
        StartCoroutine(spawn_prevention());
        targets.Add(GameObject.Find("player"));
        time_left = time_start;
    }
    private bool already_sent_starter_inactive;
    private bool already_sent_starter_active;
    public void Update()
    {
        if (TimeManager.current.starter_reignitable == true)
        {
            if (TimeManager.current.starter == true)
            {
                if (already_sent_starter_active == false)
                {
                    already_sent_starter_inactive = false;
                    TimeManager.current.starters_inactive -= 1;
                    already_sent_starter_active = true;
                    StartCoroutine(timer());
                    StartCoroutine(spawn_prevention());
                }
            }
        }
        if (TimeManager.current.starter == false)
        {
            already_sent_starter_active = false;
            if (already_sent_starter_inactive == false)
            {
                TimeManager.current.starters_inactive += 1;
                already_sent_starter_inactive = true;
            }
        }

        if (TimeManager.current.update == true)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                spawning_time = !spawning_time;
            }

            if (Input.GetKey(KeyCode.RightShift))
            {
                Debug.Log("adding a debug fish to the wavepsawner");
                Add_dead(fishes[2]);
            }

            if (rod.GetComponent<fishing_script>().win_state == 1)
            {
                fish_quality = rod.GetComponent<fishing_script>().fish_quality;
            }

            //spawn_left_right = UnityEngine.Random.Range(-spawning_radius, spawning_radius + 1);
            //spawn_forward_back = UnityEngine.Random.Range(-spawning_radius, spawning_radius + 1);
            family_size = UnityEngine.Random.Range(0, family_max);

            if (rod.GetComponent<fishing_script>().spawning_fish == true)
            {
                //Debug.Log("fish are spawning, get the potency buffs");
                fish_potency_buff_mult = rod.GetComponent<fishing_script>().fish_potency_buff_mult;
                fish_potency_buff_add = rod.GetComponent<fishing_script>().fish_potency_buff_add;
            }



            if (spawning_time == true)
            {
                foreach (GameObject potential_target in GameObject.FindGameObjectsWithTag("player"))
                {

                    if (targets.Contains(potential_target) == false)
                    {
                        targets.Add(potential_target);
                    }

                }

                int random_target = UnityEngine.Random.Range(0, targets.Count);



                foreach (fish_dead f in dead_fish.ToList())
                {
                    //fish_left += f.stackSize;
                    for (int i = 0; i < family_size; i++)
                    {
                        var fish_object = Instantiate(f.data, new Vector3(spawn_rad_dist.transform.position.x, 10, spawn_rad_dist.transform.position.z), Quaternion.identity);
                        fish_object.GetComponent<heat_seeking_fishles>().target = targets[random_target];
                        fish_object.GetComponent<heat_seeking_fishles>().disable_water = true;
                        fish_object.GetComponent<heat_seeking_fishles>().enemy = true;

                        fish_object.GetComponent<fish_variable_holder>().potentcy += f.fish_potency_buff_add;
                        fish_object.GetComponent<fish_variable_holder>().potentcy += fish_potency_buff_add;

                        if (f.fish_potency_buff_mult > 1) fish_object.GetComponent<fish_variable_holder>().potentcy *= f.fish_potency_buff_mult;
                        if (fish_potency_buff_mult > 1) fish_object.GetComponent<fish_variable_holder>().potentcy *= fish_potency_buff_mult;

                        if (fish_quality > 0) fish_object.transform.localScale = new Vector3(fish_quality, fish_quality, fish_quality);
                        Add_alive(fish_object);
                        fish_have_been_alive = true;
                    }

                    Remove_dead(f.data);
                }



                //Debug.Log(encounters_copy_list.Count);
                foreach (enemy_encounter_data encounter in encounters.ToList())
                {

                    var rod_script = rod.GetComponent<fishing_script>();
                    //Debug.Log("going through encounters");
                    if (encounter.requirement_type == 1)
                    {
                        //Debug.Log("encounters that equal type 1");
                        if (rod_script.consecutive_wins >= encounter.requirement_amount)
                        {
                            spawn_encounter(encounter);
                            encounters.Remove(encounter);
                        }
                    }
                    if (encounter.requirement_type == 2)
                    {
                        //Debug.Log("encounters that equal type 2");
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
                    //navmesh.SetActive(true);
                }

                if (fish_have_been_alive == true || encounter_enemies_alive.Count != 0)
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
                //navmesh.SetActive(false);
                //Debug.Log("no more fish alive");

                if (fish_have_been_alive == true)
                {
                    fish_potency_buff_mult = 1;
                    fish_potency_buff_add = 0;
                }
                foreach (GameObject wat in GameObject.FindGameObjectsWithTag("water_off"))
                {
                    wat.tag = "water";
                    fish_have_been_alive = false;
                }

            }



        }
    }

    public IEnumerator timer()
    {
        while (TimeManager.current.starter == true)
        {

            if (time_left >= 1 && spawning_time == false)
            {
                time_left -= 1;
            }
            else
            {
                if (spawnable == true)
                {
                    if (dead_fish.Count == 0)
                    {
                        spawning_time = false;
                        time_left = time_start;
                    }
                    else
                    {
                        //Debug.Log("not null");
                        spawning_time = true;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
        
    }

    public void spawn_encounter(enemy_encounter_data encounter)
    {
        
        var rod_script = rod.GetComponent<fishing_script>();
        //Debug.Log("started encounter spawn");
        foreach (GameObject enemy in encounter.enemies)
        {
            float spawn_rand = UnityEngine.Random.Range(-encounter.spawn_radius, encounter.spawn_radius);

            Vector3 spawn_position = new Vector3(spawn_rand, 20+Mathf.Abs(spawn_rand), 400 + spawn_rand);
            var anenemy = Instantiate(enemy, spawn_position, quaternion.identity);
            if (anenemy.TryGetComponent<heat_seeking_fishles>(out heat_seeking_fishles fishle))
            {
                fishle.target = GameObject.Find("player");
            }

            //was gonna do something here for the fish enemies(like mobster lobsters) but those shouldn't be affected by potency buffs because they're not a consequence of your fishing as directly
            //as regular fish.
            //Debug.Log("spawned enemy");
            encounter_enemies_alive.Add(enemy);
        }
    }
    
    public IEnumerator spawn_prevention()
    {
        while (TimeManager.current.starter == true)
        {
            if (spawning_time == false) 
            {
                foreach (GameObject fish in GameObject.FindGameObjectsWithTag("fish"))
                {
                    if (!active_fish.Contains(fish))
                    {
                        //if (fish.name != "COD")
                        //{
                        active_fish.Add(fish);
                        //}
                    }
                }
            }

            if (sources_of_fish.Count > 0)
            {
                spawnable = false;
            }

            if (time_left <= 0)
            {
                stop_fishing = true;
            }
            else
            {
                stop_fishing = false;
            }

            if (sources_of_fish.Count <= 0)
            {
                if (active_fish.Count <= 1)
                {
                    //Debug.Log("could not find any fish, wait 1 second and set spawnable to true");
                    yield return new WaitForSeconds(1f);
                    spawnable = true;
                }

                
            }
            yield return new WaitForSeconds(0.1f);
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

    public float fish_potency_buff_mult;
    public float fish_potency_buff_add;
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