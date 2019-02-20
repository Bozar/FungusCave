using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCInfection : MonoBehaviour, IInfection
    {
        private ActorData actorData;
        private SubObjectTag actorTag;
        private InfectionData infectionData;

        public int InfectionDuration
        {
            get
            {
                return actorData.GetIntData(actorTag, DataTag.InfectionDuration);
            }
        }

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
            // The universal base rate is decided by victim's current HP and
            // environment.
            int baseRate = GetComponent<InfectionRate>().GetInfectionRate();

            // PC's attacking power, which is optional and can be purchased by
            // potions.
            int attack = attacker.GetComponent<Power>().IsActive(
                PowerTag.AttInfection1)
                ? infectionData.MediumInfectionRate : 0;

            // NPC's defending power is a static value.
            int defend = actorData.GetIntData(actorTag, DataTag.InfectionDefend);

            int final = baseRate + attack - defend;
            return final;
        }

        public int GetInfectionRate()
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
            infectionData = FindObjects.GameLogic.GetComponent<InfectionData>();
            actorTag = GetComponent<MetaInfo>().SubTag;
        }
    }
}
