using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class InfectionData : MonoBehaviour
    {
        public int HighInfectionRate { get; private set; }
        public int LowInfectionRate { get; private set; }
        public int MaxInfections { get; private set; }
        public int MediumInfectionRate { get; private set; }
        public int NormalDuration { get; private set; }
        public int ShortDuration { get; private set; }

        private void Awake()
        {
            HighInfectionRate = 80;
            MediumInfectionRate = 50;
            LowInfectionRate = 20;

            ShortDuration = 2;
            NormalDuration = 5;

            MaxInfections = 1;
        }
    }
}
