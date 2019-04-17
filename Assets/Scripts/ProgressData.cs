using Fungus.GameSystem.Data;
using System;
using UnityEngine;

namespace Fungus.GameSystem
{
    public enum DungeonLevel { DL1, DL2, DL3, DL4, DL5 };

    public class ProgressData : MonoBehaviour
    {
        private string dungeonLevel;

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

        public DungeonLevel GetDungeonLevel()
        {
            return (DungeonLevel)Enum.Parse(typeof(DungeonLevel), dungeonLevel);
        }

        public SeedTag GetDungeonSeed()
        {
            return (SeedTag)Enum.Parse(typeof(SeedTag), dungeonLevel);
        }

        private void Start()
        {
            dungeonLevel = GetComponent<GameData>().GetStringData(
                "Dungeon", "StartLevel");
        }
    }
}
