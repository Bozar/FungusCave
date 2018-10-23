using UnityEngine;

// Create a 2D array. Provide methods to inspect and change its content.
public class DungeonBoard : MonoBehaviour
{
    private ConvertCoordinates coordinate;

    public enum DungeonBlock { Floor, Wall, Pool, Fungus };

    public enum FOVShape { Rhombus };

    public GameObject[,] Blocks { get; set; }
    public DungeonBlock[,] Blueprint { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }

    public bool ChangeBlueprint(DungeonBlock block, int x, int y)
    {
        if (IndexOutOfRange(x, y))
        {
            return false;
        }

        Blueprint[x, y] = block;
        return true;
    }

    public bool CheckBlock(DungeonBlock block, int x, int y)
    {
        if (IndexOutOfRange(x, y))
        {
            return false;
        }

        return Blueprint[x, y] == block;
    }

    public bool CheckBlock(DungeonBlock block, Vector3 position)
    {
        int[] index = coordinate.Convert(position);

        return CheckBlock(block, index[0], index[1]);
    }

    public GameObject GetBlock(Vector3 position)
    {
        int[] index = coordinate.Convert(position);
        return GetBlock(index[0], index[1]);
    }

    public GameObject GetBlock(int x, int y)
    {
        return Blocks[x, y];
    }

    public bool IsInsideRange(FOVShape shape, int maxRange,
        int[] source, int[] target)
    {
        bool check;

        switch (shape)
        {
            case FOVShape.Rhombus:
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

        Blueprint = new DungeonBlock[Width, Height];
        Blocks = new GameObject[Width, Height];
    }

    private bool IndexOutOfRange(int x, int y)
    {
        bool checkWidth = (x < 0) || (x > Width - 1);
        bool checkHeight = (y < 0) || (y > Height - 1);

        return checkWidth || checkHeight;
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
    }
}
