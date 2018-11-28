using UnityEngine;

public class InternalClock : MonoBehaviour
{
    private int energyTurn;
    private int powerEnergy1;

    public void EndTurn()
    {
        gameObject.GetComponent<Infection>().CountDown();
    }

    public void StartTurn()
    {
        gameObject.GetComponent<Energy>().GainEnergy(energyTurn, true);

        if (gameObject.GetComponent<Power>().PowerIsActive(PowerTag.Energy1))
        {
            gameObject.GetComponent<Energy>().GainEnergy(powerEnergy1, false);
        }

        if (gameObject.GetComponent<AutoExplore>() != null)
        {
            gameObject.GetComponent<AutoExplore>().CountDown();
        }
    }

    private void Awake()
    {
        energyTurn = 2000;
        powerEnergy1 = 200;
    }
}
