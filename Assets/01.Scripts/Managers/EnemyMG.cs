using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.GlobalIllumination;


[RequireComponent(typeof(WaypointMG))]
public class EnemyMG : SerializedMonoBehaviour
{

   
    [SerializeField] public Transform spawnPoint;

    [SerializeField] private Transform enemiesHolder;
    List<BaseEnemy> currentEnemies = new List<BaseEnemy>();
    
    int currentWaveSpawnedEnemies = 0;
    WaypointMG waypointMG;



    public int getCurrentEnemies() => currentEnemies.Count;
    public int getCurrentWaveSpawnedEnemies() => currentWaveSpawnedEnemies;
   
    public Transform getNextWaypoint(int x)=> waypointMG? waypointMG.getNextWaypoint(x): null;
    public float getTotalDistance(int x, Vector3 pos) => waypointMG ? waypointMG.getTotalDistance(x, pos) : 0f;

    public void onEnemyKilled(BaseEnemy killedUnit)
    {
        if (currentEnemies.Contains(killedUnit))
        {
            currentEnemies.Remove(killedUnit);
        }
    }

    public void spawnUnit(BaseEnemy enemyToSpawn)
    {
        currentWaveSpawnedEnemies++;
        BaseEnemy newEnemy = Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
        newEnemy.enemyParent = this;
        newEnemy.transform.parent = enemiesHolder ? enemiesHolder : transform;
        currentEnemies.Add(newEnemy);        
      

    }
    void Start()
    {
        waypointMG = GetComponent<WaypointMG>();
         
    }
    public void resetPortal()
    {

        currentWaveSpawnedEnemies = 0;
    }
    

  

}
