using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private ActorBoard actorBoard;
    private int[] attackerPosition;
    private int baseEnergy;
    private double cardinalFactor;
    private double diagonalFactor;
    private bool useCardinalFactor;

    public void DealDamage(int x, int y)
    {
        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy()
            || !actorBoard.HasActor(x, y))
        {
            return;
        }

        attackerPosition
            = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(gameObject.transform.position);

        useCardinalFactor = FindObjects.GameLogic.GetComponent<Direction>()
            .CheckDirection(RelativePosition.Cardinal, attackerPosition, x, y);

        gameObject.GetComponent<Energy>().LoseEnergy(GetEnergyCost());

        FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
            "You hit: " + x + "," + y);
        actorBoard.GetActor(x, y).GetComponent<Defend>().TestKill();
    }

    private void Awake()
    {
        baseEnergy = 1200;
        cardinalFactor = 1.0;
        diagonalFactor = 1.4;
    }

    private int GetEnergyCost()
    {
        int totalEnergy;
        double direction;

        // TODO: Attack in fog.
        direction = useCardinalFactor ? cardinalFactor : diagonalFactor;

        totalEnergy = (int)Math.Floor(baseEnergy * direction);

        return totalEnergy;
    }

    private void Start()
    {
        actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
    }
}
