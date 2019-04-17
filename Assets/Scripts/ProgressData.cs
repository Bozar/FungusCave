using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using System;
using UnityEngine;

namespace Fungus.GameSystem
{
    public enum DungeonLevel { DL1, DL2, DL3, DL4, DL5 };

    public class ProgressData : MonoBehaviour, ISaveLoadBinary
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

        public void Load(IDataTemplate data)
        {
            DTProgress value = data as DTProgress;
            dungeonLevel = value.Progress;
        }

        public void Save(out IDataTemplate data)
        {
            data = new DTProgress { Progress = GetNextLevel() };
        }

        private string GetNextLevel()
        {
            int level = (int)GetDungeonLevel() + 1;
            int max = Enum.GetValues(typeof(DungeonLevel)).Length - 1;
            level = Math.Min(level, max);

            return ((DungeonLevel)level).ToString();
        }

        private void Start()
        {
            dungeonLevel = GetComponent<GameData>().GetStringData(
                "Dungeon", "StartLevel");
        }
    }
}
