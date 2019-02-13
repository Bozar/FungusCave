using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCInfection : MonoBehaviour, IInfection
    {
        private int mediumRate;

        public int InfectionDuration { get; private set; }

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
            // The universal base rate is decided by actor's current HP and
            // environment.
            int baseRate = GetComponent<InfectionRate>().GetInfectionRate();

            // NPC's defending power, which is a static value.
            int defend = GetComponent<InfectionRate>().Defend;

            // PC's attacking power, which is optional and can be purchased by
            // potions.
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

            InfectionDuration = FindObjects.GameLogic.GetComponent<ActorData>()
                .GetIntData(GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.InfectionDuration);
        }
    }
}
