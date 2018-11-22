using System;
using UnityEngine;

public class Stress : MonoBehaviour
{
    public int CurrentStress { get; private set; }
    public int MaxStress { get; private set; }

    public void GainStress(int stress)
    {
        CurrentStress = Math.Min(MaxStress, CurrentStress + stress);

        // TODO: Gain powers.
    }

    public bool IsUnderLowStress()
    {
        return CurrentStress < MaxStress;
    }

    public void ResetStress()
    {
        CurrentStress = 0;

        // TODO: Lose all powers.
    }

    private void Start()
    {
        MaxStress = FindObjects.GameLogic.GetComponent<ObjectData>().GetIntData(
            gameObject.GetComponent<ObjectMetaInfo>().SubTag, DataTag.Stress);
        CurrentStress = 0;
    }
}
