using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private ActorBoard actorBoard;
    private int attackPowerEnergy;
    private PowerTag[] attackPowers;
    private int baseDamage;
    private int baseEnergy;
    private ConvertCoordinates coordinate;
    private Direction direction;
    private int powerEnergy2;
    private int powerPoison2;
    private int weakDamage;

    public int GetCurrentDamage()
    {
        int weak;
        int finalDamage;

        // TODO: Change damage.

        weak = gameObject.GetComponent<Infection>()
            .HasInfection(InfectionTag.Weak)
            ? weakDamage : 0;

        finalDamage = baseDamage - weak;
        finalDamage = Math.Max(0, finalDamage);

        return finalDamage;
    }

    public void MeleeAttack(int x, int y)
    {
        bool hasPower;
        bool targetIsDead;
        GameObject target;

        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy()
            || !actorBoard.HasActor(x, y))
        {
            return;
        }

        gameObject.GetComponent<Energy>().LoseEnergy(GetMeleeEnergy(x, y));

        hasPower = gameObject.GetComponent<Power>().PowerIsActive(
           PowerTag.Poison2);
        target = actorBoard.GetActor(x, y);

        if (hasPower)
        {
            target.GetComponent<Energy>().LoseEnergy(powerPoison2);
        }

        targetIsDead = target.GetComponent<HP>().LoseHP(GetCurrentDamage());

        if (targetIsDead)
        {
            RestoreEnergy();
        }
        else
        {
            hasPower = gameObject.GetComponent<Power>().PowerIsActive(
                PowerTag.Poison1);
            target.GetComponent<Infection>().GainInfection(hasPower);
        }
    }

    private void Awake()
    {
        baseEnergy = 1200;
        weakDamage = 1;
        powerEnergy2 = 400;
        powerPoison2 = 400;
        attackPowerEnergy = 200;
    }

    private int GetMeleeEnergy(int x, int y)
    {
        int[] position;
        bool isCardinal;
        double directionFactor;
        int attack;
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

        attack = 0;
        foreach (PowerTag tag in attackPowers)
        {
            if (gameObject.GetComponent<Power>().PowerIsActive(tag))
            {
                attack += attackPowerEnergy;
            }
        }

        // TODO: Attack in fog.
        totalEnergy = (int)Math.Floor(
            (baseEnergy + attack) * ((100 + directionFactor + slow) * 0.01));

        if (FindObjects.GameLogic.GetComponent<WizardMode>().PrintEnergyCost
            && actorBoard.CheckActorTag(SubObjectTag.PC, gameObject))
        {
            FindObjects.GameLogic.GetComponent<UIMessage>()
                .StoreText("Melee energy: " + totalEnergy);
        }

        return totalEnergy;
    }

    private void RestoreEnergy()
    {
        if (gameObject.GetComponent<Power>().PowerIsActive(PowerTag.Energy2))
        {
            gameObject.GetComponent<Energy>().GainEnergy(powerEnergy2, false);
        }
    }

    private void Start()
    {
        actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        direction = FindObjects.GameLogic.GetComponent<Direction>();

        attackPowers = new PowerTag[]
        {
            PowerTag.Damage1, PowerTag.Damage2,
            PowerTag.Poison1, PowerTag.Poison2
        };

        baseDamage
            = FindObjects.GameLogic.GetComponent<ObjectData>().GetIntData(
            gameObject.GetComponent<ObjectMetaInfo>().SubTag, DataTag.Damage);
    }
}
