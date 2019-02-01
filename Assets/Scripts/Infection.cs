using Fungus.Actor.ObjectManager;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor
{
    public enum InfectionTag { Weak, Slow, Poisoned };

    public interface IInfection
    {
        int GetInfectionRate(GameObject attacker);

        int GetInfectionRate();
    }

    public class Infection : MonoBehaviour, ITurnCounter
    {
        private GameObject attacker;
        private int countInfections;
        private ActorData data;
        private int energyInfectionOverflow;
        private bool hasPower;
        private IInfection infectionComponent;
        private Dictionary<InfectionTag, int> infectionsDict;
        private int maxDuration;
        private int maxInfections;
        private UIMessage message;
        private int powerImmunity2;
        private RandomNumber random;
        public int ImmunityRate { get; private set; }
        public int InfectionRate { get; private set; }
        public int ModEnergy { get; private set; }
        public int WeakDamage { get; private set; }

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

            if (!IsInfected(out count))
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                hasPower = GetComponent<Power>().HasPower(PowerTag.DefInfection2);

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
            WeakDamage = 1;

            countInfections = 0;
            maxDuration = 5;
            energyInfectionOverflow = 200;
            powerImmunity2 = 2;

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

        private bool IsInfected(out int totalInfections)
        {
            int rate = infectionComponent.GetInfectionRate(attacker);
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
                if (GetComponent<ObjectMetaInfo>().IsPC)
                {
                    message.StoreText("You are infected.");
                }
                return true;
            }
            return false;
        }

        private void Start()
        {
            maxInfections = FindObjects.GameLogic.GetComponent<ActorData>()
                .GetIntData(GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.MaxInfections);

            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            random = FindObjects.GameLogic.GetComponent<RandomNumber>();
            data = FindObjects.GameLogic.GetComponent<ActorData>();

            infectionComponent = GetComponent<ObjectMetaInfo>().IsPC
                ? (GetComponent<PCInfection>() as IInfection)
                : (GetComponent<NPCInfection>() as IInfection);

            InfectionRate = data.GetIntData(
                GetComponent<ObjectMetaInfo>().SubTag, DataTag.InfectionRate);
            ImmunityRate = data.GetIntData(
                GetComponent<ObjectMetaInfo>().SubTag, DataTag.ImmunityRate);
        }
    }
}
