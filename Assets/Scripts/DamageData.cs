using UnityEngine;

namespace Fungus.GameSystem.Data
{
    public class DamageData : MonoBehaviour
    {
        public int InfectionWeak
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Damage",
                    "InfectionWeak");
            }
        }

        public int PowerAttDamage1
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Damage",
                    "PowerBleed");
            }
        }
    }
}
