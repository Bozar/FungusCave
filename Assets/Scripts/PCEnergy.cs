using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor
{
    public class PCEnergy : MonoBehaviour, IEnergy
    {
        private ActorData actorData;

        public int RestoreEveryTurn
        {
            get
            {
                return GetComponent<Power>().IsActive(PowerTag.DefEnergy1)
                    ? actorData.GetIntData(
                        GetComponent<MetaInfo>().SubTag, DataTag.EnergyRestore)
                    : 0;
            }
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
        }
    }
}
