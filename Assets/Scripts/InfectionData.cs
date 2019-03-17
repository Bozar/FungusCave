using Fungus.Actor;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class InfectionData : MonoBehaviour
    {
        private Dictionary<InfectionTag, string> infectionNames;

        public int Duration { get; private set; }
        public int MaxDuration { get; private set; }
        public int MaxInfections { get; private set; }
        public int RateHigh { get; private set; }
        public int RateNormal { get; private set; }
        public int RecoveryFast { get; private set; }
        public int RecoveryNormal { get; private set; }

        public string GetInfectionName(InfectionTag tag)
        {
            return infectionNames[tag];
        }

        private void Awake()
        {
            RateHigh = 80;
            RateNormal = 40;

            MaxDuration = 9;
            Duration = 5;

            RecoveryNormal = 1;
            RecoveryFast = 2;

            MaxInfections = 1;
        }

        private void Start()
        {
            infectionNames = new Dictionary<InfectionTag, string>
            {
                { InfectionTag.Mutated, "Mutated" },
                { InfectionTag.Poisoned, "Poisoned" },
                { InfectionTag.Slow, "Slow" },
                { InfectionTag.Weak, "Weak" }
            };
        }
    }
}
