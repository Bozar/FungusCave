using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class EnergyData : MonoBehaviour
    {
        public int ActionThreshold
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Energy",
                    "ActionThreshold");
            }
        }

        public int Attack
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Energy",
                    "Attack");
            }
        }

        public int Maximum
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Energy",
                    "Maximum");
            }
        }

        public int Minimum
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Energy",
                    "Minimum");
            }
        }

        public int ModHigh
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Energy",
                    "ModHigh");
            }
        }

        public int ModNormal
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Energy",
                    "ModNormal");
            }
        }

        public int Move
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Energy",
                    "Move");
            }
        }

        public int Restore
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Energy",
                    "Restore");
            }
        }
    }
}
