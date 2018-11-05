using UnityEngine;

public enum BuildingTag { Floor, Wall, Pool, Fungus };

public enum FOVShape { Rhombus };

// Create a 2D array. Provide methods to inspect and change its content.
public class DungeonBoard : MonoBehaviour
{
    private ConvertCoordinates coordinate;

    public GameObject[,] Blocks { get; set; }
    public BuildingTag[,] Blueprint { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }

    public bool ChangeBlueprint(BuildingTag block, int x, int y)
    {
        if (IndexOutOfRange(x, y))
        {
            return false;
        }

        Blueprint[x, y] = block;
        return true;
    }

    public bool CheckBlock(BuildingTag block, int[] position)
    {
        int x = position[0];
        int y = position[1];

        return CheckBlock(block, x, y);
    }

    public bool CheckBlock(BuildingTag block, int x, int y)
    {
        if (IndexOutOfRange(x, y))
        {
            return false;
        }

        return Blueprint[x, y] == block;
    }

    public bool CheckBlock(BuildingTag block, Vector3 position)
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

    public int GetDistance(int[] source, int[] target)
    {
        int x = System.Math.Abs(source[0] - target[0]);
        int y = System.Math.Abs(source[1] - target[1]);

        return System.Math.Max(x, y);
    }

    public bool IndexOutOfRange(int x, int y)
    {
        bool checkWidth = (x < 0) || (x > Width - 1);
        bool checkHeight = (y < 0) || (y > Height - 1);

        return checkWidth || checkHeight;
    }

    public bool IsInsideRange(FOVShape shape, int maxRange,
        int[] source, int[] target)
    {
        bool check;

        if (IndexOutOfRange(target[0], target[1]))
        {
            return false;
        }

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

        Blueprint = new BuildingTag[Width, Height];
        Blocks = new GameObject[Width, Height];
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
    }
}
