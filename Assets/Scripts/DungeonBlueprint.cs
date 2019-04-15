using Fungus.GameSystem.ObjectManager;
using System;
using UnityEngine;

namespace Fungus.GameSystem.WorldBuilding
{
    public interface IIsEmptyArea
    {
        bool IsEmptyArea(int x, int y, int width, int height);
    }

    // Change the 2D array's content, which is defined in
    // Fungus.GameSystem.WorldBuilding.
    public class DungeonBlueprint : MonoBehaviour
    {
        protected DungeonBoard board;
        protected RandomNumber random;

        public SeedTag Seed
        {
            get
            {
                return (SeedTag)Enum.Parse(typeof(SeedTag),
                    GetComponent<ProgressData>().CurrentDungeonLevel);
            }
        }

        public void DrawManually()
        {
            board.ChangeBlueprint(SubObjectTag.Wall, 4, 4);
            board.ChangeBlueprint(SubObjectTag.Wall, 5, 5);
            board.ChangeBlueprint(SubObjectTag.Wall, 6, 6);

            board.ChangeBlueprint(SubObjectTag.Wall,
                board.Width - 1, board.Height - 1);
            board.ChangeBlueprint(SubObjectTag.Wall,
                board.Width, board.Height - 1);

            board.ChangeBlueprint(SubObjectTag.Pool, 8, 8);
            board.ChangeBlueprint(SubObjectTag.Pool, 8, 9);
            board.ChangeBlueprint(SubObjectTag.Pool, 9, 9);
        }

        public void DrawRandomly()
        {
            int wall = 5;
            int count = 0;
            int[] index;
            int x;
            int y;

            while (count < wall)
            {
                index = RandomIndex();
                x = index[0];
                y = index[1];

                if (board.CheckBlock(SubObjectTag.Floor, x, y))
                {
                    board.ChangeBlueprint(SubObjectTag.Wall, x, y);
                    count++;
                }
            }
        }

        public int[] RandomIndex()
        {
            int[] index;
            int x;
            int y;

            x = random.Next(Seed, 0, board.Width);
            y = random.Next(Seed, 0, board.Height);
            index = new int[] { x, y };

            return index;
        }

        private void Start()
        {
            board = GetComponent<DungeonBoard>();
            random = GetComponent<RandomNumber>();
        }
    }
}
