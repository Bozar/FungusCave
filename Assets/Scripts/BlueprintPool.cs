using System.Collections;

public class BlueprintPool : DungeonBlueprint
{
    private int[] index;
    private int maxRange;
    private int nextX;
    private int nextY;
    private int startX;
    private int startY;
    private Stack submergeGrid;
    private int[][] surround;

    public void DrawBlueprint()
    {
        maxRange = 3;
        WaterSource();
        WaterFlow();
    }

    private void Awake()
    {
        submergeGrid = new Stack();
    }

    private bool InsideRange(int x, int y)
    {
        bool check = System.Math.Abs(x - startX) + System.Math.Abs(y - startY)
            <= maxRange;

        return check;
    }

    private void WaterFlow()
    {
        if (submergeGrid.Count == 0)
        {
            return;
        }

        index = (int[])submergeGrid.Pop();
        nextX = index[0];
        nextY = index[1];

        surround = new int[][] {
            new int[] { nextX - 1, nextY},
            new int[] { nextX + 1, nextY},
            new int[] { nextX , nextY-1},
            new int[] { nextX , nextY+1}
        };

        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, nextX, nextY);

        foreach (var grid in surround)
        {
            if (board.CheckTerrain(DungeonBoard.DungeonBlock.Floor,
                grid[0], grid[1])
                && InsideRange(grid[0], grid[1]))
            {
                submergeGrid.Push(grid);
            }
        }

        WaterFlow();
    }

    private void WaterSource()
    {
        do
        {
            index = RandomIndex();
            startX = index[0];
            startY = index[1];
        } while (!board.CheckTerrain(DungeonBoard.DungeonBlock.Floor,
        startX, startY));

        submergeGrid.Push(index);
    }
}
