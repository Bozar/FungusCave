using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor
{
    public enum InfectionTag { INVALID, Weak, Slow, Mutated };

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

    public class Infection : MonoBehaviour, ITurnCounter, IResetData
    {
        private InfectionData infectionData;
        private Dictionary<InfectionTag, int> infectionDict;
        private IInfectionRate iRate;
        private IInfectionRecovery iRecovery;
        private int maxRepeat;
        private Stack<InfectionTag> previousInfection;
        private RandomNumber random;

        public Dictionary<InfectionTag, int> InfectionDict
        {
            get
            {
                return new Dictionary<InfectionTag, int>(infectionDict);
            }
        }

        public void Count()
        {
            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                if (infectionDict[tag] > 0)
                {
                    infectionDict[tag] -= iRecovery.Recovery;
                }
            }
        }

        public void GainInfection(GameObject attacker)
        {
            if (!IsInfected(attacker, out int count))
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                if (!GetComponent<Stress>().HasMaxStress())
                {
                    GetComponent<Stress>().GainStress(1);
                    continue;
                }

                GetComponent<ICombatMessage>().IsInfected();
                if (HasInfection(out InfectionTag tag, out _))
                {
                    infectionDict[tag]
                        = Math.Min(infectionData.MaxDuration,
                        infectionDict[tag] + infectionData.OverflowDuration);
                }
                else
                {
                    ChooseInfection();
                }
            }
            return;
        }

        public bool HasInfection(InfectionTag tag, out int duration)
        {
            duration = infectionDict[tag];
            return duration > 0;
        }

        public bool HasInfection(out InfectionTag tag, out int duration)
        {
            duration = 0;
            tag = InfectionTag.INVALID;

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
            return infectionDict[tag] > 0;
        }

        public void Reset()
        {
            ResetInfection();
        }

        public void ResetInfection()
        {
            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                infectionDict[tag] = 0;
            }
        }

        public void Trigger()
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            maxRepeat = 2;
            previousInfection = new Stack<InfectionTag>();

            infectionDict = new Dictionary<InfectionTag, int>();
            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                infectionDict.Add(tag, 0);
            }
        }

        private void ChooseInfection()
        {
            bool repeat;
            InfectionTag newInfection;
            InfectionTag[] candidates = new InfectionTag[]
            { InfectionTag.Mutated, InfectionTag.Slow, InfectionTag.Weak };

            // The same infection should not appear 3 times (maxRepeat) in a row.
            do
            {
                newInfection = candidates[random.Next(SeedTag.Infection,
                    0, candidates.Length)];

                if (previousInfection.Count == 0)
                {
                    previousInfection.Push(newInfection);
                    break;
                }
                else if (previousInfection.Peek() == newInfection)
                {
                    if (previousInfection.Count < maxRepeat)
                    {
                        previousInfection.Push(newInfection);
                        break;
                    }
                    else
                    {
                        repeat = true;
                    }
                }
                else
                {
                    previousInfection = new Stack<InfectionTag>();
                    previousInfection.Push(newInfection);
                    break;
                }
            } while (repeat);

            infectionDict[newInfection] = infectionData.Duration;
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
