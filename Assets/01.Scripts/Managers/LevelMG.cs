using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
public class LevelMG : SingletonComponent<LevelMG>
{
    [SerializeField] private List<EnemyMG> enemyPortals;
    public UnityEvent onLevelComplete;

    public void onPortalDeactivated(EnemyMG portal)
    {
        if(enemyPortals.Contains(portal))
        {
            enemyPortals.Remove(portal);
        }
        if(enemyPortals.Count == 0)
        {
            onGameOver();
        }
    }

    void onGameOver()
    {
        Debug.Log("Level Complete");
        onLevelComplete.Invoke();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
