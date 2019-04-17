using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public enum DataTemplateTag { INVALID, Dungeon, Seed, Actor };

    public interface IDataTemplate
    {
        DataTemplateTag DataTag { get; }
    }

    public class DataTemplate : MonoBehaviour
    {
    }

    [Serializable]
    public class DTSeed : IDataTemplate
    {
        public Dictionary<SeedTag, int> SeedInt;
        public Dictionary<SeedTag, Queue<int>> SeedIntQueue;

        public DataTemplateTag DataTag { get { return DataTemplateTag.Seed; } }
    }
}
