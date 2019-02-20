using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class EnergyData : MonoBehaviour
    {
        public int ActionThreshold { get; private set; }
        public int Attack { get; private set; }
        public int InfectionOverflowed { get; private set; }
        public int InfectionSlow { get; private set; }
        public int Maximum { get; private set; }
        public int Move { get; private set; }
        public int Restore { get; private set; }

        private void Awake()
        {
            ActionThreshold = 2000;
            Restore = 1000;
            Maximum = 3000;

            Attack = 1400;
            Move = 1000;

            InfectionOverflowed = 200;
            InfectionSlow = 600;
        }
    }
}
