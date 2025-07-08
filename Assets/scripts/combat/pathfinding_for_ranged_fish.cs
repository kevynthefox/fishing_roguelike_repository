using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class behavior_for_ranged_fish : MonoBehaviour
{
    [Header("pathfinding")]
    
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    

    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("health")]

    public Canvas Canvas;
    public GameObject health_bar;

    [Header("attacking")]

    public GameObject gun;

    private void Awake()
    {
        player = GameObject.Find("player").transform;
        agent = GetComponent<NavMeshAgent>();

        Canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

         
    }
    #region pathfinding
    private void patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }    

        Vector3 distanceToWalkpoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkpoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomx = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomx, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint,-transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        //make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //attack code here
            gun.GetComponent<gun>().fire();
            //

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack),timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    #endregion

    #region health
    public void TakeDamage(int damage)
    {
        health_bar.GetComponent<Health_display>().health -= damage / 2;

        if (health_bar.GetComponent<Health_display>().health <= 0) Invoke(nameof(DestroyEnemy), .5f);
        if (health_bar.GetComponent<Health_display>().health <= 60) this.gameObject.tag = "fish";
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
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
    #endregion
}
