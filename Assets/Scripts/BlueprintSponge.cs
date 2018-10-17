using System.Collections.Generic;
using UnityEngine;

public class BlueprintSponge : DungeonBlueprint, DungeonBlueprint.IIsEmptyArea
{
    private int countWalls;
    private int digX;
    private int digY;
    private int height;
    private int[] index;
    private int maxHeight;
    private int maxWidth;
    private int minHeight;
    private int minWidth;
    private int startX;
    private int startY;
    private int width;

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

    public void Test()
    {
        for (int i = 0; i < 999; i++)
        {
            if (SolidWall())
            {
                DigTunnel();
            }
        }

        Debug.Log("Wall / Area: "
            + (int)((float)countWalls / (board.Width * board.Height) * 100));
    }

    private void Awake()
    {
        minWidth = 4;
        minHeight = 4;
        maxWidth = 7;
        maxHeight = 7;
        countWalls = 0;
        index = new int[2];
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
        bool maxSize;

        index = RandomIndex();
        startX = index[0];
        startY = index[1];

        width = random.RNG.Next(minWidth, maxWidth + 1);
        height = random.RNG.Next(minHeight, maxHeight + 1);

        // If a block is of maxWidth & maxHeight, it looks too big.
        maxSize = width == maxWidth && height == maxHeight;

        if (maxSize || !IsEmptyArea(startX, startY, width, height))
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
