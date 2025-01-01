using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;


[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemy : MonoBehaviour, IDamageable
{
    [SerializeField]

    private NavMeshAgent agent;
    private int currentWP = 0;
    private Transform currentWaypoint;

    [SerializeField] float rotationSpeed = 10f;

    [SerializeField] float attackDamage = 1f;
    [SerializeField] float deadDelay = 0.2f;

    public EnemyMG enemyParent;

    [field:SerializeField]public float maxHealth { get; set; }

 
    [ShowInInspector] [ReadOnly]
    public float currentHealth { get; set ; }

    public GameObject onHitFX;
    public void updateTarget(Transform newTarget)
    {
        currentWaypoint = newTarget;

        if (agent)
        {
            agent.SetDestination(currentWaypoint.position);

        }

    }

    public virtual float DestroyEnemy()
    {
        StartCoroutine(OnDead());
        return attackDamage;
    }
    IEnumerator OnDead()
    {
        yield return new WaitForSeconds(deadDelay);
        if (enemyParent != null)
        {
            enemyParent.onEnemyKilled(this);
        }

        Destroy(gameObject);
    }
    void getNextWaypoint()
    {
        currentWaypoint = WaypointMG.Instance.getNextWaypoint(currentWP);
        if(currentWaypoint !=null)
        {
            updateTarget(currentWaypoint);
            currentWP++;
        }
        else
        {
            Debug.Log("Reached Final Destination");
        }
         
    }

    private void FaceTarget(Vector3 target)
    {
        var direction = target - transform.position;
        direction.y = 0;
        Quaternion targetRoataion = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRoataion, Time.deltaTime * rotationSpeed);
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.Log("No nave mesh found removing");
            Destroy(this);
        }
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
        getNextWaypoint();
        currentHealth = maxHealth;
        
    }


    public void Update()
    {
        if (agent && currentWaypoint)
        {
            FaceTarget(agent.steeringTarget);
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                getNextWaypoint();
            }
        }
    }

    public void onGetDamage(float damage)
    {
        if(onHitFX)
        {
            var hitFX = Instantiate(onHitFX, transform.position, Quaternion.identity);
            hitFX.transform.parent = transform;
        }
        currentHealth-= damage;
        if (currentHealth <= 0)
            onDead();
    }

    public void onDead()
    {
        DestroyEnemy();
    }

     
}
