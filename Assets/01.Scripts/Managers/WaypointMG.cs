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
}
