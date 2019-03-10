using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class EnergyData : MonoBehaviour
    {
        public int ActionThreshold { get; private set; }
        public int Attack { get; private set; }
        public int Maximum { get; private set; }
        public int Minimum { get; private set; }
        public int ModHigh { get; private set; }
        public int ModNormal { get; private set; }
        public int Move { get; private set; }
        public int Restore { get; private set; }

        private void Awake()
        {
            ModNormal = 400;
            ModHigh = 800;

            Maximum = 5000;
            Minimum = 0;

            ActionThreshold = 3000;
            Restore = 1000;

            Move = 1000;
            Attack = 1400;
        }
    }
}
