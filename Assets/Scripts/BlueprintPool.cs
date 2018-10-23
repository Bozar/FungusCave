using System.Collections;
using System.Collections.Generic;

public class BlueprintPool : DungeonBlueprint
{
    private int countPools;
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
    private List<int[]> surround;

    public void DrawBlueprint()
    {
        countPools = random.RNG.Next(minPool, maxPool + 1);

        while (countPools > 0)
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
        minPool = 90;
        maxPool = 110;
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

        surround = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .SurroundCoord(ConvertCoordinates.Surround.Horizonal, nextX, nextY);

        board.ChangeBlueprint(DungeonBoard.DungeonBlock.Pool, nextX, nextY);
        countPools--;

        foreach (var grid in surround)
        {
            if (board.CheckBlock(DungeonBoard.DungeonBlock.Floor,
                grid[0], grid[1])
                && board.IsInsideRange(DungeonBoard.FOVShape.Rhombus, range,
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
        } while (!board.CheckBlock(DungeonBoard.DungeonBlock.Floor,
        startX, startY));

        startIndex = index;
        submergeGrid.Push(index);
    }
}
