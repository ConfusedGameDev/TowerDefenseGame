using System.Diagnostics;
using UnityEngine;

public class WaypointMG : SingletonComponent<WaypointMG>
{
    [SerializeField] private Transform[] waypoints;
     public bool debug = true;
    private void OnDrawGizmos()
    {
        if (!debug || waypoints== null)
        {
            return;
        }
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(waypoints[i].position, 0.25f);
            if (i > 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);

            }
        }
         


    }
    public Transform getNextWaypoint(int currentWP)
    {
        if (currentWP < waypoints.Length)
        {
            return waypoints[currentWP];
            
        }
        else
        {
            return null;
        }
    }

    public float getTotalDistance(int currentWP, Vector3 startPos)
    {
        float totalDistance = 0;
        if ( currentWP < waypoints.Length)
        {
            totalDistance += Vector3.Distance(startPos, waypoints[currentWP].position);
            for (int i = currentWP; i < waypoints.Length-1; i++)
            {
                totalDistance += Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            }
           
        }
        return totalDistance;
    }
}
