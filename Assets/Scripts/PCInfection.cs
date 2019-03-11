using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor
{
    public class PCInfection : MonoBehaviour, IInfection
    {
        private ActorData actorData;
        private InfectionData infectionData;

        public int InfectionDuration
        {
            get
            {
                return GetComponent<Power>().IsActive(PowerTag.DefInfection2)
                    ? infectionData.DurationShort
                    : infectionData.DurationNormal;
            }
        }

        public int GetInfectionRate(GameObject attacker)
        {
            // The universal base rate is decided by victim's current HP and
            // environment.
            int baseRate = GetComponent<InfectionRate>().GetInfectionRate();

            // PC's defending power, which is optional and can be purchased by
            // potions.
            int defend
                = GetComponent<Power>().IsActive(PowerTag.DefInfection1)
                ? actorData.GetIntData(GetComponent<MetaInfo>().SubTag,
                DataTag.InfectionDefend)
                : 0;

            // NPC's attacking power is a static value.
            int attack = actorData.GetIntData(
                attacker.GetComponent<MetaInfo>().SubTag,
                DataTag.InfectionAttack);

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
        }
    }
}
