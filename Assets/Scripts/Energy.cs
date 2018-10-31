using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum EnergyConsume { Move, Attack, MoveDiagonally, AttackDiagonally };

public enum EnergyRestore { Turn };

public class Energy : MonoBehaviour
{
    private Dictionary<EnergyConsume, int> ConsumeData;
    private StringBuilder printText;
    private Dictionary<EnergyRestore, int> RestoreData;
    private int[] testPosition;
    public int CurrentEnergy { get; private set; }

    public bool ConsumeEnergy(EnergyConsume type, bool checkThreshold)
    {
        if (checkThreshold)
        {
            if (CurrentEnergy >= RestoreData[EnergyRestore.Turn])
            {
                CurrentEnergy -= ConsumeData[type];
                return true;
            }
            return false;
        }

        CurrentEnergy -= ConsumeData[type];
        return true;
    }

    public bool HasEnoughEnergy()
    {
        return CurrentEnergy >= RestoreData[EnergyRestore.Turn];
    }

    public void PrintEnergy()
    {
        testPosition = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(gameObject.transform.position);

        printText.Remove(0, printText.Length);
        printText.Append("[");
        printText.Append(testPosition[0].ToString());
        printText.Append(",");
        printText.Append(testPosition[1].ToString());
        printText.Append("] ");
        printText.Append("Energy: ");
        printText.Append(CurrentEnergy.ToString());

        FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
            printText.ToString());
    }

    public void RestoreEnergy(EnergyRestore type)
    {
        if (type == EnergyRestore.Turn)
        {
            if (CurrentEnergy < RestoreData[EnergyRestore.Turn])
            {
                CurrentEnergy += RestoreData[EnergyRestore.Turn];
            }
        }
        else
        {
            CurrentEnergy += RestoreData[type];
        }
    }

    private void Awake()
    {
        printText = new StringBuilder();

        RestoreData = new Dictionary<EnergyRestore, int>
        {
            { EnergyRestore.Turn, 2000 },
        };

        ConsumeData = new Dictionary<EnergyConsume, int>
        {
            { EnergyConsume.Move, 1000 },
            { EnergyConsume.MoveDiagonally, 1400 },
            { EnergyConsume.Attack, 1400 },
            { EnergyConsume.AttackDiagonally, 1960 }
        };

        CurrentEnergy = RestoreData[EnergyRestore.Turn];
    }
}
