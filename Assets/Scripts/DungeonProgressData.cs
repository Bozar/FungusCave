using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public enum DungeonLevel { DL1, DL2, DL3, DL4, DL5 };

    public class DungeonProgressData : MonoBehaviour,
        ISaveLoadBinary, ISaveLoadXML
    {
        private string dungeonLevel;
        private string node;

        public int MaxActor
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node, "MaxActor");
            }
        }

        public int MaxSoldier
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node, "MaxSoldier");
            }
        }

        public DungeonLevel GetDungeonLevel()
        {
            LoadXML();
            return GetDungeonLevel(dungeonLevel);
        }

        public DungeonLevel GetDungeonLevel(string level)
        {
            return (DungeonLevel)Enum.Parse(typeof(DungeonLevel), level);
        }

        public SeedTag GetDungeonSeed()
        {
            LoadXML();
            return (SeedTag)Enum.Parse(typeof(SeedTag), dungeonLevel);
        }

        public DungeonLevel GetNextLevel()
        {
            int level = (int)GetDungeonLevel() + 1;
            int max = (int)GetDungeonLevel(
                GetComponent<GameData>().GetStringData(node, "MaxLevel"));
            level = Math.Min(level, max);

            return (DungeonLevel)level;
        }

        public void GotoNextLevel()
        {
            dungeonLevel = GetNextLevel().ToString();
        }

        public void LoadBinary(IDataTemplate[] dt)
        {
            foreach (IDataTemplate d in dt)
            {
                if (d.DTTag == DataTemplateTag.Progress)
                {
                    DTDungeonProgressData value = d as DTDungeonProgressData;
                    dungeonLevel = value.Progress;
                    return;
                }
            }
        }

        public void LoadXML()
        {
            if (dungeonLevel == null)
            {
                dungeonLevel = GetComponent<GameData>().GetStringData(node,
                    "StartLevel");
            }
        }

        public void SaveBinary(Stack<IDataTemplate> dt)
        {
            DTDungeonProgressData data = new DTDungeonProgressData
            {
                Progress = GetDungeonLevel().ToString()
            };
            dt.Push(data);
        }

        public void SaveXML()
        {
            throw new NotImplementedException();
        }

        private void DungeonProgressData_LoadingDungeon(object sender,
            LoadEventArgs e)
        {
            LoadBinary(e.GameData);
        }

        private void DungeonProgressData_LoadingGame(object sender,
            LoadEventArgs e)
        {
            LoadBinary(e.GameData);
        }

        private void DungeonProgressData_SavingDungeon(object sender,
            SaveEventArgs e)
        {
            SaveBinary(e.GameData);
        }

        private void DungeonProgressData_SavingGame(object sender,
            SaveEventArgs e)
        {
            SaveBinary(e.GameData);
        }

        private void Start()
        {
            node = "Dungeon";
            GetComponent<SaveLoadGame>().SavingDungeon
                += DungeonProgressData_SavingDungeon;
            GetComponent<SaveLoadGame>().LoadingDungeon
                += DungeonProgressData_LoadingDungeon;

            GetComponent<SaveLoadGame>().SavingGame
                += DungeonProgressData_SavingGame;
            GetComponent<SaveLoadGame>().LoadingGame
                += DungeonProgressData_LoadingGame;
        }
    }
}
