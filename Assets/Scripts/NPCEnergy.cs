using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCEnergy : MonoBehaviour, IEnergy
    {
        private ActorData actorData;

        public int RestoreEveryTurn
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
