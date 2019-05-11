using Fungus.Actor;
using Fungus.Actor.FOV;
using Fungus.GameSystem.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public enum DataTemplateTag
    {
        INVALID, Seed, Progress, Kill, Schedule, Spawn, Dungeon, Actor
    };

    public interface IDataTemplate
    {
        DataTemplateTag DTTag { get; }
    }

    public class DataTemplate : MonoBehaviour
    {
    }

    [Serializable]
    public class DTActor : IDataTemplate
    {
        public SubObjectTag ActorTag;
        public int Energy;
        public FOVStatus[,] FovBoard;
        public int HP;
        public Dictionary<InfectionTag, int> Infection;
        public int[] Position;
        public int Potion;
        public Dictionary<PowerSlotTag, PowerTag> Power;
        public int Stress;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Actor; } }
    }

    [Serializable]
    public class DTDungeonBoard : IDataTemplate
    {
        public SubObjectTag[,] Blueprint;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Dungeon; } }
    }

    [Serializable]
    public class DTDungeonProgress : IDataTemplate
    {
        public int KillCount;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Kill; } }
    }

    [Serializable]
    public class DTDungeonProgressData : IDataTemplate
    {
        public string Progress;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Progress; } }
    }

    [Serializable]
    public class DTRandomNumber : IDataTemplate
    {
        public Dictionary<SeedTag, int> SeedInt;
        public Dictionary<SeedTag, Queue<int>> SeedIntQueue;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Seed; } }
    }

    [Serializable]
    public class DTSchedulingSystem : IDataTemplate
    {
        public DTActor[] Actors;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Schedule; } }
    }

    [Serializable]
    public class DTSpawnBeetle : IDataTemplate
    {
        public int Count;
        public bool NotWarned;

        public DataTemplateTag DTTag { get { return DataTemplateTag.Spawn; } }
    }
}
