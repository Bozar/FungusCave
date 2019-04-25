using Fungus.GameSystem.Data;

namespace Fungus.GameSystem.WorldBuilding
{
    public class BlueprintFungus : DungeonBlueprint
    {
        private int countFungus;
        private int maxFungus;
        private int minFungus;
        private int retry;

        public void DrawBlueprint()
        {
            countFungus = random.Next(Seed, minFungus, maxFungus + 1);
            ConvertWall2Fungus();
        }

        private void Awake()
        {
            retry = 0;
            minFungus = 30;
            maxFungus = 40;
        }

        private void ConvertWall2Fungus()
        {
            if ((countFungus < 1) || (retry > 999))
            {
                return;
            }

            int[] index = RandomIndex();
            int x = index[0];
            int y = index[1];

            if (board.CheckBlock(SubObjectTag.Wall, x, y))
            {
                board.ChangeBlueprint(SubObjectTag.Fungus, x, y);
                countFungus--;
            }
            retry++;

            ConvertWall2Fungus();
        }
    }
}
