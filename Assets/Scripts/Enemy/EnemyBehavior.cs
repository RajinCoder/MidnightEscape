using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    /*
    [Header("Chase Target")]
    public Transform player;

    [Header("Enemy Settings")]
    public int enemyHealth = 100;
    public float moveSpeed = 10;
    public float walkDistance = 10f;
    public float runDistance = 5f;
    public int damageAmount = 20;

    private bool notDead = true;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (notDead)
        {
            float step = moveSpeed * Time.deltaTime;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > walkDistance)
            {
                animator.SetInteger("animState", 0); // Idle
            }
            else if (distance > runDistance)
            {
                animator.SetInteger("animState", 1); // Walking
            }
            else
            {
                animator.SetInteger("animState", 2); // Running
            }

            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        enemyHealth -= damageAmount;

        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        notDead = false;
        animator.SetInteger("animState", 3); // Dead
        FindObjectOfType<LevelManager>().LevelBeat();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) 
        {
            StartCoroutine(DealDamage());
        }

        if (other.CompareTag("Bullet"))
        {
            TakeDamage(20);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DealDamage());
        }
    }

    private IEnumerator DealDamage()
    {
        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damageAmount);
        yield return new WaitForSeconds(1);
    }
    */

    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }
    public FSMStates currentState;

    [Header("Enemy Settings")]
    public float attackDistance = 1;
    public float chaseDistance = 10;
    public float attackRate = 2.0f;
    public Transform enemyEyes;
    public float fieldOfView = 90f;

    // Player Info
    public GameObject player;
    float distanceToPlayer;

    // Enemy Components
    Animator anim;
    NavMeshAgent agent;
    EnemyHealth enemyHealth;

    // Wander Points
    GameObject[] wanderPoints;
    Vector3 nextDestination;
    int currentDestinationIndex = 0;

    // Attack Time
    float elapsedTime = 0;

    // Current health
    int health;

    // Death Info
    Transform deadTransform;
    bool isDead;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        health = enemyHealth.currentHealth;

        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Dead:
                UpdateDeadState();
                break;
        }

        elapsedTime += Time.deltaTime;

        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }
    }

    private void Initialize()
    {
        currentState = FSMStates.Patrol;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();

        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        player = GameObject.FindGameObjectWithTag("Player");

        health = enemyHealth.currentHealth;
        isDead = false;

        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        anim.SetInteger("animState", 1);

        agent.stoppingDistance = 0;
        agent.speed = 3.5f;

        if (Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }
        else if (IsPlayerInClearFOV())
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }

    void UpdateChaseState()
    {
        anim.SetInteger("animState", 2);

        nextDestination = player.transform.position;

        agent.stoppingDistance = attackDistance;
        agent.speed = 5;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            FindNextPoint();
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }

    void UpdateAttackState()
    {
        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        anim.SetInteger("animState", 3);

        MeleeAttack();
    }

    void UpdateDeadState()
    {
        anim.SetInteger("animState", 4);

        isDead = true;
        deadTransform = gameObject.transform;

        Destroy(gameObject, 3);
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;

        agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void MeleeAttack()
    {
        if (!isDead)
        {
            if (elapsedTime >= attackRate)
            {
                var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;

                // attack code here / deal damage if collision with melee hits
                elapsedTime = 0.0f;
            }
        }
    }

    private void OnDestroy()
    {
        /*
        Instantiate(deadVFX, deadTransform.position, deadTransform.rotation);
        */
    }

    private void OnDrawGizmos()
    {
        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;

        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
}