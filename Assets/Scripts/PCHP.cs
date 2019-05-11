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

        public void Count()
        {
            throw new NotImplementedException();
        }

        public void Trigger()
        {
            if ((GetComponent<HP>().CurrentHP
                < (int)Math.Floor(0.5 * GetComponent<HP>().MaxHP))
                && GetComponent<Power>().IsActive(PowerTag.DefHP1))
            {
                GetComponent<HP>().GainHP(hpRestore);
            }

            if ((GetComponent<HP>().CurrentHP
                < (int)Math.Floor(0.8 * GetComponent<HP>().MaxHP))
                && GetComponent<Infection>().HasInfection(out _, out _)
                && GetComponent<Power>().IsActive(PowerTag.DefHP2))
            {
                GetComponent<HP>().GainHP(hpRestore);
            }
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
            hpRestore = actorData.GetIntData(
                GetComponent<MetaInfo>().SubTag, DataTag.HPRestore);
        }
    }
}
