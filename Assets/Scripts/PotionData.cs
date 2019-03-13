using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class PotionData : MonoBehaviour
    {
        public int BonusPotion { get; private set; }
        public int MaxPotion { get; private set; }
        public int ReducedHP { get; private set; }
        public int RelieveStress { get; private set; }
        public int RestoreEnergy { get; private set; }
        public int StartPotion { get; private set; }

        private void Awake()
        {
            MaxPotion = 9;
            StartPotion = 2;
            BonusPotion = 1;

            ReducedHP = 5;
            RelieveStress = 3;
            RestoreEnergy = 9999;
        }
    }
}
