using Fungus.GameSystem.ObjectManager;
using System.Collections.Generic;

namespace Fungus.GameSystem.WorldBuilding
{
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
        private Stack<int[]> submergeGrid;
        private List<int[]> surround;

        public void DrawBlueprint()
        {
            countPools = random.Next(Seed, minPool, maxPool + 1);

            while (countPools > 0)
            {
                range = random.Next(Seed, minRange, maxRange + 1);
                WaterSource();
                WaterFlow();
            }
        }

        private void Awake()
        {
            submergeGrid = new Stack<int[]>();
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

            index = submergeGrid.Pop();
            nextX = index[0];
            nextY = index[1];

            surround = GetComponent<ConvertCoordinates>().SurroundCoord(
                Surround.Cardinal, nextX, nextY);

            board.ChangeBlueprint(SubObjectTag.Pool, nextX, nextY);
            countPools--;

            foreach (var grid in surround)
            {
                if (board.CheckBlock(SubObjectTag.Floor,
                    grid[0], grid[1])
                    && board.IsInsideRange(FOVShape.Rhombus, range,
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
            } while (!board.CheckBlock(SubObjectTag.Floor, startX, startY));

            startIndex = index;
            submergeGrid.Push(index);
        }
    }
}
