using UnityEngine;

public class InternalClock : MonoBehaviour
{
    private int energyTurn;

    public void EndTurn()
    {
    }

    public void StartTurn()
    {
        gameObject.GetComponent<Energy>().GainEnergy(energyTurn, true);

        if (gameObject.GetComponent<AutoExplore>() != null)
        {
            gameObject.GetComponent<AutoExplore>().CountDown();
        }
    }

    private void Awake()
    {
        energyTurn = 2000;
    }
}
