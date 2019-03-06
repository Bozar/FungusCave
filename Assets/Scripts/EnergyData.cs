using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class EnergyData : MonoBehaviour
    {
        public int ActionThreshold { get; private set; }
        public int Attack { get; private set; }
        public int BonusRestoreHigh { get; private set; }
        public int BonusRestoreNormal { get; private set; }
        public int DrainHigh { get; private set; }
        public int DrainLow { get; private set; }
        public int DrainMedium { get; private set; }
        public int Maximum { get; private set; }
        public int Minimum { get; private set; }
        public int Move { get; private set; }
        public int Restore { get; private set; }

        private void Awake()
        {
            DrainLow = 200;
            DrainMedium = 400;
            DrainHigh = 600;

            Maximum = 4000;
            Minimum = 0;

            ActionThreshold = 3000;
            Restore = 1000;
            BonusRestoreNormal = 400;
            BonusRestoreHigh = 800;

            Move = 1000;
            Attack = 1400;
        }
    }
}
