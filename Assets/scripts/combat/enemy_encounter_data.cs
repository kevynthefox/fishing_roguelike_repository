using UnityEngine;


[CreateAssetMenu(menuName = "Enemy Encounter Data")]

public class enemy_encounter_data : ScriptableObject
{
    public GameObject[] enemies;
    public int requirement_type;
    public float requirement_amount;
    public float spawn_radius;
}
