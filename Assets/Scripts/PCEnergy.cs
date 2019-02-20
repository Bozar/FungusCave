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
                int bonus1 = actorData.GetIntData(
                        GetComponent<MetaInfo>().SubTag, DataTag.EnergyRestore);
                int bonus2 = (12 - GetComponent<HP>().CurrentHP) * 100;

                int defEnergy1
                    = GetComponent<Power>().IsActive(PowerTag.DefEnergy1)
                    ? bonus1 : 0;
                int defEnergy2
                    = GetComponent<Power>().IsActive(PowerTag.DefEnergy2)
                    ? bonus2 : 0;

                return defEnergy1 + defEnergy2;
            }
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
        }
    }
}
