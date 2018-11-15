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
    private Dictionary<SeedTag, int> seedDict;
    private List<SeedTag> tagList;

    public System.Random RNG { get; private set; }
    public int RootSeed { get; private set; }

    public void InitializeSeed()
    {
        RootSeed = FindObjects.GameLogic.GetComponent<SaveLoad>().SaveFile.Seed;

        if (RootSeed == 0)
        {
            RootSeed = RandomSeed();
        }

        RNG = new System.Random(RootSeed);
        tagList = new List<SeedTag>(seedDict.Keys);

        foreach (var tag in tagList)
        {
            if (tag != SeedTag.Root)
            {
                seedDict[tag] = RandomSeed(RNG);
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
    }

    private void Awake()
    {
        seedDict = new Dictionary<SeedTag, int>();

        foreach (var tag in Enum.GetValues(typeof(SeedTag)))
        {
            if (!tag.Equals(SeedTag.DISCARDABLE)
                && !tag.Equals(SeedTag.PERSISTENT))
            {
                seedDict.Add((SeedTag)tag, -1);
            }
        }
    }

    private int RandomSeed()
    {
        double doubleSeed;
        System.Random rng = new System.Random();

        do
        {
            doubleSeed = rng.NextDouble();
        }
        while (doubleSeed < 0.1);

        return (int)(doubleSeed * Math.Pow(10, 9));
    }

    private int RandomSeed(System.Random rng)
    {
        return (int)(rng.NextDouble() * Math.Pow(10, 9));
    }
}
