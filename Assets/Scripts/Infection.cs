using System;
using System.Collections.Generic;
using UnityEngine;

public enum InfectionTag { Slow, Weak, Poison };

public class Infection : MonoBehaviour
{
    private int duration;
    private Dictionary<InfectionTag, int> infectionsDict;
    private int maxInfections;
    private UIMessage message;

    public void CountDown()
    {
        foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
        {
            if (infectionsDict[tag] > 0)
            {
                infectionsDict[tag] -= 1;
            }
        }
    }

    public void GainInfection(InfectionTag tag)
    {
        if (!IsInfected())
        {
            return;
        }

        infectionsDict[tag] = duration;
        message.StoreText("You are infected.");
    }

    public bool HasInfection(InfectionTag tag)
    {
        return infectionsDict[tag] > 0;
    }

    private void Awake()
    {
        duration = 5;
        infectionsDict = new Dictionary<InfectionTag, int>();

        foreach (var tag in Enum.GetValues(typeof(InfectionTag)))
        {
            infectionsDict.Add((InfectionTag)tag, 0);
        }
    }

    private bool IsInfected()
    {
        return true;
    }

    private void LoseInfection()
    {
    }

    private void Start()
    {
        maxInfections = FindObjects.GameLogic.GetComponent<ObjectData>()
            .GetIntData(gameObject.GetComponent<ObjectMetaInfo>().SubTag,
            DataTag.MaxInfections);

        message = FindObjects.GameLogic.GetComponent<UIMessage>();
    }
}
