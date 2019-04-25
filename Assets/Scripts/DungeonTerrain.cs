using Fungus.GameSystem.Data;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class DungeonTerrain : MonoBehaviour, IInitialize
    {
        private ActorBoard actor;
        private bool[,] board;
        private DungeonBoard dungeon;

        public void ChangeStatus(bool isPassable, int x, int y)
        {
            if (dungeon.IndexOutOfRange(x, y))
            {
                return;
            }
            board[x, y] = isPassable;
            return;
        }

        public void Initialize()
        {
            for (int i = 0; i < dungeon.Width; i++)
            {
                for (int j = 0; j < dungeon.Height; j++)
                {
                    if (!actor.HasActor(i, j)
                        && (dungeon.CheckBlock(SubObjectTag.Floor, i, j)
                        || dungeon.CheckBlock(SubObjectTag.Pool, i, j)))
                    {
                        board[i, j] = true;
                    }
                }
            }
        }

        public bool IsPassable(int x, int y)
        {
            if (dungeon.IndexOutOfRange(x, y))
            {
                return false;
            }
            return board[x, y];
        }

        private void Start()
        {
            dungeon = GetComponent<DungeonBoard>();
            actor = GetComponent<ActorBoard>();

            // Default value: false.
            board = new bool[dungeon.Width, dungeon.Height];
        }
    }
}
