using Fungus.Actor.ObjectManager;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor
{
    public enum InfectionTag { Weak, Slow, Poisoned, Mutated };

    public interface IInfectionRate
    {
        // The actor specific infection rate: PCInfections.cs, NPCInfections.cs.
        int GetInfectionRate(GameObject attacker);

        // The universal infection rate: InfectionRate.cs.
        int GetInfectionRate();
    }

    public interface IInfectionRecovery
    {
        // The infection duration reduces every turn.
        int Recovery { get; }
    }

    public class Infection : MonoBehaviour, ITurnCounter
    {
        private EnergyData energyData;
        private InfectionData infectionData;
        private Dictionary<InfectionTag, int> infectionsDict;
        private IInfectionRate iRate;
        private IInfectionRecovery iRecovery;
        private int maxRepeat;
        private Stack<InfectionTag> previousInfections;
        private RandomNumber random;

        public int ActiveInfections
        {
            get
            {
                int count = 0;

                foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
                {
                    if (infectionsDict[tag] > 0)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public void Count()
        {
            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                if (infectionsDict[tag] > 0)
                {
                    infectionsDict[tag] -= iRecovery.Recovery;
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
                }
                else if (ActiveInfections >= infectionData.MaxInfections)
                {
                    GetComponent<ICombatMessage>().IsExhausted();
                    GetComponent<Energy>().LoseEnergy(energyData.ModNormal);
                }
                else
                {
                    GetComponent<ICombatMessage>().IsInfected();
                    ChooseInfection();
                }
            }
            return;
        }

        public bool HasInfection()
        {
            return ActiveInfections > 0;
        }

        public bool HasInfection(InfectionTag tag, out int duration)
        {
            duration = infectionsDict[tag];
            return duration > 0;
        }

        public bool HasInfection(out InfectionTag tag, out int duration)
        {
            duration = 0;
            tag = InfectionTag.Weak;

            foreach (InfectionTag t in Enum.GetValues(typeof(InfectionTag)))
            {
                if (GetComponent<Infection>().HasInfection(t, out duration))
                {
                    tag = t;
                    return true;
                }
            }
            return false;
        }

        public bool HasInfection(InfectionTag tag)
        {
            return infectionsDict[tag] > 0;
        }

        public void ResetInfection()
        {
            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                infectionsDict[tag] = 0;
            }
        }

        public void Trigger()
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            maxRepeat = 2;
            previousInfections = new Stack<InfectionTag>();

            infectionsDict = new Dictionary<InfectionTag, int>();
            foreach (var tag in Enum.GetValues(typeof(InfectionTag)))
            {
                infectionsDict.Add((InfectionTag)tag, 0);
            }
        }

        private void ChooseInfection()
        {
            bool repeat;
            InfectionTag newInfection;

            // The same infection should not appear 3 times (maxRepeat) in a row.
            do
            {
                repeat = false;
                newInfection = (InfectionTag)random.Next(
                    SeedTag.Infection,
                    0, Enum.GetNames(typeof(InfectionTag)).Length);

                if (previousInfections.Count == 0)
                {
                    previousInfections.Push(newInfection);
                    break;
                }
                else if (previousInfections.Peek() == newInfection)
                {
                    if (previousInfections.Count < maxRepeat)
                    {
                        previousInfections.Push(newInfection);
                        break;
                    }
                    else
                    {
                        repeat = true;
                    }
                }
                else
                {
                    previousInfections = new Stack<InfectionTag>();
                    previousInfections.Push(newInfection);
                    break;
                }
            } while (repeat);

            infectionsDict[newInfection] = infectionData.Duration;
        }

        private bool IsInfected(GameObject attacker, out int totalInfections)
        {
            int rate = iRate.GetInfectionRate(attacker);
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
            random = FindObjects.GameLogic.GetComponent<RandomNumber>();
            energyData = FindObjects.GameLogic.GetComponent<EnergyData>();
            infectionData = FindObjects.GameLogic.GetComponent<InfectionData>();

            if (GetComponent<MetaInfo>().IsPC)
            {
                iRate = GetComponent<PCInfection>() as IInfectionRate;
                iRecovery = GetComponent<PCInfection>() as IInfectionRecovery;
            }
            else
            {
                iRate = GetComponent<NPCInfection>() as IInfectionRate;
                iRecovery = GetComponent<NPCInfection>() as IInfectionRecovery;
            }
        }
    }
}
