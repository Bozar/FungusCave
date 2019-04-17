using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem
{
    public enum SeedTag
    {
        // Root seed
        Root,

        // Leaf seeds
        DL1, DL2, DL3, DL4, DL5,

        AutoExplore, Infection, NPCAction
    }

    public class RandomNumber : MonoBehaviour,
        ISaveLoadXML, ISaveLoadBinary, IInitialize
    {
        private int maxQueueLength;
        private int minQueueLength;
        private System.Random rootRNG;

        // Seeds are stored in seedInt.
        private Dictionary<SeedTag, int> seedInt;

        // Random integers are stored in seedIntQueue.
        private Dictionary<SeedTag, Queue<int>> seedIntQueue;

        public int RootSeed { get; private set; }

        public void Initialize()
        {
            // Load a root seed from an external source.
            Load();
            // Use the root seed to create an RNG.
            InitializeRootRNG();
            // Use the RNG to generate leaf seeds for the first time.
            InitializeSeedInt();
        }

        public void Load(IDataTemplate data)
        {
            DTSeed value = data as DTSeed;

            seedInt = value.SeedInt;
            seedIntQueue = value.SeedIntQueue;
            RootSeed = seedInt[SeedTag.Root];
        }

        public void Load()
        {
            RootSeed = GetComponent<GameSetting>().Seed;
        }

        public double Next(SeedTag tag)
        {
            if (IsRoot(tag))
            {
                return rootRNG.NextDouble();
            }

            ReplenishSeedIntQueue(tag);
            return DequeDouble(tag);
        }

        public int Next(SeedTag tag, int min, int max)
        {
            CheckError(min, max);

            if (IsRoot(tag))
            {
                return rootRNG.Next(min, max);
            }

            ReplenishSeedIntQueue(tag);
            return DequeInt(tag, min, max);
        }

        public void Save(out IDataTemplate data)
        {
            data = new DTSeed
            {
                SeedInt = seedInt,
                SeedIntQueue = seedIntQueue
            };
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            minQueueLength = 100;
            maxQueueLength = 1000;

            seedInt = new Dictionary<SeedTag, int>();
            seedIntQueue = new Dictionary<SeedTag, Queue<int>>();
        }

        private void CheckError(int min, int max)
        {
            if (max < min)
            {
                throw new Exception("Invalid range.");
            }
        }

        private double DequeDouble(SeedTag tag)
        {
            double result = seedIntQueue[tag].Dequeue() / Math.Pow(10, 9);

            return result;
        }

        private int DequeInt(SeedTag tag, int min, int max)
        {
            int result;
            int delta = max - min;

            result = (int)(min + Math.Floor(delta * DequeDouble(tag)));

            return result;
        }

        private void InitializeRootRNG()
        {
            if (RootSeed == 0)
            {
                RootSeed = RandomInteger(true);
            }
            rootRNG = new System.Random(RootSeed);
        }

        private void InitializeSeedInt()
        {
            foreach (SeedTag seed in Enum.GetValues(typeof(SeedTag)))
            {
                if (IsRoot(seed))
                {
                    seedInt.Add(seed, RootSeed);
                }
                else
                {
                    seedInt.Add(seed, RandomInteger(false, rootRNG));
                }
            }
        }

        private bool IsRoot(SeedTag tag)
        {
            return tag == SeedTag.Root;
        }

        private int RandomInteger(bool mustHave9Digits)
        {
            return RandomInteger(mustHave9Digits, new System.Random());
        }

        private int RandomInteger(bool mustHave9Digits, System.Random rng)
        {
            double result;

            do
            {
                result = rng.NextDouble();
            }
            while (mustHave9Digits && (result < 0.1));

            return (int)(result * Math.Pow(10, 9));
        }

        private void ReplenishSeedIntQueue(SeedTag seed)
        {
            if (IsRoot(seed))
            {
                return;
            }

            // Random numbers from leaf seeds are pregenerated and stored in
            // seedIntQueue. Generate more numbers and a new leaf seed when the
            // queue is almost consumed.
            if (seedIntQueue.TryGetValue(seed, out Queue<int> intQueue))
            {
                if (intQueue.Count > minQueueLength)
                {
                    return;
                }
            }
            else
            {
                seedIntQueue.Add(seed, new Queue<int>());
            }

            System.Random rng = new System.Random(seedInt[seed]);
            for (int i = 0; i < maxQueueLength; i++)
            {
                seedIntQueue[seed].Enqueue(RandomInteger(false, rng));
            }
            seedInt[seed] = RandomInteger(false, rng);
        }
    }
}
