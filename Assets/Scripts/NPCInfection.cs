using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCInfection : MonoBehaviour, IInfectionRate, IInfectionRecovery
    {
        private ActorData actorData;
        private SubObjectTag actorTag;

        public int Recovery
        {
            get
            {
                return actorData.GetIntData(actorTag, DataTag.InfectionRecovery);
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
                ? actorData.GetIntData(actorTag, DataTag.InfectionAttack)
                : 0;

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
            actorTag = GetComponent<MetaInfo>().SubTag;
        }
    }
}
