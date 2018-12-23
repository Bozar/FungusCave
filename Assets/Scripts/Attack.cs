using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Attack : MonoBehaviour
    {
        private ActorBoard actorBoard;
        private int attackPowerEnergy;
        private PowerTag[] attackPowers;
        private int baseDamage;
        private int baseEnergy;
        private ConvertCoordinates coordinate;
        private ObjectData data;
        private Direction direction;
        private int powerDamage1;
        private int powerDamage2;
        private int powerEnergy2;
        private int powerPoison2;
        private int relieveStressAfterKill;
        private int weakDamage;

        public int CurrentDamage
        {
            get
            {
                int weak;
                int power;
                int finalDamage;

                // TODO: Change damage.

                weak = GetComponent<Infection>().HasInfection(InfectionTag.Weak)
                    ? weakDamage : 0;

                power = GetComponent<Power>().PowerIsActive(PowerTag.Damage1)
                    ? powerDamage1 : 0;
                power += GetComponent<Power>().PowerIsActive(PowerTag.Damage2)
                    ? powerDamage2 : 0;

                finalDamage = baseDamage + power - weak;
                finalDamage = Math.Max(0, finalDamage);

                return finalDamage;
            }
        }

        public void MeleeAttack(int x, int y)
        {
            bool hasPower;
            bool targetIsDead;
            GameObject target;

            if (!GetComponent<Energy>().HasEnoughEnergy()
                || !actorBoard.HasActor(x, y))
            {
                return;
            }

            GetComponent<Energy>().LoseEnergy(GetMeleeEnergy(x, y));

            hasPower = GetComponent<Power>().PowerIsActive(PowerTag.Poison2);
            target = actorBoard.GetActor(x, y);

            if (hasPower)
            {
                target.GetComponent<Energy>().LoseEnergy(powerPoison2);
            }

            targetIsDead = target.GetComponent<HP>().LoseHP(CurrentDamage);

            if (targetIsDead)
            {
                RestoreEnergy();
                GetComponent<Stress>().LoseStress(relieveStressAfterKill);
            }
            else
            {
                hasPower = GetComponent<Power>().PowerIsActive(PowerTag.Poison1);
                target.GetComponent<Infection>().GainInfection(hasPower);
            }
        }

        private void Awake()
        {
            weakDamage = 1;
            powerEnergy2 = 400;
            powerPoison2 = 400;
            powerDamage1 = 1;
            powerDamage2 = 1;
            attackPowerEnergy = 200;
            relieveStressAfterKill = 2;
        }

        private int GetMeleeEnergy(int x, int y)
        {
            int[] position;
            bool isCardinal;
            double directionFactor;
            int attack;
            int totalEnergy;
            int slow;

            position = coordinate.Convert(transform.position);
            isCardinal = direction.CheckDirection(
                RelativePosition.Cardinal, position, x, y);

            directionFactor = isCardinal
                ? direction.CardinalFactor
                : direction.DiagonalFactor;

            slow = GetComponent<Infection>().HasInfection(InfectionTag.Slow)
                ? GetComponent<Infection>().ModEnergy : 0;

            attack = 0;
            foreach (PowerTag tag in attackPowers)
            {
                if (GetComponent<Power>().PowerIsActive(tag))
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
            if (GetComponent<Power>().PowerIsActive(PowerTag.Energy2))
            {
                GetComponent<Energy>().GainEnergy(powerEnergy2);
            }
        }

        private void Start()
        {
            actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
            coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            direction = FindObjects.GameLogic.GetComponent<Direction>();
            data = FindObjects.GameLogic.GetComponent<ObjectData>();

            attackPowers = new PowerTag[]
            {
            PowerTag.Damage1, PowerTag.Damage2,
            PowerTag.Poison1, PowerTag.Poison2
            };

            baseDamage = data.GetIntData(GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.Damage);
            baseEnergy = data.GetIntData(GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.EnergyAttack);
        }
    }
}
