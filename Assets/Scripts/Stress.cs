using System;
using UnityEngine;

public class Stress : MonoBehaviour
{
    public int CurrentStress { get; private set; }
    public int MaxStress { get; private set; }

    public void ClearStress()
    {
        CurrentStress = 0;

        // TODO: Lose all powers.
    }

    public void GainStress(int stress)
    {
        CurrentStress = Math.Min(MaxStress, CurrentStress + stress);

        // TODO: Stress component: Gain powers or infections. Infection
        // component: Lose HP.
    }

    private void Start()
    {
        MaxStress = FindObjects.GameLogic.GetComponent<ObjectData>().GetIntData(
            gameObject.GetComponent<ObjectMetaInfo>().SubTag, DataTag.Stress);
        CurrentStress = 0;
    }
}
