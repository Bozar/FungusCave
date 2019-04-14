using Fungus.Actor;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class InfectionData : MonoBehaviour
    {
        private string node;

        public int Duration
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "Duration");
            }
        }

        public int MaxDuration
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "DurationMax");
            }
        }

        public int OverflowDuration
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "DurationOverflow");
            }
        }

        public int RateHigh
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "RateHigh");
            }
        }

        public int RateNormal
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "RateNormal");
            }
        }

        public int RecoveryFast
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "RecoveryFast");
            }
        }

        public int RecoveryNormal
        {
            get
            {
                return GetComponent<GameData>().GetIntData(node,
                    "RecoveryNormal");
            }
        }

        public string GetInfectionName(InfectionTag infection)
        {
            return GetComponent<GameText>().GetStringData(
                "InfectionName", infection);
        }

        private void Awake()
        {
            node = "Infection";
        }
    }
}
