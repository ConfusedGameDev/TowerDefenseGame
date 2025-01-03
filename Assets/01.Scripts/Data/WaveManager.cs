using System;
using UnityEngine;

[Serializable]
public class Wave
{
    [SerializeField] public BaseEnemy[] enemiesPrefabs;
    [SerializeField] public int totalEnemies = 20;
    [SerializeField] public int maxConcurrentEnemies = 5;
    [SerializeField] public float delayBetweenSpawns;
    [SerializeField] public float delayBeforeFirstSpawn = 3f;
    [SerializeField] public bool spawnOnStart;
}
[CreateAssetMenu(fileName = "New Wave", menuName = "Limbik/Data/New Wave Data",order = 0)]
public class WaveManager : ScriptableObject
{
   [SerializeField]  Wave[] waves;
    public float delayBetweenWaves;

    public Wave getWaveAtIndex(int index) => index < waves.Length ? waves[index] : null;

    public bool allWavesCleared(int index)=> index>= waves.Length;

    public BaseEnemy GetRandomEnemy(int wave)
    {
        if (wave<waves.Length)
        {
            if(waves[wave].enemiesPrefabs.Length>0)
            return waves[wave].enemiesPrefabs[UnityEngine.Random.Range(0, waves[wave].enemiesPrefabs.Length)];
        }
        return null;
    }
}
