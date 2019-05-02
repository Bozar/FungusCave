﻿using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using System;
using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public enum DungeonLevel { DL1, DL2, DL3, DL4, DL5 };

    public class DungeonProgressData : MonoBehaviour, ISaveLoadBinary,
        ISaveLoadXML
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

        public void LoadBinary(IDataTemplate data)
        {
            DTProgress value = data as DTProgress;
            dungeonLevel = value.Progress;
        }

        public void LoadXML()
        {
            if (dungeonLevel == null)
            {
                dungeonLevel = GetComponent<GameData>().GetStringData(node,
                    "StartLevel");
            }
        }

        public void SaveBinary(out IDataTemplate data)
        {
            data = new DTProgress { Progress = GetNextLevel().ToString() };
        }

        public void SaveXML()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            node = "Dungeon";
        }
    }
}
