using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class TileSetHolder : SerializedMonoBehaviour
{

    public Dictionary<string, Tile> tilesPrefabs;
    void Start()
    {
        
    }

    public Tile getTile(string tileName)
    {
        if (tilesPrefabs != null && tilesPrefabs.ContainsKey(tileName))
        {
            return tilesPrefabs[tileName];
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
