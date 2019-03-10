using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class InfectionData : MonoBehaviour
    {
        public int DurationNormal { get; private set; }
        public int DurationShort { get; private set; }
        public int MaxInfections { get; private set; }
        public int RateHigh { get; private set; }
        public int RateNormal { get; private set; }

        private void Awake()
        {
            RateHigh = 80;
            RateNormal = 40;

            DurationShort = 2;
            DurationNormal = 5;

            MaxInfections = 1;
        }
    }
}
