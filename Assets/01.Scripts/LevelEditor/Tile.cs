using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public enum TileType
{
    Ground,
    Road,
    Hill1,
    Hill2,
    Hill3,
    InnerCornerSmall,
    InnerCornerBig,
    OuterCornerSmall,
    OuterCornerBig,
    BridgeRoad,
    BridgeField,
    BridgeSideWay,
    sideWay

}
public class Tile : MonoBehaviour
{


    public TileType tileType;

    int idX;
    int idY;

    public void SetID(int x, int y)
    {
        idX = x; idY = y;
        updateName();
    }
    public int getIdX() => idX;
    public int getIdY() => idY;
    public Tile changeTileType(Tile tile)
    {
        if (tile == null) return null;
        var newTile = Instantiate(tile.gameObject, transform.position, transform.rotation).GetComponent<Tile>();
        newTile.transform.parent = transform.parent;
        newTile.SetID(idX, idY);
        Selection.activeObject = newTile;
        gameObject.SetActive(false);
        GridBuilder.Instance.updateTile(newTile.getIdX(), newTile.getIdY(), newTile);
        GridBuilder.Instance.updateNavMesh();
        if(gameObject)
        DestroyImmediate(gameObject);

        return newTile;


    }

    public Tile changeTileType(string tileType)
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        var t = tHolder.getTile(tileType);
        if (t != null)
        {
          return  changeTileType(t);
        }

        return null;
    }
    public void updateName()
    {
        gameObject.name = tileType + $"({ Mathf.RoundToInt(transform.position.x)},{Mathf.RoundToInt(transform.position.z)})";

    }

    [TabGroup("Main Blocks")]
    
    [Button( ButtonHeight =50) ]
    public void getRoadTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("Road"));
    }
    [TabGroup("Main Blocks")]
     [Button( ButtonHeight =50)]
    public void getGroundTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("Ground"));
    }
    [TabGroup("sideWay Blocks")]
    [Button( ButtonHeight =50)]
    public void getSideWayTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("sideWay"));
    }

    [TabGroup("Corner Blocks")]

    [Button( ButtonHeight =50)]
    public void getOuterCornerSmallTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("OuterCornerSmall"));
    }
    [TabGroup("Corner Blocks")]

    [Button( ButtonHeight =50)]
    public void getInnerSmallCornerTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("InnerCornerSmall"));
    }

    [TabGroup("Corner Blocks")]

    [Button(ButtonHeight = 50)]
    public void getInnerCornerBigTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("InnerCornerBig"));
    }

    [TabGroup("Corner Blocks")]

    [Button(ButtonHeight = 50)]
    public void getOuterCornerBigTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("OuterCornerBig"));
    }

    [TabGroup("Hill Blocks")]

    [Button( ButtonHeight =50)]
    public void getHill3Tile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("Hill3"));
    }

    [TabGroup("Hill Blocks")]

    [Button( ButtonHeight =50)]
    public void getHill2Tile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("Hill2"));
    }

    [TabGroup("Hill Blocks")]

    [Button( ButtonHeight =50)]
    public void getHill1Tile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("Hill1"));
    }
    [TabGroup("sideWay Blocks")]
    [Button( ButtonHeight =50)]
    public void getBridgeSideWayTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("BridgeSideWay"));
    }

    [TabGroup("Bridge Blocks")]

    [Button( ButtonHeight =50)]
    public void getBridgeRoadTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("BridgeRoad"));
    }
    [TabGroup("Bridge Blocks")]

    [Button( ButtonHeight =50)]
    public void geBridgeFieldTile()
    {
        var tHolder = FindAnyObjectByType<TileSetHolder>();
        if (tHolder)
            changeTileType(tHolder.getTile("BridgeField"));
    }

    [TabGroup("Vertical Controls")]

    [Button( ButtonHeight =50)]
    public void MoveTileUp()
    {

        transform.position += Vector3.up * 0.1f;
        GridBuilder.Instance.updateNavMesh();

    }
    [TabGroup("Vertical Controls")]

    [Button( ButtonHeight =50)]
    public void MoveTileDown()
    {
        transform.position += Vector3.up * -0.1f;
        GridBuilder.Instance.updateNavMesh();

    }

    [TabGroup("Position Controls")]

    [Button( ButtonHeight =50)]
    public void MoveTileX()
    {

        transform.position += Vector3.right;
        GridBuilder.Instance.updateNavMesh();

        updateName();

    }
    [TabGroup("Position Controls")]

    [Button( ButtonHeight =50)]
    public void MoveTileXMinus()
    {
        transform.position -= Vector3.right;
        GridBuilder.Instance.updateNavMesh();

        updateName();

    }
    [TabGroup("Position Controls")]

    [Button( ButtonHeight =50)]
    public void MoveTileZ()
    {
        transform.position += Vector3.forward;
        GridBuilder.Instance.updateNavMesh();

        updateName();

    }
    [TabGroup("Position Controls")]

    [Button( ButtonHeight =50)]
    public void MoveTileZMinus()
    {
        transform.position -= Vector3.forward;
        GridBuilder.Instance.updateNavMesh();

        updateName();

    }
     
    [TabGroup("Rotation Controls")]
    [Button( ButtonHeight =50)]
    public void rotateClockWise()
    {
        transform.Rotate(0, 90, 0);
        GridBuilder.Instance.updateNavMesh();

    }
    [TabGroup("Rotation Controls")]
    [Button( ButtonHeight =50)]
    public void rotateCounterClockWise()
    {
        transform.Rotate(0, -90, 0);
        GridBuilder.Instance.updateNavMesh();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
