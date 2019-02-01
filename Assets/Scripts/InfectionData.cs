using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class InfectionData : MonoBehaviour
    {
        public int HighInfectionRate { get; private set; }
        public int LowInfectionRate { get; private set; }
        public int MediumInfectionRate { get; private set; }

        private void Awake()
        {
            HighInfectionRate = 80;
            MediumInfectionRate = 50;
            LowInfectionRate = 20;
        }
    }
}
