using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField]    
    private Transform currentWaypoint;
    private NavMeshAgent agent;
    private int currentWP = 0;

    public bool debug=true;
    public float rotationSpeed = 10f;

    public float attackDamage = 1f;
    public float deadDelay = 0.2f;
    public void updateTarget(Transform newTarget)
    {
        currentWaypoint = newTarget;

        if(agent)
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
        Destroy(gameObject);
    }
    void getNextWaypoint()
    {       
        if (currentWP < waypoints.Length)
        {
            updateTarget(waypoints[currentWP]);
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
        transform.rotation= Quaternion.Lerp(transform.rotation,targetRoataion, Time.deltaTime*rotationSpeed);
    }
     void Start()
    {
        agent= GetComponent<NavMeshAgent>();
        if(agent==null)
        {
            Debug.Log("No nave mesh found removing");
            Destroy(this);
        }
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
        if(waypoints.Length>0)
        {
            getNextWaypoint();
        }
    }

    private void OnDrawGizmos()
    {
        if(!debug)
        {
            return;
        }
        for(int i = 0; i < waypoints.Length; i++)  
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(waypoints[i].position, 0.25f);
            if(i>0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(waypoints[i-1].position, waypoints[i].position);

            }
        }
        if(currentWaypoint)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(currentWaypoint.position, 0.24f);

        }


    }
    public void Update()
    {
        if(agent && currentWaypoint)
        {
            FaceTarget(agent.steeringTarget);
            if(agent.remainingDistance<agent.stoppingDistance)
            {
                getNextWaypoint();
            }
        }
    }
}
