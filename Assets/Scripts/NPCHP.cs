using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCHP : MonoBehaviour, IHP
    {
        private ActorData actorData;

        public void Count()
        {
            throw new NotImplementedException();
        }

        public void RestoreAfterKill()
        {
            return;
        }

        public void Trigger()
        {
            GetComponent<HP>().GainHP(actorData.GetIntData(
                GetComponent<MetaInfo>().SubTag, DataTag.HPRestore));
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
        }
    }
}
