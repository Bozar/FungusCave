using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class PCHP : MonoBehaviour, IHP
    {
        private ActorData actorData;
        private int hpRestoreKill;
        private int hpThreshold;

        public void Count()
        {
            throw new NotImplementedException();
        }

        public void RestoreAfterKill()
        {
            if (GetComponent<Power>().IsActive(PowerTag.DefHP2))
            {
                GetComponent<HP>().GainHP(hpRestoreKill);
            }
        }

        public void Trigger()
        {
            if (GetComponent<Power>().IsActive(PowerTag.DefHP1)
                && (GetComponent<HP>().CurrentHP < hpThreshold))
            {
                GetComponent<HP>().GainHP(actorData.GetIntData(
                    GetComponent<MetaInfo>().SubTag, DataTag.HPRestore));
            }
        }

        private void Awake()
        {
            hpThreshold = 4;
            hpRestoreKill = 2;
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
        }
    }
}
