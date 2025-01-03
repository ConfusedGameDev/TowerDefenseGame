using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct tileData
{
    public TileType tileType;
    public quaternion rotation;

    public tileData(TileType tileType, quaternion rotation)
    {
        this.tileType = tileType;
        this.rotation = rotation;
    }
}
[CreateAssetMenu(fileName = "New Level Data", menuName = "Limbik/Data/NewLevelData", order = 1)]
public class LevelData : SerializedScriptableObject
{
    [SerializeField]
    public List<tileData[,]> tiles = new List<tileData[,]>();

    public void saveTile(int waveNumber, Tile[,] tilesData)
    {
        if (tiles == null)
        {
            tiles = new List<tileData[,]>();
        }
        if (waveNumber < tiles.Count)
        {
            tiles[waveNumber] = new tileData[tilesData.GetLength(0), tilesData.GetLength(1)];
            for (int i = 0; i < tilesData.GetLength(0); i++)
            {
                for (int j = 0; j < tilesData.GetLength(1); j++)
                {
                    tiles[waveNumber][i, j] = new tileData(tilesData[i, j].tileType, tilesData[i,j].transform.rotation);
                }
            }
        }
        else
        {
            var newData = new tileData[tilesData.GetLength(0), tilesData.GetLength(1)];
            for (int i = 0; i < tilesData.GetLength(0); i++)
            {
                for (int j = 0; j < tilesData.GetLength(1); j++)
                {
                    newData[i, j] = new tileData(tilesData[i, j].tileType, tilesData[i, j].transform.rotation);
                }
            }
            tiles.Add(newData);

        }
    }

    public KeyValuePair<int, int> getWaveGridSize(int waveNumber)
    {
        if (waveNumber < tiles.Count)
        {
            return new KeyValuePair<int, int>(tiles[waveNumber].GetLength(0), tiles[waveNumber].GetLength(1));
        }

        return new KeyValuePair<int, int>(0, 0);
    }

    public TileType getTileType(int waveN, int x, int y)
    {
      return   tiles[waveN][x, y].tileType;
    }
    public Quaternion getTileRotation(int waveN, int x, int y) => tiles[waveN][x, y].rotation;
}
