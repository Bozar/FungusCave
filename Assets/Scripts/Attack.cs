using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private ActorBoard actorBoard;
    private int baseDamage;
    private int baseEnergy;
    private double cardinalFactor;
    private ConvertCoordinates coordinate;
    private int damageWeak;
    private double diagonalFactor;
    private Direction direction;

    public void DealDamage(int x, int y)
    {
        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy()
            || !actorBoard.HasActor(x, y))
        {
            return;
        }

        gameObject.GetComponent<Energy>().LoseEnergy(GetMeleeEnergy(x, y));
        actorBoard.GetActor(x, y).GetComponent<HP>().LoseHP(GetCurrentDamage());
    }

    public int GetCurrentDamage()
    {
        int weak;
        int finalDamage;

        // TODO: Change damage.

        weak = gameObject.GetComponent<Infection>()
            .HasInfection(InfectionTag.Weak) ? damageWeak : 0;

        finalDamage = baseDamage - weak;

        return finalDamage;
    }

    private void Awake()
    {
        baseEnergy = 1200;
        damageWeak = 2;
        cardinalFactor = 1.0;
        diagonalFactor = 1.4;
    }

    private int GetMeleeEnergy(int x, int y)
    {
        int[] position;
        bool isCardinal;
        double directionFactor;
        int totalEnergy;

        position = coordinate.Convert(gameObject.transform.position);
        isCardinal = direction.CheckDirection(
            RelativePosition.Cardinal, position, x, y);

        // TODO: Attack in fog. Infection and power.
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
