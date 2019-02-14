using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class DamageData : MonoBehaviour
    {
        public int InfectionWeak { get; private set; }
        public int PowerAttDamage1 { get; private set; }

        private void Awake()
        {
            PowerAttDamage1 = 1;
            InfectionWeak = 1;
        }
    }
}
