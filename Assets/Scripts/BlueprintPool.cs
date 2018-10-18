using System.Collections;

public class BlueprintPool : DungeonBlueprint
{
    private int countPool;
    private int[] index;
    private int maxPool;
    private int maxRange;
    private int minPool;
    private int minRange;
    private int nextX;
    private int nextY;
    private int range;
    private int[] startIndex;
    private int startX;
    private int startY;
    private Stack submergeGrid;
    private int[][] surround;

    public void DrawBlueprint()
    {
        countPool = random.RNG.Next(minPool, maxPool + 1);

        for (int i = 0; i < countPool; i++)
        {
            range = random.RNG.Next(minRange, maxRange + 1);
            WaterSource();
            WaterFlow();
        }
    }

    private void Awake()
    {
        submergeGrid = new Stack();
        minRange = 1;
        maxRange = 3;
        minPool = 6;
        maxPool = 10;
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
                && board.IsInsideFOV(DungeonBoard.FOVShape.Rhombus, range,
                startIndex, grid))
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

        startIndex = index;
        submergeGrid.Push(index);
    }
}
