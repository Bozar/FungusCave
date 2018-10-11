using UnityEngine;

public class BuildDungeon : MonoBehaviour
{
    private readonly int dungeonHeight = 17;
    private readonly int dungeonWidth = 24;
    private DungeonBlock[,] board;

    public enum DungeonBlock { Floor, Wall, Pool };

    public bool CheckTerrain(DungeonBlock block, int x, int y)
    {
        if (IndexOutOfRange(x, y))
        {
            return false;
        }

        return board[x, y] == block;
    }

    private void Awake()
    {
        board = new DungeonBlock[dungeonWidth, dungeonHeight];
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
        GameObject newObject;

        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                switch (board[x, y])
                {
                    case DungeonBlock.Wall:
                        newObject = Instantiate(wallTile);
                        newObject.transform.position = gameObject.
                            GetComponent<ConvertCoordinates>().Convert(x, y);
                        break;

                    case DungeonBlock.Pool:
                        break;

                    case DungeonBlock.Floor:
                        break;
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

        CreateDungeonObjects();
    }

    private void Start()
    {
        PlaceWallsManually();
        CreateDungeonObjects();
    }
}
