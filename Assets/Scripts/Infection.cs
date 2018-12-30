using Fungus.Actor.ObjectManager;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor
{
    public enum InfectionTag { Mutate, Weak, Slow, Poison };

    public class Infection : MonoBehaviour, ITurnCounter
    {
        private ActorBoard actor;
        private bool attacker;
        private int countInfections;
        private int defaultMaxHP;
        private DungeonBoard dungeon;
        private int energyInfectionOverflow;
        private bool hasPower;
        private double hpFactor;
        private Dictionary<InfectionTag, int> infectionsDict;
        private int maxDuration;
        private int maxInfections;
        private UIMessage message;
        private int modFog;
        private int modInfectionPoison;
        private int modPool;
        private int modPowerImmunity1;
        private int modPowerPoison1;
        private int powerImmunity2;
        private RandomNumber random;

        public int ModEnergy { get; private set; }

        public void Count()
        {
            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                if (infectionsDict[tag] > 0)
                {
                    infectionsDict[tag] -= 1;

                    if (infectionsDict[tag] == 0)
                    {
                        countInfections--;
                    }
                }
            }
        }

        public void GainInfection(bool attackerHasPower)
        {
            int count;
            attacker = attackerHasPower;

            if (!IsInfected(out count))
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                hasPower = GetComponent<Power>().HasPower(PowerTag.Immunity2);

                if (!GetComponent<Stress>().HasMaxStress())
                {
                    GetComponent<Stress>().GainStress(1);
                }
                else if (countInfections >= maxInfections)
                {
                    GetComponent<Energy>().LoseEnergy(
                        energyInfectionOverflow);
                }
                else
                {
                    ChooseInfection();

                    if (hasPower)
                    {
                        GetComponent<HP>().GainHP(powerImmunity2);
                    }
                }
            }

            return;
        }

        public bool HasInfection()
        {
            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                if (infectionsDict[tag] > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasInfection(InfectionTag tag, out int duration)
        {
            duration = infectionsDict[tag];
            return HasInfection(tag);
        }

        public bool HasInfection(InfectionTag tag)
        {
            return infectionsDict[tag] > 0;
        }

        public void ResetInfection()
        {
            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                if (infectionsDict[tag] > 0)
                {
                    infectionsDict[tag] = 0;
                }
            }

            countInfections = 0;
        }

        public void Trigger()
        {
        }

        private void Awake()
        {
            ModEnergy = 60;

            countInfections = 0;
            maxDuration = 5;
            energyInfectionOverflow = 200;
            powerImmunity2 = 2;
            defaultMaxHP = 10;
            hpFactor = 3.0;
            modFog = 40;
            modPool = 20;
            modInfectionPoison = 60;
            modPowerImmunity1 = 20;
            modPowerPoison1 = 30;

            infectionsDict = new Dictionary<InfectionTag, int>();

            foreach (var tag in Enum.GetValues(typeof(InfectionTag)))
            {
                infectionsDict.Add((InfectionTag)tag, 0);
            }
        }

        private void ChooseInfection()
        {
            List<InfectionTag> candidates = new List<InfectionTag>();
            InfectionTag newInfection;
            int index;

            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                if (!HasInfection(tag))
                {
                    candidates.Add(tag);
                }
            }

            if (candidates.Count > 0)
            {
                index = random.Next(SeedTag.Infection, 0, candidates.Count);
                newInfection = candidates[index];
                infectionsDict[newInfection] = maxDuration;

                countInfections++;
            }
        }

        private int GetInfectionRate(bool attackerHasPower)
        {
            int currentHP;
            Vector3 currentPosition;
            int hp;
            int pool;
            int fog;
            int attPower;
            int stress;
            int poison;
            int sumMod;
            int sumFactor;
            int finalRate;

            currentHP = GetComponent<HP>().CurrentHP;
            currentPosition = transform.position;

            hp = (int)Math.Floor(
                Math.Max(0, defaultMaxHP - currentHP) / hpFactor * 10);

            pool = dungeon.CheckBlock(SubObjectTag.Pool, currentPosition)
                ? modPool : 0;

            // TODO: Check weather.
            fog = modFog * 0;
            attPower = attackerHasPower ? modPowerPoison1 : 0;

            hasPower = GetComponent<Power>().HasPower(PowerTag.Immunity1);
            stress = (hasPower && GetComponent<Stress>().HasMaxStress())
                ? modPowerImmunity1 : 0;

            poison = HasInfection(InfectionTag.Poison) ? modInfectionPoison : 0;

            sumMod = Math.Max(0, hp + pool + fog + attPower - stress);
            sumFactor = poison + 100;

            finalRate = (int)Math.Floor(sumMod * (sumFactor * 0.01));

            return finalRate;
        }

        private bool IsInfected(out int totalInfections)
        {
            int rate = GetInfectionRate(attacker);
            totalInfections = 0;

            while (rate > 100)
            {
                totalInfections++;
                rate -= 100;
            }

            if (random.Next(SeedTag.Infection, 1, 101) <= rate)
            {
                totalInfections++;
            }

            if (totalInfections > 0)
            {
                if (actor.CheckActorTag(SubObjectTag.PC, gameObject))
                {
                    message.StoreText("You are infected.");
                }
                return true;
            }
            return false;
        }

        private void Start()
        {
            maxInfections = FindObjects.GameLogic.GetComponent<ObjectData>()
                .GetIntData(GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.MaxInfections);

            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            random = FindObjects.GameLogic.GetComponent<RandomNumber>();
            dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
        }
    }
}
