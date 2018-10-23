public class BlueprintFungus : DungeonBlueprint, DungeonBlueprint.IIsEmptyArea
{
    private int countFungus;
    private int[] index;
    private int maxFungus;
    private int minFungus;
    private int retry;
    private int x;
    private int y;

    public void DrawBlueprint()
    {
        countFungus = random.RNG.Next(minFungus, maxFungus + 1);
        ConvertWall2Fungus();
    }

    public bool IsEmptyArea(int x, int y, int width, int height)
    {
        for (int i = x - width; i < x + width + 1; i++)
        {
            for (int j = y - height; j < y + height + 1; j++)
            {
                if (board.CheckBlock(DungeonBoard.DungeonBlock.Fungus, i, j))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void Awake()
    {
        retry = 0;
        minFungus = 5;
        maxFungus = 10;
    }

    private void ConvertWall2Fungus()
    {
        if ((countFungus < 1) || (retry > 999))
        {
            return;
        }

        index = RandomIndex();
        x = index[0];
        y = index[1];

        if (board.CheckBlock(DungeonBoard.DungeonBlock.Wall, x, y)
            && IsEmptyArea(x, y, 3, 3))
        {
            board.ChangeBlueprint(DungeonBoard.DungeonBlock.Fungus, x, y);
            countFungus--;
        }

        retry++;

        ConvertWall2Fungus();
    }
}
