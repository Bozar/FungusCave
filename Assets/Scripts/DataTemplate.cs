using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public enum DataTemplateTag
    {
        INVALID, Seed, Progress, Spawn, Dungeon, Actor
    };

    public interface IDataTemplate
    {
        DataTemplateTag DTTag { get; }
    }

    public class DataTemplate : MonoBehaviour
    {
    }

    [Serializable]
    public class DTProgress : IDataTemplate
    {
        public string Progress;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Progress; } }
    }

    [Serializable]
    public class DTSeed : IDataTemplate
    {
        public Dictionary<SeedTag, int> SeedInt;
        public Dictionary<SeedTag, Queue<int>> SeedIntQueue;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Seed; } }
    }

    [Serializable]
    public class DTSpawn : IDataTemplate
    {
        public int Count;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Spawn; } }
    }
}
