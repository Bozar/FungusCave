using UnityEngine;

public class InternalClock : MonoBehaviour
{
    public void EndTurn()
    {
    }

    public void StartTurn()
    {
        gameObject.GetComponent<Energy>().RestoreEnergy(Energy.RestoreType.Turn);
    }
}
