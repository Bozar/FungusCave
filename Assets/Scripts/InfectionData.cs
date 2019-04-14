using Fungus.Actor;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class InfectionData : MonoBehaviour
    {
        public int Duration { get; private set; }

        public int MaxDuration { get; private set; }

        public int OverflowDuration { get; private set; }

        public int RateHigh { get; private set; }

        public int RateNormal { get; private set; }

        public int RecoveryFast { get; private set; }

        public int RecoveryNormal { get; private set; }

        public string GetInfectionName(InfectionTag infection)
        {
            return GetComponent<GameText>().GetStringData(
                "InfectionName", infection);
        }

        private void Awake()
        {
            RateHigh = 80;
            RateNormal = 40;

            MaxDuration = 9;
            OverflowDuration = 1;
            Duration = 5;

            RecoveryNormal = 1;
            RecoveryFast = 2;
        }
    }
}
