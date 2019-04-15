using UnityEngine;

namespace Fungus.GameSystem
{
    public enum DungeonLevel { DL1, DL2, DL3, DL4, DL5 };

    public class ProgressData : MonoBehaviour
    {
        public string CurrentDungeonLevel { get; private set; }

        public int MaxActor
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Dungeon",
                    "MaxActor");
            }
        }

        public int MaxSoldier
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Dungeon",
                    "MaxSoldier");
            }
        }

        private void Start()
        {
            CurrentDungeonLevel = GetComponent<GameData>().GetStringData(
                "Dungeon", "StartLevel");
        }
    }
}
