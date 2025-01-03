using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections;
 using Sirenix.Serialization;
public class LevelMG : SerializedMonoBehaviour
{
     [InlineEditor]
    [SerializeField] private WaveManager waveManager;
    int currentWave = 0;
    public UnityEvent onLevelComplete;

  
    public Dictionary<int, EnemyMG[]> perWaveEnemyPortals;
    public GridBuilder levelBuilder;
    Wave currentWaveData;
    public int GetCurrentWave() => currentWave;


    IEnumerator Start()
    {
         if (!waveManager)
        {
            Debug.LogError("No wave manager Found");
            Destroy(this);
        }
        setupWave();

        if (currentWaveData != null && currentWaveData.spawnOnStart)
        {
            yield return new WaitForSeconds(currentWaveData.delayBeforeFirstSpawn);
            startSpawning();
        }
    }
    void startSpawning()
    {
        if (currentWaveData.enemiesPrefabs != null && currentWaveData.enemiesPrefabs.Length > 0)
        {
            StartCoroutine(spawnUnit());
        }
    }
    public EnemyMG getValidSpawnPoint()
    {
        if (perWaveEnemyPortals.ContainsKey(currentWave))
        {
            var random = Random.Range(0, perWaveEnemyPortals[currentWave].Length);
            return perWaveEnemyPortals[currentWave][random];
        }
        return null;
    }
    public EnemyMG[] getCurrentWavePortals()
    {
        return perWaveEnemyPortals[currentWave];
    }
     

    void LevelComplete()
    {
        Debug.Log("Level Complete");
        onLevelComplete.Invoke();
    }

    public BaseEnemy GetRandomEnemy()
    {
        if (waveManager)
        {
           return waveManager.GetRandomEnemy(currentWave);
        }
        return null;
    }

    IEnumerator spawnUnit()
    {

        if (canSpawnEnemy())
        {

            var spawnPoint = getValidSpawnPoint();
            if (spawnPoint != null)
            {                
                spawnPoint.spawnUnit(GetRandomEnemy());
                yield return new WaitForSeconds(currentWaveData.delayBetweenSpawns);
                StartCoroutine(spawnUnit());
            }
        }
        else if (currentWaveData != null && getCurrentWaveSpawnedEnemies()  >= currentWaveData.totalEnemies && getCurrentAliveEnemies()== 0)
        {
            foreach (var portal in getCurrentWavePortals())
            {
                portal.resetPortal();
            }
            currentWave++;
            setupWave();
        }
        else if (currentWaveData != null)
        {
            yield return new WaitForSeconds(currentWaveData.delayBetweenSpawns);
            StartCoroutine(spawnUnit());
        }


    }
    void setupWave()
    {
        Debug.Log($"Setting up wave {currentWave}");    
        currentWaveData = waveManager.getWaveAtIndex(currentWave);
        if (currentWaveData == null)
        {
            if (waveManager.allWavesCleared(currentWave))
            {
                LevelComplete();
            }
            else
            {
                Debug.LogError("Error getting Data");
            }
        }
        else
        {
            if(levelBuilder)
            {
                levelBuilder.LoadLevelWave(currentWave);
            }
            StartCoroutine(prepareNextWave());
        }
    }
    IEnumerator prepareNextWave()
    {
        yield return new WaitForSeconds(waveManager.delayBetweenWaves);
        StartCoroutine(spawnUnit());
    }
    public bool canSpawnEnemy()
    {
        int currentEnemies=0;
        int currentWaveSpawnedEnemies=0;
        if(currentWave<perWaveEnemyPortals.Count)
        foreach (var portals in perWaveEnemyPortals[currentWave])
        {
            currentEnemies += portals.getCurrentEnemies();
            currentWaveSpawnedEnemies += portals.getCurrentWaveSpawnedEnemies();
        }
        return currentWave<perWaveEnemyPortals.Count && currentEnemies < currentWaveData.maxConcurrentEnemies && currentWaveSpawnedEnemies < currentWaveData.totalEnemies;
    }

    int getCurrentWaveSpawnedEnemies()
    {
        int currentWaveSpawnedEnemies = 0;
        foreach (var portals in perWaveEnemyPortals[currentWave])
        {
             currentWaveSpawnedEnemies += portals.getCurrentWaveSpawnedEnemies();
        }
        return currentWaveSpawnedEnemies;
    }
    int getCurrentAliveEnemies()
    {
        int currentEnemies = 0;
        foreach (var portals in perWaveEnemyPortals[currentWave])
        {
            currentEnemies += portals.getCurrentEnemies();
        }
        return currentEnemies;
    }
}
