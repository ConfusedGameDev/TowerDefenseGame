using Sirenix.OdinInspector;
using Unity.AI.Navigation;
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
}
