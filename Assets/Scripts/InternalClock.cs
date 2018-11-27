using UnityEngine;

public class InternalClock : MonoBehaviour
{
    private int bonusEnergy;
    private int energyTurn;

    public void EndTurn()
    {
        gameObject.GetComponent<Infection>().CountDown();
    }

    public void StartTurn()
    {
        gameObject.GetComponent<Energy>().GainEnergy(energyTurn, true);

        // TODO: Update after Unity 2018.3.
        if (gameObject.GetComponent<PCPowers>() != null
            && gameObject.GetComponent<PCPowers>().PowerIsActive(
                PowerTag.Energy1))
        {
            gameObject.GetComponent<Energy>().GainEnergy(bonusEnergy, false);
        }

        if (gameObject.GetComponent<AutoExplore>() != null)
        {
            gameObject.GetComponent<AutoExplore>().CountDown();
        }
    }

    private void Awake()
    {
        energyTurn = 2000;
        bonusEnergy = 400;
    }
}
