using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class PCHP : MonoBehaviour, IHP
    {
        private ActorData actorData;
        private int hpRestore;
        private int hpThreshold;

        public void Count()
        {
            throw new NotImplementedException();
        }

        public void RestoreAfterKill()
        {
            if (GetComponent<Power>().IsActive(PowerTag.DefHP2))
            {
                GetComponent<HP>().GainHP(hpRestore);
            }
        }

        public void Trigger()
        {
            if ((GetComponent<HP>().CurrentHP < hpThreshold)
                && GetComponent<Power>().IsActive(PowerTag.DefHP1))
            {
                GetComponent<HP>().GainHP(hpRestore);
            }
        }

        private void Awake()
        {
            hpThreshold = 5;
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
            hpRestore = actorData.GetIntData(
                GetComponent<MetaInfo>().SubTag, DataTag.HPRestore);
        }
    }
}
