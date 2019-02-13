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
    public enum HealthTag { Stressed, Infected, Overflowed };

    public enum InfectionTag { Weak, Slow, Poisoned };

    public interface IInfection
    {
        int InfectionDuration { get; }

        // Describe actor's status in the message board.
        string GetHealthReport(HealthTag status);

        // The actor specific infection rate: PCInfections.cs, NPCInfections.cs.
        int GetInfectionRate(GameObject attacker);

        // The universal infection rate: InfectionRate.cs.
        int GetInfectionRate();
    }

    public class Infection : MonoBehaviour, ITurnCounter
    {
        private int countInfections;
        private EnergyData energyData;
        private int energyInfectionOverflow;
        private IInfection infectionComponent;
        private InfectionData infectionData;
        private Dictionary<InfectionTag, int> infectionsDict;
        private int maxInfections;
        private UIMessage message;
        private RandomNumber random;

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

        public void GainInfection(GameObject attacker)
        {
            int count;

            if (!IsInfected(attacker, out count))
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                if (!GetComponent<Stress>().HasMaxStress())
                {
                    GetComponent<Stress>().GainStress(1);
                    message.StoreText(infectionComponent.GetHealthReport(
                        HealthTag.Stressed));
                }
                else if (countInfections >= maxInfections)
                {
                    GetComponent<Energy>().LoseEnergy(energyInfectionOverflow);
                    message.StoreText(infectionComponent.GetHealthReport(
                        HealthTag.Overflowed));
                }
                else
                {
                    ChooseInfection();
                    message.StoreText(infectionComponent.GetHealthReport(
                       HealthTag.Infected));
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
            throw new NotImplementedException();
        }

        private void Awake()
        {
            ModEnergy = 60;
            WeakDamage = 1;

            countInfections = 0;

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
                infectionsDict[newInfection]
                    = infectionComponent.InfectionDuration;

                countInfections++;
            }
        }

        private bool IsInfected(GameObject attacker, out int totalInfections)
        {
            int rate = infectionComponent.GetInfectionRate(attacker);
            totalInfections = 0;

            if (rate < 1)
            {
                return false;
            }

            while (rate > 100)
            {
                totalInfections++;
                rate -= 100;
            }

            if (random.Next(SeedTag.Infection, 1, 101) <= rate)
            {
                totalInfections++;
            }

            return totalInfections > 0;
        }

        private void Start()
        {
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            random = FindObjects.GameLogic.GetComponent<RandomNumber>();
            energyData = FindObjects.GameLogic.GetComponent<EnergyData>();
            infectionData = FindObjects.GameLogic.GetComponent<InfectionData>();

            infectionComponent = GetComponent<ObjectMetaInfo>().IsPC
                ? (GetComponent<PCInfection>() as IInfection)
                : (GetComponent<NPCInfection>() as IInfection);

            maxInfections = infectionData.MaxInfections;
            energyInfectionOverflow = energyData.InfectionOverflowed;
        }
    }
}
