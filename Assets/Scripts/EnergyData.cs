using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class EnergyData : MonoBehaviour
    {
        public int Attack { get; private set; }
        public int InfectionOverflowed { get; private set; }
        public int InfectionSlow { get; private set; }
        public int Maximum { get; private set; }
        public int Move { get; private set; }
        public int Restore { get; private set; }

        private void Awake()
        {
            InfectionOverflowed = 200;
            InfectionSlow = 400;

            Attack = 1200;
            Move = 1000;
            Restore = 1000;
            Maximum = 5000;
        }
    }
}
