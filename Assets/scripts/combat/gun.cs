using GDK;
using UnityEngine;

public class gun : MonoBehaviour
{
    //public bool fire;
    public GameObject projectile;
    public Transform spawn_point;

    public bool manual_fire;

    [Header("Object Pools")]
    [SerializeField] private ObjectPoolSO fish_bullet_pool;

    public GameObject bullet_pool;

    public Transform player;

    public void Awake()
    {
        player = GameObject.Find("player").transform;

        bullet_pool = GameObject.Find("bullet_pool");

        fish_bullet_pool.parent = bullet_pool.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            fire();
        }

        if (manual_fire == true)
        {
            fire();
            manual_fire = false;
        }
 
    }

    public void fire()
    {
        Debug.Log("pew");
        /*var proj = Instantiate(projectile, spawn_point.transform.position,Quaternion.identity);
        proj.GetComponent<Rigidbody>().AddForce(transform.forward * proj.GetComponent<projectile_controller>().speed,ForceMode.Impulse);*/

        /*GameObject projectile = ObjectPool.SharedInstance.GetPooledObject();
        if (projectile != null)
        {
            projectile.transform.position = spawn_point.transform.position;
            projectile.transform.rotation = spawn_point.transform.rotation;
            projectile.SetActive(true);

            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectile.GetComponent<projectile_controller>().speed, ForceMode.Impulse);
        }*/
        if (player != null)
        {
            transform.LookAt(player);
        }
        GameObject projectile = fish_bullet_pool.Get();
        spawn_point.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
        projectile.transform.SetPositionAndRotation(pos, rot);

        if (projectile.TryGetComponent(out projectile_controller proj))
        {
            proj.Reset_momentum();
            proj.shoot(transform.forward);
        }
    }
}


