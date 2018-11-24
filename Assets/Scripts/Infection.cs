using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InfectionTag { Weak, Slow, Poison };

public class Infection : MonoBehaviour
{
    private DungeonBoard board;
    private int countInfections;
    private int defaultMaxHP;
    private double hpFactor;
    private int infectionOverflowDamage;
    private Dictionary<InfectionTag, int> infectionsDict;
    private int maxDuration;
    private int maxInfections;
    private UIMessage message;
    private int modFog;
    private int modPoison;
    private int modPool;
    private RandomNumber random;

    public int ModEnergy { get; private set; }

    public void CountDown()
    {
        foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
        {
            if (infectionsDict[tag] > 0)
            {
                infectionsDict[tag] -= 1;

                if (infectionsDict[tag] == 0)
                {
                    countInfections--;
                }
            }
        }
    }

    public void GainInfection()
    {
        int count;

        if (!IsInfected(out count))
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            if (gameObject.GetComponent<Stress>().IsUnderLowStress())
            {
                gameObject.GetComponent<Stress>().GainStress(1);
            }
            else if (countInfections >= maxInfections)
            {
                gameObject.GetComponent<HP>().LoseHP(infectionOverflowDamage);
            }
            else
            {
                ChooseInfection();
            }
        }
    }

    public bool HasInfection(InfectionTag tag, out int duration)
    {
        duration = infectionsDict[tag];
        return duration > 0;
    }

    public bool HasInfection(InfectionTag tag)
    {
        int temp;
        return HasInfection(tag, out temp);
    }

    public void ResetInfection()
    {
        foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
        {
            if (infectionsDict[tag] > 0)
            {
                infectionsDict[tag] = 0;
            }
        }

        countInfections = 0;
    }

    private void Awake()
    {
        ModEnergy = 200;

        countInfections = 0;
        maxDuration = 5;
        infectionOverflowDamage = 1;
        defaultMaxHP = 10;
        hpFactor = 3.0;
        modFog = 30;
        modPool = 10;
        modPoison = 20;

        infectionsDict = new Dictionary<InfectionTag, int>();

        foreach (var tag in Enum.GetValues(typeof(InfectionTag)))
        {
            infectionsDict.Add((InfectionTag)tag, 0);
        }
    }

    private void ChooseInfection()
    {
        List<InfectionTag> candidates = new List<InfectionTag>();
        InfectionTag newInfection;
        int index;

        foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
        {
            if (!HasInfection(tag))
            {
                candidates.Add(tag);
            }
        }

        if (candidates.Count > 0)
        {
            index = random.Next(SeedTag.Infection, 0, candidates.Count);
            newInfection = candidates[index];
            infectionsDict[newInfection] = maxDuration;

            countInfections++;
        }
    }

    private int GetInfectionRate()
    {
        int currentHP;
        int hp;
        int pool;
        int fog;
        int poison;
        int maxMod;
        int sumMod;
        int finalRate;

        currentHP = gameObject.GetComponent<HP>().CurrentHP;
        hp = (int)Math.Floor(
            Math.Max(0, defaultMaxHP - currentHP) / hpFactor * 10);

        pool = board.CheckBlock(SubObjectTag.Pool, gameObject.transform.position)
            ? modPool : 0;

        // TODO: Check weather. Attack power changes infection rate.
        bool hasFog = false;
        fog = hasFog ? modFog : 0;

        poison = HasInfection(InfectionTag.Poison) ? modPoison : 0;

        sumMod = hp + pool + fog + poison;
        // NOTE: Enumerable.Max
        maxMod = new int[] { hp, pool, fog, poison }.Max();

        finalRate = (int)Math.Floor((sumMod + maxMod) * 0.5);

        return finalRate;
    }

    private bool IsInfected(out int totalInfections)
    {
        int rate = GetInfectionRate();
        totalInfections = 0;

        while (rate > 100)
        {
            totalInfections++;
            rate -= 100;
        }

        if (random.Next(SeedTag.Infection, 1, 101) <= rate)
        {
            totalInfections++;
        }

        if (totalInfections > 0)
        {
            message.StoreText("You are infected.");
            return true;
        }
        return false;
    }

    private void Start()
    {
        maxInfections = FindObjects.GameLogic.GetComponent<ObjectData>()
            .GetIntData(gameObject.GetComponent<ObjectMetaInfo>().SubTag,
            DataTag.MaxInfections);

        message = FindObjects.GameLogic.GetComponent<UIMessage>();
        random = FindObjects.GameLogic.GetComponent<RandomNumber>();
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
    }
}
