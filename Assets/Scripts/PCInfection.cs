using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor
{
    public class PCInfection : MonoBehaviour, IInfection
    {
        private int mediumRate;

        public string GetHealthReport(InfectionStatus status)
        {
            switch (status)
            {
                case InfectionStatus.Stressed:
                    return "You feel stressed.";

                case InfectionStatus.Infected:
                    return "You are infected.";

                case InfectionStatus.Overflowed:
                    return "You feel exhausted.";

                default:
                    return "";
            }
        }

        public int GetInfectionRate(GameObject attacker)
        {
            int baseRate = GetComponent<InfectionRate>().GetInfectionRate();
            int defend
                = GetComponent<Power>().PowerIsActive(PowerTag.DefInfection1)
                ? mediumRate : 0;

            int attack = attacker.GetComponent<Infection>().InfectionRate;

            int final = baseRate + attack - defend;
            return final;
        }

        public int GetInfectionRate()
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            mediumRate = FindObjects.GameLogic.GetComponent<InfectionData>()
                .MediumInfectionRate;
        }
    }
}
