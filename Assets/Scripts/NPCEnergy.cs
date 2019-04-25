using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCEnergy : MonoBehaviour, IEnergy
    {
        private ActorData actorData;

        public int Drain
        {
            get
            {
                return actorData.GetIntData(
                    GetComponent<MetaInfo>().SubTag, DataTag.EnergyDrain);
            }
        }

        public int RestoreTurn
        {
            get
            {
                return actorData.GetIntData(
                    GetComponent<MetaInfo>().SubTag, DataTag.EnergyRestore);
            }
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
        }
    }
}
