using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    [SerializeField]
    private GameObject _target;
    public List<GameObject> targets;
    public float distance;
    public Vector3 direction_to_target;
    public Vector3 current_direction;
    public Transform player;
    public bool stationary;
    
    public LayerMask whatIsGround, whatIsPlayer;
    
    [Header("Health")]
    public float health;
    public Canvas Canvas;
    public GameObject health_bar;
    
    [Header("states")]
    public float sightRange, attackRange;
    public bool playerInAttackRange;
    [SerializeField]
    private bool _playerInSightRange;
    
    [Header("attacking")]
    public GameObject gun;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    
    [Header("patrolling")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public Vector3 walkPointAnchor;
    public int new_walkPoint_timer,walkPoint_timer_limit;
    public Vector3 floppy;
    public float floppy_difference_magnitude;
    public float walkPoint_Counter;
    public Vector3 distanceToWalkpoint,distanceToWalkpoint_initial;
    public float distanceToWalkpoint_magnitude,distanceToWalkpoint_magnitude_initial;
    public float sped;
    public GameObject patrol_point;
    //public Rigidbody rb;
    //public List<Vector3> walkPointBeen;
    
    [Header("sequencing")]//as in, preparing attacks
    public GameObject wave_spawner;
    public bool disable_water;
    private GameObject water;


    public void Start()
    {
        walkPointAnchor = transform.position;
        if (ranged_fish == true) patroling();
        if (walkPoint_timer_limit == 0)
        {
            walkPoint_timer_limit = 99999;
        }

        if (enemy == false)
        {
            StartCoroutine(failsafe_counter_2()); //this one turns off the boid pathfinding and goes back to simpler pathfinding. this is for fished up fish as they dont need as advanced pathfinding
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(walkPointAnchor, 1);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(walkPointAnchor, new Vector3(walkPointRange,walkPointRange,walkPointRange));
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(walkPoint,1);

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

            

            if (ranged_fish == true)
            {
                //Debug.Log("checking spheres");
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
                //Debug.Log("spheres checked");
            }

            if (target != null)
            {
                distance = Vector3.Distance(transform.position, target.transform.position);


                

                //moves this object towards the other object, at this speed per second
                if (ranged_fish == true)
                {
                    if (TryGetComponent<Boid>(out Boid boid_))
                    {
                        if (playerInSightRange == false)
                        {
                            boid_.dead = true;
                        }

                        if (playerInSightRange && !playerInAttackRange)
                        {
                            boid_.dead = false;

                        }

                        if (playerInSightRange && playerInAttackRange)
                        {
                            boid_.dead = true; //turns off the pathfinding
                            this.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; //this makes it so they don't bounce off of something and start floating away
                            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                            AttackPlayer();
                        }
                    }
                }


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
            //this.GetComponent<Rigidbody>().isKinematic = true;
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

    public GameObject target
    {
        get { return _target; }
        set
        {
            _target = value;

            if (TryGetComponent<Boid>(out Boid boid_))
            {
                if (target == null)
                {
                    boid_.target = null;
                }
                else
                {
                    
                    boid_.target = _target.transform;
                }
            }
        }
    }

    #region patrolling

    

    //[SerializeField]
    public bool playerInSightRange
    {
        
        get
        {
            //Debug.Log("getting playerinsightrange");
            return _playerInSightRange;
        }
        set
        {
            //Debug.Log("triggering stuff when playerinsightrange changes");
            _playerInSightRange = value;
            
            if (_playerInSightRange == true)
            {
                //Debug.Log("stopping patrolling");
                StopCoroutine(patrol());
                StopCoroutine(patrol_timer());
                walkPointSet = false;
                if (targets[0].CompareTag("patrol") == true)
                {
                    Destroy(targets[0]);
                    targets.RemoveAt(0);
                    target = null;
                }
                walkPoint = Vector3.zero;
                new_walkPoint_timer = -1;
            }
            else
            {
                if (targets[0].CompareTag("patrol") == false)
                {
                    targets.Remove(target);
                    target = null;  
                }
                //Debug.Log("target removed");
                
                if (new_walkPoint_timer == -1) patroling();
            }
            
            //if (!_playerInSightRange && !walkPointSet && ranged_fish) patroling();
            

           
            
        }
    }

    
    public IEnumerator patrol()
    {
        //Debug.Log("patrol_moving");
        //rb.linearVelocity = distanceToWalkpoint_initial;
        
        Quaternion floppy_rotation = new Quaternion(floppy.x, floppy.y, floppy.z, 0);
        //Vector3 flop = floppy - transform.rotation.eulerAngles;
        
        
        /*Vector3 flop = floppy - transform.rotation.eulerAngles;
        rb.linearVelocity = distanceToWalkpoint;//(distanceToWalkpoint_initial.x,distanceToWalkpoint_initial.y,distanceToWalkpoint_initial.z);
        rb.angularVelocity = flop;
        */
        
        while (playerInSightRange == false)
        {
            distanceToWalkpoint = walkPoint - transform.position;
            distanceToWalkpoint_magnitude = distanceToWalkpoint.magnitude;
            
            floppy_difference_magnitude = (floppy_rotation.eulerAngles - transform.rotation.eulerAngles).magnitude;
            
            walkPoint_Counter  += 0.01f;
            //sped =  (walkPoint_Counter/10) * ((distanceToWalkpoint_magnitude_initial * Time.deltaTime)/(walkPoint_timer_limit * speed));
            sped = ((1/((distanceToWalkpoint_magnitude+0.00001f) / distanceToWalkpoint_magnitude_initial))/walkPoint_timer_limit) * 0.01f;
            
            //Vector3 flop = floppy - transform.rotation.eulerAngles;
            //rb.linearVelocity = distanceToWalkpoint;//(distanceToWalkpoint_initial.x,distanceToWalkpoint_initial.y,distanceToWalkpoint_initial.z);
            //rb.angularVelocity = flop;
            //Debug.Log("linear velocity: " + rb.linearVelocity);
            //Debug.Log("distanceToWalkpoint_initial: " + distanceToWalkpoint_initial);
            //rb.velocity.Set(distanceToWalkpoint.x,distanceToWalkpoint.y,distanceToWalkpoint.z);
            
            transform.position = Vector3.Slerp(transform.position, walkPoint, sped);
            transform.rotation = Quaternion.Slerp(transform.rotation,floppy_rotation,sped/(walkPoint_timer_limit * floppy_difference_magnitude));// = Quaternion.Slerp(transform.rotation,floppy_rotation, str);
            
            //transform.rotation = new Quaternion(floppy.x * Time.deltaTime,floppy.y * Time.deltaTime,floppy.z * Time.deltaTime,0);

            if (new_walkPoint_timer >= walkPoint_timer_limit)
            {
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
            }

        }

        //yield return null;
        
        
    }

    public IEnumerator patrol_timer()
    {
        //Debug.Log("patrol_timing");
        while (playerInSightRange == false)
        {
            new_walkPoint_timer++;

            if (new_walkPoint_timer >= walkPoint_timer_limit)
            {
                walkPoint_Counter = 0;
                new_walkPoint_timer = 0;
                //walkPointBeen.Add(transform.position);
                walkPointSet = false;
                if (targets[0].CompareTag("patrol") == true)
                {
                    Destroy(targets[0]);
                    targets.RemoveAt(0);
                    target = null;
                }
                StopCoroutine(patrol());
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
        
            //Debug.Log("patroling");
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
            {

                StartCoroutine(patrol());
                StartCoroutine(patrol_timer());
            }

            distanceToWalkpoint = walkPoint - transform.position;
            distanceToWalkpoint_initial = distanceToWalkpoint;
            distanceToWalkpoint_magnitude = distanceToWalkpoint.magnitude;
            distanceToWalkpoint_magnitude_initial = distanceToWalkpoint_magnitude;

            //walkpoint reached
            //if (distanceToWalkpoint.magnitude < 1f)
            //{
                //walkPointSet = false;
                //SearchWalkPoint();
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
            floppy.x = Random.Range(0, 360);
            floppy.y = Random.Range(0, 360);
            floppy.z = Random.Range(0, 360);
            walkPoint = new Vector3(walkPointAnchor.x + randomx, walkPointAnchor.y + randomy, walkPointAnchor.z + randomZ);

            var point = Instantiate(patrol_point, walkPoint, quaternion.identity);
            point.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            targets.Add(point);
            //if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            //{
            walkPointSet = true;
            //}
        
        }
    }
    #endregion
    
    #region health
    
    public void OnTriggerEnter(Collider other)
    {
        if (TimeManager.current.update == true)
        {
            if (other.isTrigger == true)
            {
                

                if (other.TryGetComponent<fishing_script>(out fishing_script fishing_))
                {
                    if (other.CompareTag("fishing_rod") && fishing_.blocking == false && fishing_.attacking == true)
                    {
                        TakeDamage(fishing_.damage);
                    }

                    
                }
                if (other.CompareTag("projectile"))
                {
                    TakeDamage(other.gameObject.GetComponent<projectile_controller>().damage);
                }
                /*if (other.gameObject.tag == "fishing_rod" && other.gameObject.GetComponent<fishing_script>().blocking == false && other.gameObject.GetComponent<fishing_script>().attacking == true)
                {
                    TakeDamage(other.gameObject.GetComponent<fishing_script>().damage);
                }
                if (other.gameObject.tag == "projectile")
                {
                    TakeDamage(((int)other.gameObject.GetComponent<projectile_controller>().damage));
                }*/
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (TimeManager.current.update == true)
        {
            health -= damage;
            if (health_bar != null) health_bar.GetComponent<Health_display>().health -= health;

            if (health <= 60 && health >= 1) this.gameObject.tag = "fish";
            if (health <= 0)
            {
                DestroyEnemy();
                Debug.Log("i've been killed!");
            }

            
        }
    }

    public void DestroyEnemy()
    {
    
        enemy = false;
        targets.Clear();
        target = null;
        if (TryGetComponent<Boid>(out Boid boid))
        {
            boid.dead = true;
        }

        this.GetComponent<Rigidbody>().useGravity = true;
        
        //this.GetComponent<GameObject>().tag = "food_items";
        
        if (ranged_fish == true)
        {
            
            Wavespawner.current.encounter_enemies_alive.Remove(this.gameObject);
            tag = "super_food_items";
        }
        else
        {
            Wavespawner.current.Remove_alive(this.gameObject);
            tag = "food_items";
            //this.gameObject.tag = "food_items";
        }

        //Destroy(gameObject); removed because you wanna be able to eat the corpses
        
        
        
        Debug.Log("enemy dead");

    
    }

    private void OnDestroy()
    {
        if (TryGetComponent<Boid>(out Boid boid))
        {
            boid.enabled = false;
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
            
            GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Boid>().enabled = false;

            StartCoroutine(failsafe_movement());
            
            yield return new WaitForSeconds(flight_duration * 10);
            GetComponent<BoxCollider>().isTrigger = true; //hopefully this will make it so that after double the time has passed, if the fish has still not reached the seller, turn off its physical collider and let it go through the floor and shish.
        }
        else
        {
            StopCoroutine(failsafe_counter_2());
        }
        //Debug.Log("failed the safe");
        

    }

    public IEnumerator failsafe_movement()
    {
        while (TimeManager.current.update == true)
        {
            yield return new WaitForSeconds(0.1f);
            
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
                speed * Time.deltaTime);
            
            
        }
    }

    
    #endregion
    /*private void OnCollisionEnter(Collision collision)
    {
        if (this.GetComponent<Collider>() != null)
        {
           

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
