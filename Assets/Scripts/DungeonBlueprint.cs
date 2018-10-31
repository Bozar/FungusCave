using UnityEngine;

// Change the 2D array's content, which is defined in DungeonBoard.
public class DungeonBlueprint : MonoBehaviour
{
    protected DungeonBoard board;
    protected RandomNumber random;

    public interface IIsEmptyArea
    {
        bool IsEmptyArea(int x, int y, int width, int height);
    }

    public void DrawManually()
    {
        board.ChangeBlueprint(DungeonBlock.Wall, 4, 4);
        board.ChangeBlueprint(DungeonBlock.Wall, 5, 5);
        board.ChangeBlueprint(DungeonBlock.Wall, 6, 6);

        board.ChangeBlueprint(DungeonBlock.Wall,
            board.Width - 1, board.Height - 1);
        board.ChangeBlueprint(DungeonBlock.Wall,
            board.Width, board.Height - 1);

        board.ChangeBlueprint(DungeonBlock.Pool, 8, 8);
        board.ChangeBlueprint(DungeonBlock.Pool, 8, 9);
        board.ChangeBlueprint(DungeonBlock.Pool, 9, 9);
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

            if (board.CheckBlock(DungeonBlock.Floor, x, y))
            {
                board.ChangeBlueprint(DungeonBlock.Wall, x, y);
                count++;
            }
        }
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
