using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using UnityEngine;

namespace Fungus.Actor
{
    public class PCInfection : MonoBehaviour, IInfectionRate, IInfectionRecovery
    {
        private ActorData actorData;
        private InfectionData infectionData;

        public int Recovery
        {
            get
            {
                return GetComponent<Power>().IsActive(PowerTag.DefInfection2)
                    ? infectionData.RecoveryFast
                    : infectionData.RecoveryNormal;
            }
        }

        public int GetInfectionRate(GameObject attacker)
        {
            // The universal base rate is decided by victim's current HP and
            // environment.
            int baseRate = GetComponent<InfectionRate>().GetInfectionRate();

            // NPC's attacking power is a static value.
            int attack = actorData.GetIntData(
                attacker.GetComponent<MetaInfo>().SubTag,
                DataTag.InfectionAttack);

            int final = baseRate + attack;
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
        }
    }
}
