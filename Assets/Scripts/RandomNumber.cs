using System;
using System.Collections.Generic;
using UnityEngine;

public enum SeedTag
{
    DISCARDABLE,
    Root, Dungeon,

    PERSISTENT,
    AutoExplore
}

public class RandomNumber : MonoBehaviour
{
    private Dictionary<SeedTag, Queue<int>> intQueueDict;
    private int maxQueueLength;
    private int minQueueLength;
    private Dictionary<SeedTag, System.Random> rngDict;
    private Dictionary<SeedTag, int> seedDict;

    public int RootSeed { get; private set; }

    public void InitializeSeed()
    {
        List<SeedTag> tagList;
        RootSeed = FindObjects.GameLogic.GetComponent<SaveLoad>().SaveFile.Seed;

        if (RootSeed == 0)
        {
            RootSeed = RandomInteger(true);
        }

        rngDict[SeedTag.Root] = new System.Random(RootSeed);
        tagList = new List<SeedTag>(seedDict.Keys);

        foreach (var tag in tagList)
        {
            if (tag != SeedTag.Root)
            {
                seedDict[tag] = RandomInteger(false, rngDict[SeedTag.Root]);
            }
            else
            {
                seedDict[SeedTag.Root] = RootSeed;
            }
        }
    }

    public double Next(SeedTag tag)
    {
        CheckErrors(tag);
        InitializeRNG(tag);

        if (IsPersistent(tag))
        {
            return DequeDouble(tag);
        }

        return rngDict[tag].NextDouble();
    }

    public int Next(SeedTag tag, int min, int max)
    {
        CheckErrors(tag, min, max);
        InitializeRNG(tag);

        if (IsPersistent(tag))
        {
            return DequeInt(tag, min, max);
        }

        return rngDict[tag].Next(min, max);
    }

    private void Awake()
    {
        minQueueLength = 100;
        maxQueueLength = 1000;

        seedDict = new Dictionary<SeedTag, int>();
        rngDict = new Dictionary<SeedTag, System.Random>();
        intQueueDict = new Dictionary<SeedTag, Queue<int>>();

        foreach (var tag in Enum.GetValues(typeof(SeedTag)))
        {
            if (IsInvalid((SeedTag)tag))
            {
                continue;
            }

            seedDict.Add((SeedTag)tag, -1);

            if (IsPersistent((SeedTag)tag))
            {
                intQueueDict.Add((SeedTag)tag, new Queue<int>());
            }
            else
            {
                rngDict.Add((SeedTag)tag, null);
            }
        }
    }

    private void CheckErrors(SeedTag tag, int min = 0, int max = 0)
    {
        if (IsInvalid(tag))
        {
            throw new Exception("Invalid tag: " + tag);
        }

        if (max < min)
        {
            throw new Exception("Invalid range.");
        }
    }

    private double DequeDouble(SeedTag tag)
    {
        double result = intQueueDict[tag].Dequeue() / Math.Pow(10, 9);

        if (intQueueDict[tag].Count < minQueueLength)
        {
            InitializeRNG(tag);
        }

        return result;
    }

    private int DequeInt(SeedTag tag, int min, int max)
    {
        int result;
        int delta = max - min;

        result = (int)(min + Math.Floor(delta * DequeDouble(tag)));

        return result;
    }

    private void InitializeRNG(SeedTag tag)
    {
        System.Random tempRNG;

        if (IsInvalid(tag))
        {
            return;
        }

        if (IsPersistent(tag))
        {
            tempRNG = new System.Random(seedDict[tag]);

            for (int i = 0; i < maxQueueLength; i++)
            {
                intQueueDict[tag].Enqueue(RandomInteger(false, tempRNG));
            }

            seedDict[tag] = RandomInteger(false, tempRNG);
        }
        else
        {
            if (rngDict[tag] == null)
            {
                rngDict[tag] = new System.Random(seedDict[tag]);
            }
        }
    }

    private bool IsInvalid(SeedTag tag)
    {
        bool isDiscardable = tag.Equals(SeedTag.DISCARDABLE);
        bool isPersistent = tag.Equals(SeedTag.PERSISTENT);

        return isDiscardable || isPersistent;
    }

    private bool IsPersistent(SeedTag tag)
    {
        return tag.CompareTo(SeedTag.PERSISTENT) > 0;
    }

    private int RandomInteger(bool mustHave9Digits)
    {
        return RandomInteger(mustHave9Digits, new System.Random());
    }

    private int RandomInteger(bool mustHave9Digits, System.Random rng)
    {
        double doubleSeed;

        do
        {
            doubleSeed = rng.NextDouble();
        }
        while (mustHave9Digits && (doubleSeed < 0.1));

        return (int)(doubleSeed * Math.Pow(10, 9));
    }
}
