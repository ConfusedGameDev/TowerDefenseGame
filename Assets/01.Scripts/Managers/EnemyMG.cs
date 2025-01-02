using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[RequireComponent(typeof(WaypointMG))]
public class EnemyMG : MonoBehaviour
{

    [SerializeField] private WaveManager waveManager;
    int currentWave = 0;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Transform enemiesHolder;
    List<BaseEnemy> currentEnemies = new List<BaseEnemy>();
    Wave currentWaveData;
    int currentWaveSpawnedEnemies = 0;
    WaypointMG waypointMG;

    public Transform getNextWaypoint(int x)=> waypointMG? waypointMG.getNextWaypoint(x): null;
    public float getTotalDistance(int x, Vector3 pos) => waypointMG ? waypointMG.getTotalDistance(x, pos) : 0f;

    public void onEnemyKilled(BaseEnemy killedUnit)
    {
        if (currentEnemies.Contains(killedUnit))
        {
            currentEnemies.Remove(killedUnit);
        }
    }

    IEnumerator spawnUnit()
    {

        if (currentWaveData!=null && currentEnemies.Count < currentWaveData.maxConcurrentEnemies && currentWaveSpawnedEnemies < currentWaveData.totalEnemies)
        {

            currentWaveSpawnedEnemies++;
            int i = Random.Range(0, currentWaveData.enemiesPrefabs.Length);
            BaseEnemy newEnemy = Instantiate(currentWaveData.enemiesPrefabs[i], spawnPoint.position, spawnPoint.rotation);
            newEnemy.enemyParent = this;
            newEnemy.transform.parent = enemiesHolder ? enemiesHolder : transform;
            currentEnemies.Add(newEnemy);
            yield return new WaitForSeconds(currentWaveData.delayBetweenSpawns);
            StartCoroutine(spawnUnit());

        }
        else if (currentWaveData != null &&  currentWaveSpawnedEnemies >= currentWaveData.totalEnemies && currentEnemies.Count==0)
        {
            currentWave++;
            setupWave();
        }
        else if(currentWaveData != null )
        {
            yield return new WaitForSeconds(currentWaveData.delayBetweenSpawns);
            StartCoroutine(spawnUnit());
        }
      

    }
    IEnumerator Start()
    {
        waypointMG = GetComponent<WaypointMG>();
        if (!waveManager)
        {
            Debug.LogError("No wave manager Found");
            Destroy(this);
        }
        setupWave();
       
        if (currentWaveData!=null && currentWaveData.spawnOnStart)
        {
            yield return new WaitForSeconds(currentWaveData.delayBeforeFirstSpawn);
            startSpawning();
        }
    }
    void setupWave()
    {

        currentWaveSpawnedEnemies = 0;
        currentWaveData = waveManager.getWaveAtIndex(currentWave);

        
        if(currentWaveData==null)
        {
            if(waveManager.allWavesCleared(currentWave))
            {
                LevelMG.Instance.onPortalDeactivated(this);
            }
            else
            {
                Debug.LogError("Error getting Data");
            }
        }
        else
        {
           StartCoroutine( prepareNextWave());
        }
    }
    IEnumerator prepareNextWave()
    {
        yield return new WaitForSeconds(waveManager.delayBetweenWaves);        
        StartCoroutine(spawnUnit());
    }

    void startSpawning()
    {
        if (spawnPoint && currentWaveData.enemiesPrefabs != null && currentWaveData.enemiesPrefabs.Length > 0)
        {
            StartCoroutine(spawnUnit());
        }
    }

}
