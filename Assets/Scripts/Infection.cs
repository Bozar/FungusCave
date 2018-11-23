using System;
using System.Collections.Generic;
using UnityEngine;

public enum InfectionTag { Weak, Slow, Poison };

public class Infection : MonoBehaviour
{
    private int countInfections;
    private int duration;
    private int infectionOverflowDamage;
    private Dictionary<InfectionTag, int> infectionsDict;
    private int maxInfections;
    private UIMessage message;

    public int ModDamage { get; private set; }
    public int ModEnergy { get; private set; }
    public int ModInfectionRate { get; private set; }

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

    public void GainInfection(InfectionTag tag)
    {
        if (!IsInfected())
        {
            return;
        }

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
            infectionsDict[tag] = duration;
            countInfections++;
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
        ModDamage = 1;
        ModEnergy = 200;

        countInfections = 0;
        duration = 5;
        infectionOverflowDamage = 2;
        infectionsDict = new Dictionary<InfectionTag, int>();

        foreach (var tag in Enum.GetValues(typeof(InfectionTag)))
        {
            infectionsDict.Add((InfectionTag)tag, 0);
        }
    }

    private bool IsInfected()
    {
        message.StoreText("You are infected.");

        return true;
    }

    private void Start()
    {
        maxInfections = FindObjects.GameLogic.GetComponent<ObjectData>()
            .GetIntData(gameObject.GetComponent<ObjectMetaInfo>().SubTag,
            DataTag.MaxInfections);

        message = FindObjects.GameLogic.GetComponent<UIMessage>();
    }
}
