using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class PassableTerrian : MonoBehaviour
    {
        private bool[,] board;
        private DungeonBoard dungeon;

        public void ChangeStatus(bool isPassable)
        {
        }

        public void Initialize()
        {
        }

        public bool IsPassable()
        {
            return false;
        }

        private void Start()
        {
            dungeon = GetComponent<DungeonBoard>();

            // Default value: false.
            board = new bool[dungeon.Width, dungeon.Height];
        }
    }
}
