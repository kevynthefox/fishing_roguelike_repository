using System;
using System.Collections.Generic;
using NUnit.Framework;
//using Unity.Mathematics;
using UnityEngine;

public class effectCatalouge : MonoBehaviour
{
    public static effectCatalouge current;

    public List<buffed_target> affected;

    public List<GameObject> reference_buffs;


    public Vector3 randomPos;
    public float x_radius;
    public float y_radius;
    public float z_radius;

    public Quaternion randomRotation;


    void trackBuffs()
    {

    }

    public void spawnBuffs(effect buff, buffed_target target)
    {
        randomPos.x = UnityEngine.Random.Range(-x_radius, x_radius);
        randomPos.y = UnityEngine.Random.Range(-y_radius, y_radius);
        randomPos.z = UnityEngine.Random.Range(-z_radius, z_radius);
        randomRotation = new Quaternion(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), 0f);

        var new_buff = Instantiate(reference_buffs[buff.effect_data_object.action_effect], randomPos, randomRotation);
        target.buffs.Add(new_buff);
    }

    void trackPlayerBuffs()
    {

    }

}


[Serializable]
public class buffed_target
{
    public List<effect> applied_effects;// {  get; private set; }
    public GameObject target;
    public List<GameObject> buffs;

    /*public void addBuffs()
    {
        spawnBuffs();
    }*/

    public void removeBuffs(effect buff)
    {

    }
}


public class effect
{
    public effectCatalouge data;// {  get; private set; }
    public int stackSize;// { get; private set; }

    public InventoryItemData effect_data_object;
    
    
    public bool toggleOffOn;
    



    public effect(effectCatalouge source)
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
