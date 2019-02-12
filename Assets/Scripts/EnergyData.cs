using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class EnergyData : MonoBehaviour
    {
        public int InfectionOverflowed { get; private set; }

        private void Awake()
        {
            InfectionOverflowed = 200;
        }
    }
}
