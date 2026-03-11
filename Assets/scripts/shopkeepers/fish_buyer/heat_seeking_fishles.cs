using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class heat_seeking_fishles : MonoBehaviour
{

    

    [Header("pathfinding")]
    public float flight_duration;
    public float speed;
    public bool enemy;
    public bool ranged_fish;
    public GameObject target;
    public List<GameObject> targets;
    public float distance;
    public Transform player;
    public bool stationary;
    
    public LayerMask whatIsGround, whatIsPlayer;
    
    [Header("Health")]
    public float health;
    public Canvas Canvas;
    public GameObject health_bar;
    
    [Header("states")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    [Header("attacking")]
    public GameObject gun;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    
    [Header("patrolling")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public Vector3 walkPointAnchor;
    public int new_walkPoint_timer;
    //public List<Vector3> walkPointBeen;
    
    [Header("sequencing")]//as in, preparing attacks
    public GameObject wave_spawner;
    public bool disable_water;
    private GameObject water;


    public void Start()
    {
        walkPointAnchor = transform.position;
        patroling();
    }

    private void OnDrawGizmosSelected()
    {float floppy;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(walkPointAnchor, 1);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(walkPointAnchor, new Vector3(walkPointRange,walkPointRange,walkPointRange));
        /*foreach (Vector3 been in walkPointBeen)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(been, 1);
        }*/
        
    }
    void Update()
    {
        if (TimeManager.current.update == true)
        {
            this.GetComponent<Rigidbody>().isKinematic = false;

            if (target != null)
            {



                //makes the object move faster the further away it is from the other one
                //speed = Vector3.Distance(target.transform.position, transform.position);
                distance = Vector3.Distance(transform.position,target.transform.position);

                //moves this object towards the other object, at this speed per second
                if (ranged_fish == true)
                {
                    playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                    playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

                    //come back to this later
                    //if (!playerInSightRange && !playerInAttackRange) patroling();
                    
                    if (playerInSightRange && !playerInAttackRange)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
                            speed * Time.deltaTime);
                        
                    };
                    if (playerInSightRange && playerInAttackRange) AttackPlayer();
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
                        speed * Time.deltaTime);
                }
                //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.position, target.transform.position, 0, 360));
                transform.LookAt(target.transform); // you need to child the object to an empty gameobject so that the object maintains the rotation you want.
                
            }

            if (target != null && enemy == false)
            {
                StartCoroutine(failsafe_counter());

            }

            if (target != null && enemy == true)
            {
                StartCoroutine(failsafe_counter_2());
            }
            
            

            if (enemy == true && target == null)
            {
                foreach (GameObject potential_target in GameObject.FindGameObjectsWithTag("player"))
                {

                    if (targets.Contains(potential_target) == false)
                    {
                        if (ranged_fish == true)
                        {
                            if (playerInSightRange) targets.Add(potential_target);
                        }
                        else
                        {
                            targets.Add(potential_target);
                        }
                    }

                }

                int random_target = UnityEngine.Random.Range(0, targets.Count);

                if (targets.Count >=1) target = targets[random_target];
            }

            if (ranged_fish == true)
            { 
                health_bar.GetComponent<Health_display>().health = health;
            }
            
        }
        else
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            //attack code here
            gun.GetComponent<gun>().manual_fire = true;
            //

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        if (TimeManager.current.update == true)
        {
            alreadyAttacked = false;
        }
    }
    
    public bool _playerInSightRange
    {
        get => playerInSightRange;
        set
        {
            _playerInSightRange = value;
            if (!playerInSightRange && !walkPointSet && ranged_fish) patroling();
        }
    }
    //float floppy;
    public IEnumerator patrol()
    {
        while (playerInSightRange == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, walkPoint,
                (speed/10) * Time.deltaTime);
            

            
            //floppy += Random.Range(-360, 360) * Time.deltaTime;
            
            //transform.rotation = new Quaternion(floppy,floppy,floppy,floppy);
            if (new_walkPoint_timer >= 8)
            {
                yield break;
                
            }
            
            yield return new WaitForSeconds(0.01f*Time.deltaTime);
        }

        
        
        
    }

    public IEnumerator patrol_timer()
    {
        while (playerInSightRange == false)
        {
            new_walkPoint_timer++;

            if (new_walkPoint_timer >= 8)
            {
                new_walkPoint_timer = 0;
                //walkPointBeen.Add(transform.position);
                patroling();
                yield break;
                
            }

            yield return new WaitForSecondsRealtime(1f);
        }
    }
    
    public void patroling()
    {
        if (TimeManager.current.update == true)
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
            {

                StartCoroutine(patrol());
                StartCoroutine(patrol_timer());
            }

            //Vector3 distanceToWalkpoint = transform.position - walkPoint;

            //walkpoint reached
            //if (distanceToWalkpoint.magnitude < 1f)
            //{
                walkPointSet = false;
            //}
        }
    }
    public void SearchWalkPoint()
    {
        if (TimeManager.current.update == true)
        {
            //calculate random point in range
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomx = Random.Range(-walkPointRange, walkPointRange);
            float randomy = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(walkPointAnchor.x + randomx, walkPointAnchor.y + randomy, walkPointAnchor.z + randomZ);

            //if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            //{
                walkPointSet = true;
            //}
        }
    }
    
    #region health
    
    public void OnTriggerEnter(Collider other)
    {
        if (TimeManager.current.update == true)
        {
            if (other.isTrigger == true)
            {
                if (other.gameObject.tag == "fishing_rod" && other.gameObject.GetComponent<fishing_script>().blocking == false && other.gameObject.GetComponent<fishing_script>().attacking == true)
                {
                    TakeDamage(other.gameObject.GetComponent<fishing_script>().damage);
                }

                if (other.gameObject.tag == "projectile")
                {
                    TakeDamage(((int)other.gameObject.GetComponent<projectile_controller>().damage));
                }
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (TimeManager.current.update == true)
        {
            health -= damage;
            if (health_bar != null) health_bar.GetComponent<Health_display>().health -= health;

            if (health <= 0)
            {
                Invoke(nameof(DestroyEnemy), .5f);
                Debug.Log("i've been hit!");
            }

            if (health <= 60) this.gameObject.tag = "fish";
        }
    }

    private void DestroyEnemy()
    {
        if (TimeManager.current.update == true)
        {
            target = null;
            enemy = false;
            if (ranged_fish == true)
            {
                //wave_spawner.GetComponent<Wavespawner>().encounter_enemies_alive.Remove(this.gameObject);
                this.tag = "super_food_items";
            }
            else
            {
                //wave_spawner.GetComponent<Wavespawner>().Remove_alive(this.gameObject); 
                this.tag = "food_items";
            }

            //Destroy(gameObject); removed because you wanna be able to eat the corpses
            
            
            
            Debug.Log("enemy dead");

        }
    }

    
    #endregion

    #region  failsafe

    

    
    public IEnumerator failsafe_counter()
    {
        
        yield return new WaitForSeconds(1);
        //Debug.Log("failed the safe");
        GetComponent<Rigidbody>().useGravity = true;

    }

    public IEnumerator failsafe_counter_2()
    {
        if (flight_duration != 0)
        {
            yield return new WaitForSeconds(flight_duration);
        }
        else
        {
            StopCoroutine(failsafe_counter_2());
        }
        //Debug.Log("failed the safe");
        GetComponent<Rigidbody>().useGravity = true;

    }
    #endregion
    /*private void OnCollisionEnter(Collision collision)
    {
        if (this.GetComponent<Collider>() != null)
        {
            //Debug.Log("i exist and am touching something");

            if (collision.gameObject.tag == "fishing_rod")
            {
                //Debug.Log("touching the fishing rod. state: " + collision.gameObject.GetComponent<fishing_rod_movement>().blocking);

                if (collision.gameObject.GetComponent<fishing_rod_movement>().blocking == false)
                {
                    //Debug.Log("touched the rod. not blocking");
                    this.tag = "super_food_items";
                    disable_water = false;
                    target = null;
                }
                else
                {
                    //Debug.Log("fling");
                    direction = cam.GetComponent<Transform>().forward;
                    direction_modified = direction * Time.deltaTime * 500;

                    GetComponent<Rigidbody>().AddForce(direction_modified, ForceMode.Impulse);
                }    
            }
            
        }
    }*/
}
