using System;
using System.Collections.Generic;
using UnityEngine;

public enum InfectionTag { Slow, Weak, Poison };

public class Infection : MonoBehaviour
{
    private Dictionary<InfectionTag, int> infectionDuration;
    private int maxInfections;
    private UIMessage message;

    public void CountDown()
    {
    }

    public void GainInfection()
    {
        if (!IsInfected())
        {
            return;
        }
        message.StoreText("You are infected.");
    }

    public bool HasInfection(InfectionTag infection)
    {
        return false;
    }

    private void Awake()
    {
        infectionDuration = new Dictionary<InfectionTag, int>();

        foreach (var tag in Enum.GetValues(typeof(InfectionTag)))
        {
            infectionDuration.Add((InfectionTag)tag, 0);
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
