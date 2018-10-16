using UnityEngine;

// Change the 2D array's content, which is defined in DungeonBoard.
public class DungeonBlueprint : MonoBehaviour
{
    protected DungeonBoard board;
    protected RandomNumber random;

    public void DrawManually()
    {
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, 4, 4);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, 5, 5);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, 6, 6);

        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall,
            board.Width - 1, board.Height - 1);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall,
            board.Width, board.Height - 1);

        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, 8, 8);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, 8, 9);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, 9, 9);
    }

    public void DrawRandomly()
    {
        int wall = 5;
        int count = 0;
        int[] index;
        int x;
        int y;

        while (count < wall)
        {
            index = RandomIndex();
            x = index[0];
            y = index[1];

            if (board.CheckTerrain(DungeonBoard.DungeonBlock.Floor, x, y))
            {
                board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, x, y);
                count++;
            }
        }
    }

    protected bool IsEmptyArea(int x, int y, int width, int height)
    {
        bool checkX;
        bool checkY;
        bool checkSize;
        bool checkFloor = true;

        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                if (!board.CheckTerrain(DungeonBoard.DungeonBlock.Floor, i, j))
                {
                    checkFloor = false;
                    break;
                }
            }
        }

        checkX = x >= 0 && x + width <= board.Width;
        checkY = y >= 0 && y + height <= board.Height;

        checkSize = Mathf.Min(width - 2, height - 2) * 3
            > Mathf.Max(width - 2, height - 2);

        return checkX && checkY && checkFloor && checkSize;
    }

    protected int[] RandomIndex()
    {
        int[] index;
        int x;
        int y;

        x = random.RNG.Next(0, board.Width);
        y = random.RNG.Next(0, board.Height);
        index = new int[] { x, y };

        return index;
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        random = FindObjects.GameLogic.GetComponent<RandomNumber>();
    }
}
