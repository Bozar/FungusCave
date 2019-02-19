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
        private int baseEnergy;
        private ConvertCoordinates coordinate;
        private Direction direction;
        private EnergyData energyData;
        private int powerEnergy2;
        private int powerPoison2;
        private int relieveStressAfterKill;
        private int slowEnergy;

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

            hasPower = (GetComponent<Power>() != null)
                && GetComponent<Power>().IsActive(PowerTag.AttInfection2);
            target = actorBoard.GetActor(x, y);

            if (hasPower)
            {
                target.GetComponent<Energy>().LoseEnergy(powerPoison2);
            }

            targetIsDead = target.GetComponent<HP>().LoseHP(
                GetComponent<IDamage>().CurrentDamage);

            if (targetIsDead)
            {
                RestoreEnergy();
                GetComponent<Stress>().LoseStress(relieveStressAfterKill);

                if (GetComponent<Potion>() != null)
                {
                    GetComponent<Potion>().GainPotion(
                        target.GetComponent<MetaInfo>().DropPotion);
                }
            }
            else
            {
                target.GetComponent<Infection>().GainInfection(gameObject);
            }
        }

        private void Awake()
        {
            powerEnergy2 = 400;
            powerPoison2 = 400;
            attackPowerEnergy = 200;
            relieveStressAfterKill = 1;
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
                ? slowEnergy : 0;

            attack = 0;
            foreach (PowerTag tag in attackPowers)
            {
                if ((GetComponent<Power>() != null)
                    && GetComponent<Power>().IsActive(tag))
                {
                    attack += attackPowerEnergy;
                }
            }

            // TODO: Attack in fog.
            totalEnergy = (int)Math.Floor(
                (baseEnergy + attack + slow) * ((100 + directionFactor) * 0.01));

            if (FindObjects.GameLogic.GetComponent<WizardMode>().PrintEnergyCost
                && GetComponent<MetaInfo>().IsPC)
            {
                FindObjects.GameLogic.GetComponent<UIMessage>()
                    .StoreText("Melee energy: " + totalEnergy);
            }

            return totalEnergy;
        }

        private void RestoreEnergy()
        {
            if ((GetComponent<Power>() != null)
                && GetComponent<Power>().IsActive(PowerTag.DefEnergy2))
            {
                GetComponent<Energy>().GainEnergy(powerEnergy2);
            }
        }

        private void Start()
        {
            actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
            coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            direction = FindObjects.GameLogic.GetComponent<Direction>();
            energyData = FindObjects.GameLogic.GetComponent<EnergyData>();

            attackPowers = new PowerTag[]
            {
                PowerTag.AttDamage1, PowerTag.AttDamage2,
                PowerTag.AttInfection1, PowerTag.AttInfection2
            };

            baseEnergy = energyData.Attack;
            slowEnergy = energyData.InfectionSlow;
        }
    }
}
