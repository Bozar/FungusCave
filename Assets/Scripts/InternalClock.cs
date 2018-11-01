using UnityEngine;

public class InternalClock : MonoBehaviour
{
    private int energyTurn;

    public void EndTurn()
    {
    }

    public void StartTurn()
    {
        gameObject.GetComponent<Energy>().RestoreEnergy(energyTurn, true);
    }

    private void Awake()
    {
        energyTurn = 2000;
    }
}
