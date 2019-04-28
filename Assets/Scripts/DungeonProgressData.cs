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
            Load();
            return GetDungeonLevel(dungeonLevel);
        }

        public DungeonLevel GetDungeonLevel(string level)
        {
            return (DungeonLevel)Enum.Parse(typeof(DungeonLevel), level);
        }

        public SeedTag GetDungeonSeed()
        {
            Load();
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

        public void Load(IDataTemplate data)
        {
            DTProgress value = data as DTProgress;
            dungeonLevel = value.Progress;
        }

        public void Load()
        {
            if (dungeonLevel == null)
            {
                dungeonLevel = GetComponent<GameData>().GetStringData(node,
                    "StartLevel");
            }
        }

        public void Save(out IDataTemplate data)
        {
            data = new DTProgress { Progress = GetNextLevel().ToString() };
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            node = "Dungeon";
        }
    }
}