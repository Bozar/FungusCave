using UnityEngine;

namespace Fungus.GameSystem.Data
{
    public class PotionData : MonoBehaviour
    {
        private string node;

        public int BonusPotion
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "Bonus");
            }
        }

        public int MaxPotion
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "Max");
            }
        }

        public int MutatedHP
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "MutatedHP");
            }
        }

        public int RelieveStress
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "RelieveStress");
            }
        }

        public int RestoreEnergy
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "RestoreEnergy");
            }
        }

        public int RestoreHP
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "RestoreHP");
            }
        }

        public int StartPotion
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "Start");
            }
        }

        private void Awake()
        {
            node = "Potion";
        }
    }
}
