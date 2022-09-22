using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 场地管理器
/// </summary>
public class BattleManager: MonoBehaviour
{
    private static BattleManager _instance;
    public static BattleManager Instance { get { return _instance; } }
    
    public BattleTile tilePrefab;

    [SerializeField] private int gridSize = 1;
    [SerializeField] private int gridX = 12;
    [SerializeField] private int gridZ = 8;

    public Dictionary<Vector3Int, BattleTile> gridMap;

    


    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        gridMap = new Dictionary<Vector3Int, BattleTile>();

        // initial battle field
        var targetField = GameObject.Find("BattleField");
        for ( int x = gridX/2; x > -gridX/2; x--)
        {
            for (int z = gridZ/2;z > -gridZ/2; z--)
            {
                var tileKey = new Vector3Int(x, 0, z);
                if (!gridMap.ContainsKey(tileKey))
                {
                    var tileLocation = new Vector3Int(x, 0, z); // location in grid
                    //generate tiles
                    tileLocation *= gridSize;
                    var overLayTile = Instantiate(tilePrefab, targetField.transform);
                    overLayTile.transform.localScale *= gridSize;
                    overLayTile.transform.position = targetField.transform.position + tileLocation; // real mesh -- > no deed to remanage sorting order
                    overLayTile.gridPos = tileLocation;
                    gridMap.Add(tileKey, overLayTile);
                }

                
            }
        }


    }

    public List<BattleTile> GetNeightbourTiles(BattleTile current)
    {

        List<BattleTile> neighbours = new List<BattleTile>();

        //up
        Vector3Int upCheck = new Vector3Int(current.gridPos.x, current.gridPos.y, current.gridPos.z + 1);
        if (gridMap.ContainsKey(upCheck))
        {
            neighbours.Add(gridMap[upCheck]);
        }

        //down
        Vector3Int downCheck = new Vector3Int(current.gridPos.x, current.gridPos.y, current.gridPos.z - 1);
        if (gridMap.ContainsKey(downCheck))
        {
            neighbours.Add(gridMap[downCheck]);
        }

        //left
        Vector3Int leftCheck = new Vector3Int(current.gridPos.x - 1, current.gridPos.y, current.gridPos.z);
        if (gridMap.ContainsKey(leftCheck))
        {
            neighbours.Add(gridMap[leftCheck]);
        }

        //right
        Vector3Int rightCheck = new Vector3Int(current.gridPos.x + 1, current.gridPos.y, current.gridPos.z);
        if (gridMap.ContainsKey(rightCheck))
        {
            neighbours.Add(gridMap[rightCheck]);
        }

        return neighbours;
    }

    public void ClearAllTileHighlighter()
    {
        foreach(var tilePos in gridMap.Keys)
        {
            gridMap[tilePos].HideTile();
        }
    }


    public BattleTile GetTileByPos(Vector3Int pos )
    {
        return gridMap[pos];
    }
}