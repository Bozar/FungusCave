using System;
using System.Collections.Generic;
using UnityEngine;

public enum InfectionTag { Mutate, Weak, Slow, Poison };

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
    private int modInfectionPoison;
    private int modPool;
    private int modPowerImmunity1;
    private int modPowerPoison1;
    private bool npcHasPower;
    private bool pcHasPower;
    private int powerImmunity2;
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

        // TODO: Update after Unity 2018.3.
        pcHasPower = gameObject.GetComponent<PCPowers>() != null
            && gameObject.GetComponent<PCPowers>().HasPower(PowerTag.Immunity2);
        npcHasPower = gameObject.GetComponent<NPCPowers>() != null
            && gameObject.GetComponent<NPCPowers>().HasPower(PowerTag.Immunity2);

        for (int i = 0; i < count; i++)
        {
            if (!gameObject.GetComponent<Stress>().HasMaxStress())
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

                if (pcHasPower || npcHasPower)
                {
                    gameObject.GetComponent<HP>().GainHP(powerImmunity2);
                }
            }
        }
    }

    public bool HasInfection()
    {
        foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
        {
            if (infectionsDict[tag] > 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasInfection(InfectionTag tag, out int duration)
    {
        duration = infectionsDict[tag];
        return HasInfection(tag);
    }

    public bool HasInfection(InfectionTag tag)
    {
        return infectionsDict[tag] > 0;
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
        powerImmunity2 = 2;
        defaultMaxHP = 10;
        hpFactor = 3.0;
        modFog = 40;
        modPool = 20;
        modInfectionPoison = 60;
        modPowerImmunity1 = 20;
        modPowerPoison1 = 30;

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
        Vector3 currentPosition;
        int hp;
        int pool;
        int fog;
        int attack;
        int stress;
        int poison;
        int sumMod;
        int sumFactor;
        int finalRate;

        currentHP = gameObject.GetComponent<HP>().CurrentHP;
        currentPosition = gameObject.transform.position;

        hp = (int)Math.Floor(
            Math.Max(0, defaultMaxHP - currentHP) / hpFactor * 10);

        pool = board.CheckBlock(SubObjectTag.Pool, currentPosition)
            ? modPool : 0;

        // TODO: Check weather. Attack power increase infection rate.
        fog = modFog * 0;
        attack = modPowerPoison1 * 0;

        // TODO: Update after Unity 2018.3.
        pcHasPower = gameObject.GetComponent<PCPowers>() != null
            && gameObject.GetComponent<PCPowers>().HasPower(PowerTag.Immunity1);
        npcHasPower = gameObject.GetComponent<NPCPowers>() != null
            && gameObject.GetComponent<NPCPowers>().HasPower(PowerTag.Immunity1);
        stress = ((pcHasPower || npcHasPower)
            && gameObject.GetComponent<Stress>().HasMaxStress())
            ? modPowerImmunity1 : 0;

        poison = HasInfection(InfectionTag.Poison) ? modInfectionPoison : 0;

        sumMod = Math.Max(0, hp + pool + fog + attack - stress);
        sumFactor = poison + 100;

        finalRate = (int)Math.Floor(sumMod * (sumFactor * 0.01));

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
