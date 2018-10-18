using System.Collections.Generic;
using UnityEngine;

public class BlueprintSponge : DungeonBlueprint, DungeonBlueprint.IIsEmptyArea
{
    private int countWalls;
    private int digX;
    private int digY;
    private int finalWall;
    private int height;
    private int[] index;
    private int maxSize;
    private int maxWall;
    private int minSize;
    private int minWall;
    private int startX;
    private int startY;
    private int width;

    public void DrawBlueprint()
    {
        finalWall = random.RNG.Next(minWall, maxWall + 1);

        Debug.Log("Final: " + finalWall);

        for (int i = 0; i < 9999; i++)
        {
            if (countWalls > finalWall)
            {
                break;
            }

            // Build a block of solid walls.
            if (SolidWall())
            {
                // Do not dig through a 2*2 block.
                if (System.Math.Max(width, height) > 2)
                {
                    DigTunnel();
                }

                // Dig another tunnel through a big block.
                if (System.Math.Max(width, height) == maxSize - 2)
                {
                    DigTunnel();
                }

                // Dig one or more corners of a "thick" block.
                if (System.Math.Min(width, height) > 2)
                {
                    DigCorner();
                }
            }
        }

        Debug.Log("Wall: " + countWalls);
    }

    public bool IsEmptyArea(int x, int y, int width, int height)
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

        // Do not generate a looong bar. Also note that the acutal wall block is
        // shrinked by 1 gird in four directions.
        checkSize = System.Math.Min(width - 2, height - 2) * 3
            > System.Math.Max(width - 2, height - 2);

        return checkX && checkY && checkFloor && checkSize;
    }

    private void Awake()
    {
        minSize = 3;
        maxSize = 7;
        minWall = 85;
        maxWall = 95;
        countWalls = 0;
        index = new int[2];
    }

    private void DigCorner()
    {
        int[] sw = new int[] { startX, startY };
        int[] se = new int[] { startX + width - 1, startY };
        int[] nw = new int[] { startX, startY + height - 1 };
        int[] ne = new int[] { startX + width - 1, startY + height - 1 };
        int[][] cornerIndex = new int[][] { sw, se, nw, ne };

        foreach (var index in cornerIndex)
        {
            if (random.RNG.Next(0, 3) < 2)
            {
                board.ChangeBlock(DungeonBoard.DungeonBlock.Floor,
                    index[0], index[1]);
                countWalls--;
            }
        }
    }

    private void DigTunnel()
    {
        // Dig from bottom to top. If the block is a square, dig a tunnel from
        // left to right because the dungeon screen is a horizontal rectangle.
        if (width > height)
        {
            digX = random.RNG.Next(startX, startX + width);
            // Try to move starting point away from corner.
            digX = NextStep(digX, startX, startX + width - 1);
            digY = startY;

            while (digX > -1 && digY > -1)
            {
                board.ChangeBlock(DungeonBoard.DungeonBlock.Floor, digX, digY);
                countWalls--;

                digX = NextStep(digX, startX, startX + width - 1);
                digY = NextStep(digY, startY, startY + height - 1, 1);
            }
        }
        // Dig from left to right.
        else
        {
            digX = startX;
            digY = random.RNG.Next(startY, startY + height);
            digY = NextStep(digY, startY, startY + height - 1);

            while (digX > -1 && digY > -1)
            {
                board.ChangeBlock(DungeonBoard.DungeonBlock.Floor, digX, digY);
                countWalls--;

                digX = NextStep(digX, startX, startX + width - 1, 1);
                digY = NextStep(digY, startY, startY + height - 1);
            }
        }
    }

    private int NextStep(int current, int min, int max, int step)
    {
        int next = current + step;

        if (next >= min && next <= max)
        {
            return next;
        }
        return -1;
    }

    private int NextStep(int current, int min, int max)
    {
        int next;
        int stepIndex;
        int retry = 0;
        List<int> step = new List<int> { -1, 0, 1 };

        while (step.Count > 0)
        {
            stepIndex = random.RNG.Next(0, step.Count);
            next = current + step[stepIndex];

            if (next > min && next < max)
            {
                return next;
            }
            else if (next < min || next > max)
            {
                step.RemoveAt(stepIndex);
            }
            // The next grid touches the border: (next == min || next == max).
            // Try to find another grid that is inside the block if possible.
            else
            {
                if (retry > 5)
                {
                    return next;
                }
                retry++;
            }
        }

        return -1;
    }

    private bool SolidWall()
    {
        bool tooBig;

        index = RandomIndex();
        startX = index[0];
        startY = index[1];

        width = random.RNG.Next(minSize, maxSize + 1);
        height = random.RNG.Next(minSize, maxSize + 1);

        // If a block is of maxWidth & maxHeight, it looks too big.
        tooBig = System.Math.Min(width, height) == maxSize;

        if (tooBig || !IsEmptyArea(startX, startY, width, height))
        {
            return false;
        }

        // Shrink the solid wall block by 1 grid so that you can always walk
        // around it.
        startX += 1;
        startY += 1;
        width -= 2;
        height -= 2;

        for (int i = startX; i < startX + width; i++)
        {
            for (int j = startY; j < startY + height; j++)
            {
                board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, i, j);
                countWalls++;
            }
        }

        return true;
    }
}
