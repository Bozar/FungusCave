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
    private Dictionary<SeedTag, System.Random> rngDict;
    private Dictionary<SeedTag, int> seedDict;
    private List<SeedTag> tagList;

    public System.Random RNG { get; private set; }
    public int RootSeed { get; private set; }

    public void InitializeSeed()
    {
        RootSeed = FindObjects.GameLogic.GetComponent<SaveLoad>().SaveFile.Seed;

        if (RootSeed == 0)
        {
            RootSeed = RandomInteger(true);
        }

        RNG = new System.Random(RootSeed);
        tagList = new List<SeedTag>(seedDict.Keys);

        foreach (var tag in tagList)
        {
            if (tag != SeedTag.Root)
            {
                seedDict[tag] = RandomInteger(false, RNG);
            }
            else
            {
                seedDict[SeedTag.Root] = RootSeed;
            }
        }

        foreach (var tagSeed in seedDict)
        {
            Debug.Log(tagSeed);
        }

        foreach (var tagRNG in rngDict)
        {
            Debug.Log(tagRNG);
        }

        foreach (var tagRNG in intQueueDict)
        {
            Debug.Log(tagRNG);
        }

        Debug.Log(Next(SeedTag.Dungeon));
        Debug.Log(Next(SeedTag.AutoExplore));
        Debug.Log(Next(SeedTag.Dungeon, 1, 11));
        //Debug.Log(Next(SeedTag.PERSISTENT));
    }

    public double Next(SeedTag tag)
    {
        if (IsInvalid(tag))
        {
            throw new Exception("Invalid tag: " + tag);
        }

        InitializeRNG(tag);

        if (IsPersistent(tag))
        {
            return -42;
        }

        if (rngDict[tag] == null)
        {
            rngDict[tag] = new System.Random(seedDict[tag]);
        }

        return rngDict[tag].NextDouble();
    }

    public int Next(SeedTag tag, int min, int max)
    {
        if (IsInvalid(tag))
        {
            throw new Exception("Invalid tag: " + tag);
        }

        InitializeRNG(tag);

        if (IsPersistent(tag))
        {
            return -42;
        }

        return rngDict[tag].Next(min, max);
    }

    public int Next(SeedTag tag, int max)
    {
        return Next(tag, 0, max);
    }

    private void Awake()
    {
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

    private void InitializeRNG(SeedTag tag)
    {
        if (IsInvalid(tag))
        {
            return;
        }

        if (IsPersistent(tag))
        {
            Debug.Log("WIP");
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
