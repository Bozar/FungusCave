using System.Collections.Generic;
using UnityEngine;

public class BuildDungeon : MonoBehaviour
{
    private readonly int dungeonHeight = 17;
    private readonly int dungeonWidth = 24;
    private DungeonBlock[,] board;
    private ConvertCoordinates convertCoordinates;
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
        int[] index = convertCoordinates.Convert(position);

        return CheckTerrain(block, index[0], index[1]);
    }

    public GameObject GetBlock(Vector3 position)
    {
        int[] index = convertCoordinates.Convert(position);
        string dictKey = index[0] + "," + index[1];
        GameObject block;

        if (poolBlocks.TryGetValue(dictKey, out block))
        {
            return block;
        }
        return null;
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
                        newTile = wallTile;
                        blockDict = wallBlocks;
                        break;

                    case DungeonBlock.Pool:
                        newTile = poolTile;
                        blockDict = poolBlocks;
                        break;

                    default:
                        newTile = null;
                        blockDict = null;
                        break;
                }

                blockKey = x.ToString() + ',' + y.ToString();

                if (newTile != null && blockDict != null
                    && !blockDict.ContainsKey(blockKey))
                {
                    newTile = Instantiate(newTile);
                    newTile.transform.position
                        = convertCoordinates.Convert(x, y);

                    blockDict.Add(blockKey, newTile);
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

    private void Start()
    {
        convertCoordinates = FindObjects.GameLogic.
            GetComponent<ConvertCoordinates>();

        PlaceWallsManually();
        CreateDungeonObjects();
    }
}
