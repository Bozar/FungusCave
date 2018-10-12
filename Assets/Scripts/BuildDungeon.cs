using System.Collections.Generic;
using UnityEngine;

public class BuildDungeon : MonoBehaviour
{
    private readonly int dungeonHeight = 17;
    private readonly int dungeonWidth = 24;
    private DungeonBlock[,] board;
    private Dictionary<string, GameObject> poolBlocks;
    private Dictionary<string, GameObject> wallBlocks;

    public enum DungeonBlock { Floor, Wall, Pool };

    public bool CheckTerrain(DungeonBlock block, int x, int y)
    {
        if (IndexOutOfRange(x, y))
        {
            return false;
        }

        return board[x, y] == block;
    }

    public bool CheckTerrain(DungeonBlock block, Vector3 position)
    {
        int[] index = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(position);

        return CheckTerrain(block, index[0], index[1]);
    }

    private void Awake()
    {
        board = new DungeonBlock[dungeonWidth, dungeonHeight];
        poolBlocks = new Dictionary<string, GameObject>();
        wallBlocks = new Dictionary<string, GameObject>();
    }

    private bool ChangeBlock(DungeonBlock block, int x, int y)
    {
        if (IndexOutOfRange(x, y))
        {
            return false;
        }

        board[x, y] = block;
        return true;
    }

    private void CreateDungeonObjects()
    {
        GameObject wallTile = Resources.Load("Wall") as GameObject;
        GameObject poolTile = Resources.Load("Pool") as GameObject;
        GameObject newTile;
        Dictionary<string, GameObject> blockDict;
        string blockKey;

        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                switch (board[x, y])
                {
                    case DungeonBlock.Wall:
                        newTile = Instantiate(wallTile);
                        blockDict = wallBlocks;
                        break;

                    case DungeonBlock.Pool:
                        newTile = Instantiate(poolTile);
                        blockDict = poolBlocks;
                        break;

                    default:
                        newTile = null;
                        blockDict = null;
                        break;
                }

                if (newTile != null && blockDict != null)
                {
                    blockKey = x.ToString() + ',' + y.ToString();

                    newTile.transform.position
                            = gameObject.GetComponent<ConvertCoordinates>()
                            .Convert(x, y);

                    if (!blockDict.ContainsKey(blockKey))
                    {
                        blockDict.Add(blockKey, newTile);
                    }
                }
            }
        }
    }

    private bool IndexOutOfRange(int x, int y)
    {
        bool checkWidth;
        bool checkHeight;

        checkWidth = x < board.GetLowerBound(0) || x > board.GetUpperBound(0);
        checkHeight = y < board.GetLowerBound(1) || y > board.GetUpperBound(1);

        return checkWidth || checkHeight;
    }

    private void PlaceWallsManually()
    {
        ChangeBlock(DungeonBlock.Wall, 4, 4);
        ChangeBlock(DungeonBlock.Wall, 5, 5);
        ChangeBlock(DungeonBlock.Wall, 6, 6);
        ChangeBlock(DungeonBlock.Wall, dungeonWidth - 1, dungeonHeight - 1);
        ChangeBlock(DungeonBlock.Wall, dungeonWidth, dungeonHeight - 1);
        ChangeBlock(DungeonBlock.Pool, 8, 8);
        ChangeBlock(DungeonBlock.Pool, 8, 9);
        ChangeBlock(DungeonBlock.Pool, 9, 9);
    }

    private void PrintDictionary()
    {
        foreach (var block in wallBlocks)
        {
            Debug.Log(block.Key + ": " + block.Value.name);
        }

        foreach (var block in poolBlocks)
        {
            Debug.Log(block.Key + ": " + block.Value.name);
        }

        Debug.Log(poolBlocks["8,9"].transform.position);
    }

    private void Start()
    {
        PlaceWallsManually();
        CreateDungeonObjects();
        PrintDictionary();
    }
}
