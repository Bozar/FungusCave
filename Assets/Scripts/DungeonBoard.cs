using System.Collections.Generic;
using UnityEngine;

// Create a 2D array. Provide methods to inspect and change its content.
public class DungeonBoard : MonoBehaviour
{
    private ConvertCoordinates coordinate;

    public enum DungeonBlock { Floor, Wall, Pool };

    public enum RangeType { Rhombus };

    public DungeonBlock[,] Board { get; private set; }
    public int Height { get; private set; }
    public Dictionary<string, GameObject> PoolBlocks { get; private set; }
    public Dictionary<string, GameObject> WallBlocks { get; private set; }
    public int Width { get; private set; }

    public bool ChangeBlock(DungeonBlock block, int x, int y)
    {
        if (IndexOutOfRange(x, y))
        {
            return false;
        }

        Board[x, y] = block;
        return true;
    }

    public bool CheckTerrain(DungeonBlock block, int x, int y)
    {
        if (IndexOutOfRange(x, y))
        {
            return false;
        }

        return Board[x, y] == block;
    }

    public bool CheckTerrain(DungeonBlock block, Vector3 position)
    {
        int[] index = coordinate.Convert(position);

        return CheckTerrain(block, index[0], index[1]);
    }

    public GameObject GetBlock(Vector3 position)
    {
        int[] index = coordinate.Convert(position);
        string dictKey = index[0] + "," + index[1];
        GameObject block;

        if (PoolBlocks.TryGetValue(dictKey, out block))
        {
            return block;
        }
        return null;
    }

    public bool InsideRange(RangeType shape, int maxRange,
        int[] source, int[] target)
    {
        bool check;

        switch (shape)
        {
            case RangeType.Rhombus:
                check = System.Math.Abs(target[0] - source[0])
                    + System.Math.Abs(target[1] - source[1])
                    <= maxRange;
                break;

            default:
                check = false;
                break;
        }
        return check;
    }

    private void Awake()
    {
        Height = 17;
        Width = 24;

        Board = new DungeonBlock[Width, Height];
        PoolBlocks = new Dictionary<string, GameObject>();
        WallBlocks = new Dictionary<string, GameObject>();
    }

    private bool IndexOutOfRange(int x, int y)
    {
        bool checkWidth;
        bool checkHeight;

        checkWidth = x < Board.GetLowerBound(0) || x > Board.GetUpperBound(0);
        checkHeight = y < Board.GetLowerBound(1) || y > Board.GetUpperBound(1);

        return checkWidth || checkHeight;
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
    }
}
