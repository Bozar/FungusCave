using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private ActorBoard actorBoard;
    private int baseDamage;
    private int baseEnergy;
    private ConvertCoordinates coordinate;
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
            .HasInfection(InfectionTag.Weak)
            ? gameObject.GetComponent<Infection>().ModDamage
            : 0;

        finalDamage = baseDamage - weak;

        return finalDamage;
    }

    private void Awake()
    {
        baseEnergy = 1200;
    }

    private int GetMeleeEnergy(int x, int y)
    {
        int[] position;
        bool isCardinal;
        double directionFactor;
        int totalEnergy;
        int slow;

        position = coordinate.Convert(gameObject.transform.position);
        isCardinal = direction.CheckDirection(
            RelativePosition.Cardinal, position, x, y);

        directionFactor = isCardinal
            ? direction.CardinalFactor
            : direction.DiagonalFactor;

        slow = gameObject.GetComponent<Infection>()
            .HasInfection(InfectionTag.Slow)
            ? gameObject.GetComponent<Infection>().ModEnergy
            : 0;

        // NOTE: Data is calculated in this way.
        //> total = (base + positiveMod - negativeMod) * factor
        //> positiveMod
        //> = Max(pos1, pos2, ...)
        //> + (Sum(pos1, pos2, ...) - Max(pos1, pos2, ...)) * 0.5

        // TODO: Attack in fog. Power.
        totalEnergy = (int)Math.Floor((baseEnergy + slow)
            * (directionFactor * 0.1));

        if (FindObjects.GameLogic.GetComponent<WizardMode>().PrintEnergyCost
            && actorBoard.CheckActorTag(SubObjectTag.PC, gameObject))
        {
            FindObjects.GameLogic.GetComponent<UIMessage>()
                .StoreText("Melee energy: " + totalEnergy);
        }

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
