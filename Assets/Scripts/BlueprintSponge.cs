//using UnityEngine;

public class BlueprintSponge : DungeonBlueprint

{
    private int height;
    private int[] index;
    private int maxHeight;
    private int maxWidth;
    private int minHeight;
    private int minWidth;
    private int width;
    private int x;
    private int y;

    public void Test()
    {
        SolidWall();
    }

    private void Awake()
    {
        minWidth = 4;
        minHeight = 4;
        maxWidth = 8;
        maxHeight = 8;
        index = new int[2];
    }

    private void SolidWall()
    {
        do
        {
            index = RandomIndex();
            x = index[0];
            y = index[1];

            width = random.RNG.Next(minWidth, maxWidth + 1);
            height = random.RNG.Next(minHeight, maxHeight + 1);
        } while (!IsEmptyArea(x, y, width, height));

        // Shrink the solid wall block by 1 grid so that you can always walk
        // around it.
        x += 1;
        y += 1;
        width -= 2;
        height -= 2;

        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, i, j);
            }
        }
    }
}
