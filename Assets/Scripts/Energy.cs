using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    private Dictionary<ConsumeType, int> ConsumeData;
    private Dictionary<RestoreType, int> RestoreData;

    public enum ConsumeType { Move, Attack, MoveDiagonally, AttackDiagonally };

    public enum RestoreType { Turn };

    public int CurrentEnergy { get; set; }

    public bool ConsumeEnergy(ConsumeType type, bool checkThreshold)
    {
        if (checkThreshold)
        {
            if (CurrentEnergy >= RestoreData[RestoreType.Turn])
            {
                CurrentEnergy -= ConsumeData[type];
                return true;
            }
            return false;
        }

        CurrentEnergy -= ConsumeData[type];
        return true;
    }

    public void PrintEnergy()
    {
        FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
            "Energy: " + CurrentEnergy.ToString());
    }

    public void RestoreEnergy(RestoreType type)
    {
        if (type == RestoreType.Turn)
        {
            if (CurrentEnergy < RestoreData[RestoreType.Turn])
            {
                CurrentEnergy += RestoreData[RestoreType.Turn];
            }
        }
        else
        {
            CurrentEnergy += RestoreData[type];
        }
    }

    private void Awake()
    {
        RestoreData = new Dictionary<RestoreType, int>
        {
            { RestoreType.Turn, 2000 },
        };

        ConsumeData = new Dictionary<ConsumeType, int>
        {
            { ConsumeType.Move, 1000 },
            { ConsumeType.MoveDiagonally, 1400 },
            { ConsumeType.Attack, 1400 },
            { ConsumeType.AttackDiagonally, 1960 }
        };

        CurrentEnergy = RestoreData[RestoreType.Turn];
    }
}
