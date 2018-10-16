using System.Collections.Generic;

public class BlueprintSponge : DungeonBlueprint, DungeonBlueprint.IIsEmptyArea
{
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

        // Do not generate a looooog bar.
        checkSize = System.Math.Min(width - 2, height - 2) * 3
            > System.Math.Max(width - 2, height - 2);

        return checkX && checkY && checkFloor && checkSize;
    }

    public void Test()
    {
        SolidWall();
        DigTunnel();
    }

    private void Awake()
    {
        minWidth = 4;
        minHeight = 4;
        maxWidth = 7;
        maxHeight = 7;
        index = new int[2];
    }

    private void DigTunnel()
    {
        // From bottom to top.
        if (width >= height)
        {
            digX = random.RNG.Next(startX, startX + width);
            digY = startY;

            while (digX > -1 && digY > -1)
            {
                board.ChangeBlock(DungeonBoard.DungeonBlock.Floor, digX, digY);

                digX = NextStep(digX, startX, startX + width - 1);
                digY = NextStep(digY, startY, startY + height - 1, 1);
            }
        }
        // From left to right.
        else
        {
            digX = startX;
            digY = random.RNG.Next(startY, startY + height);

            while (digX > -1 && digY > -1)
            {
                board.ChangeBlock(DungeonBoard.DungeonBlock.Floor, digX, digY);

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
        List<int> step = new List<int> { -1, 0, 1 };

        while (step.Count > 0)
        {
            stepIndex = random.RNG.Next(0, step.Count);
            next = current + step[stepIndex];

            if (next >= min && next <= max)
            {
                return next;
            }

            step.RemoveAt(stepIndex);
        }

        return -1;
    }

    private void SolidWall()
    {
        bool maxSize;

        do
        {
            index = RandomIndex();
            startX = index[0];
            startY = index[1];

            width = random.RNG.Next(minWidth, maxWidth + 1);
            height = random.RNG.Next(minHeight, maxHeight + 1);

            // If a block is of maxWidth & maxHeight, it looks too big.
            maxSize = width == maxWidth && height == maxHeight;
        } while (
        !IsEmptyArea(startX, startY, width, height)
        || maxSize);

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
            }
        }
    }
}
