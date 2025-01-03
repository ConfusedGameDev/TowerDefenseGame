using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

public class GridBuilder : SingletonComponent<GridBuilder>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int width = 10;
    public int height = 10;

    public Tile defaultTile;

    [ShowInInspector]
    [ReadOnly]
    Tile[,] tiles;
    [SerializeField] NavMeshSurface navMeshController;

    [InlineEditor]
    public LevelData levelData;
    [Button]
    public void BuildGrid()
    {
        if (!defaultTile) return;
        ClearGrid();
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                tiles[i, j] = Instantiate(defaultTile, new Vector3(i, 0, j), Quaternion.identity);
                tiles[i, j].transform.parent = transform;
                tiles[i, j].SetID(i, j);
                tiles[i, j].updateName();
            }

        updateNavMesh();
    }

    private void ClearGrid()
    {
        if (tiles != null)
        {
            foreach (var tile in tiles)
            {
                if (tile == null) continue;
                if (Application.isPlaying)
                    Destroy(tile.gameObject);
                else
                    DestroyImmediate(tile.gameObject);
            }
        }
        tiles = new Tile[width, height];
    }

    public void updateTile(int x, int y, Tile t)
    {
        if (tiles == null) return;
        if (x < width && y < height)
        {
            tiles[x, y] = t;
        }
    }


    [Button]
    public void updateNavMesh()
    {
        if (navMeshController)
        {
            navMeshController.BuildNavMesh();
        }

    }

    [Button]
    public void fetchTiles()
    {
        if (width * height != transform.childCount)
        {
            Debug.LogError("Tiles cant be loaded correctly");
            return;
        }
        tiles= new Tile[width, height];
        AssignChildrenToTiles();
         
    }
    public void AssignChildrenToTiles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {

        
            var child = transform.GetChild(i);
            string childName = child.name; // e.g., "Ground(7,8)"

            // 1) Find the parentheses
            int leftParen = childName.IndexOf('(');
            int rightParen = childName.IndexOf(')');

            // Make sure we found the parentheses properly
            if (leftParen < 0 || rightParen < 0 || rightParen <= leftParen)
            {
                Debug.LogWarning($"Invalid child name format: {childName}");
                continue;
            }

            // 2) Extract the substring containing "7,8"
            string coordinatePart = childName.Substring(
                leftParen + 1,
                rightParen - (leftParen + 1)
            ); // e.g. "7,8"

            // 3) Split the string by comma
            string[] coordinates = coordinatePart.Split(',');
            if (coordinates.Length != 2)
            {
                Debug.LogWarning($"Invalid coordinate format: {childName}");
                continue;
            }

            // 4) Parse to integers
            if (int.TryParse(coordinates[0], out int x) && int.TryParse(coordinates[1], out int y))
            {
                // 5) Assign to your 2D array
                tiles[x, y] = child.GetComponent<Tile>();
            }
            else
            {
                Debug.LogWarning($"Could not parse coordinates as integers: {childName}");
            }
        }
    }

    [ShowIf("levelData")]
    [Button]
    void SaveLevelData(int waveNumber)
    {
       
            fetchTiles();
        if(levelData && tiles!= null)
        {
            levelData.saveTile(waveNumber, tiles);
        }
    }

    [ShowIf("levelData")]
    [Button]
    public void LoadLevelWave(int waveNumber)
    { 
            fetchTiles();
        
            
        if (levelData)
        {
            var gridSize= levelData.getWaveGridSize(waveNumber); 
            if (gridSize.Key!= width|| gridSize.Value!= height)
            {
                width= gridSize.Key;
                height= gridSize.Value;
                BuildGrid();
            }

        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tile = transform.GetChild(j + (width * i)).GetComponent<Tile>();
                if (tile)
                {

                    if (tiles[i, j].tileType == levelData.getTileType(waveNumber, i, j)) continue;
                   tiles[i,j]= tiles[i,j].changeTileType(levelData.getTileType(waveNumber,i,j).ToString());
                   tiles[i,j].transform.rotation= levelData.getTileRotation(waveNumber,i,j);
                   
                }

            }
        }

        Selection.activeGameObject = gameObject;
    }

}
