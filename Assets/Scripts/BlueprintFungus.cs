using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;

namespace Fungus.Actor.WorldBuilding
{
    public class BlueprintFungus : DungeonBlueprint, IIsEmptyArea
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
            countFungus = random.Next(SeedTag.Dungeon, minFungus, maxFungus + 1);
            ConvertWall2Fungus();
        }

        public bool IsEmptyArea(int x, int y, int width, int height)
        {
            for (int i = x - width; i < x + width + 1; i++)
            {
                for (int j = y - height; j < y + height + 1; j++)
                {
                    if (board.CheckBlock(SubObjectTag.Fungus, i, j))
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

            if (board.CheckBlock(SubObjectTag.Wall, x, y)
                && IsEmptyArea(x, y, 3, 3))
            {
                board.ChangeBlueprint(SubObjectTag.Fungus, x, y);
                countFungus--;
            }

            retry++;

            ConvertWall2Fungus();
        }
    }
}
