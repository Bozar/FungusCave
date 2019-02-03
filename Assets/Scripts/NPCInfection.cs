using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCInfection : MonoBehaviour, IInfection
    {
        private int mediumRate;

        public string GetHealthReport(HealthTag status)
        {
            switch (status)
            {
                case HealthTag.Infected:
                    return "NPC is infected.";

                case HealthTag.Overflowed:
                    return "NPC looks exhausted.";

                default:
                    return "";
            }
        }

        public int GetInfectionRate(GameObject attacker)
        {
            int baseRate = GetComponent<InfectionRate>().GetInfectionRate();
            int defend = GetComponent<Infection>().ImmunityRate;

            int attack
                = attacker.GetComponent<Power>().PowerIsActive(
                    PowerTag.AttInfection1)
                    ? mediumRate : 0;

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
