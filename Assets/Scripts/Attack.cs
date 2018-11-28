using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private ActorBoard actorBoard;
    private int baseDamage;
    private int baseEnergy;
    private ConvertCoordinates coordinate;
    private Direction direction;
    private int powerEnergy2;
    private int weakDamage;

    public int GetCurrentDamage()
    {
        int weak;
        int finalDamage;

        // TODO: Change damage.

        weak = gameObject.GetComponent<Infection>()
            .HasInfection(InfectionTag.Weak)
            ? weakDamage
            : 0;

        finalDamage = baseDamage - weak;
        finalDamage = Math.Max(0, finalDamage);

        return finalDamage;
    }

    public void MeleeAttack(int x, int y)
    {
        bool targetIsDead;
        GameObject target;

        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy()
            || !actorBoard.HasActor(x, y))
        {
            return;
        }

        gameObject.GetComponent<Energy>().LoseEnergy(GetMeleeEnergy(x, y));

        target = actorBoard.GetActor(x, y);
        targetIsDead = target.GetComponent<HP>().LoseHP(GetCurrentDamage())
            || target.GetComponent<Infection>().GainInfection();

        // TODO: Update after Unity 2018.3.
        if (targetIsDead
            && (gameObject.GetComponent<PCPowers>() != null
            && gameObject.GetComponent<PCPowers>().PowerIsActive(
                PowerTag.Energy2))
            || (gameObject.GetComponent<NPCPowers>() != null
            && gameObject.GetComponent<NPCPowers>().PowerIsActive(
                PowerTag.Energy2)))
        {
            gameObject.GetComponent<Energy>().GainEnergy(powerEnergy2, false);
        }
    }

    private void Awake()
    {
        baseEnergy = 1200;
        weakDamage = 1;
        powerEnergy2 = 400;
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
            ? gameObject.GetComponent<Infection>().ModEnergy : 0;

        // TODO: Attack in fog. Power.
        totalEnergy = (int)Math.Floor(
            baseEnergy * ((100 + directionFactor + slow) * 0.01));

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
