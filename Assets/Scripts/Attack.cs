using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private ActorBoard actorBoard;
    private int baseDamage;
    private int baseEnergy;
    private double cardinalFactor;
    private ConvertCoordinates coordinate;
    private double diagonalFactor;
    private Direction direction;

    public void DealDamage(int x, int y)
    {
        int[] position;
        bool isCardinalAttack;
        int attackEnergy;

        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy()
            || !actorBoard.HasActor(x, y))
        {
            return;
        }

        position = coordinate.Convert(gameObject.transform.position);
        isCardinalAttack = direction.CheckDirection(
            RelativePosition.Cardinal, position, x, y);
        attackEnergy = GetEnergyCost(isCardinalAttack);

        gameObject.GetComponent<Energy>().LoseEnergy(attackEnergy);
        actorBoard.GetActor(x, y).GetComponent<Defend>().TakeDamage(
            GetCurrentDamage());
    }

    public int GetCurrentDamage()
    {
        // TODO: Change damage.
        return baseDamage;
    }

    private void Awake()
    {
        baseEnergy = 1200;
        cardinalFactor = 1.0;
        diagonalFactor = 1.4;
    }

    private int GetEnergyCost(bool isCardinal)
    {
        int totalEnergy;
        double directionFactor;

        // TODO: Attack in fog.
        directionFactor = isCardinal ? cardinalFactor : diagonalFactor;
        totalEnergy = (int)Math.Floor(baseEnergy * directionFactor);

        return totalEnergy;
    }

    private void Start()
    {
        actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        direction = FindObjects.GameLogic.GetComponent<Direction>();

        baseDamage
            = FindObjects.GameLogic.GetComponent<ObjectData>().GetIntData(
            gameObject.GetComponent<ObjectMetaInfo>().SubTag, DataTag.Damage);
    }
}
