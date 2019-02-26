using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class PotionData : MonoBehaviour
    {
        public int BonusPotion { get; private set; }
        public int ReducedHP { get; private set; }

        private void Awake()
        {
            ReducedHP = 5;
            BonusPotion = 1;
        }
    }
}
