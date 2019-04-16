using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.ObjectManager;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class PCEnergy : MonoBehaviour, IEnergy
    {
        private ActorData actorData;
        private EnergyData energyData;
        private int maxHP;
        private int minDrain;

        public int Drain
        {
            get
            {
                int energy = Math.Max(
                    minDrain, GetComponent<IDamage>().CurrentDamage * minDrain);

                return GetComponent<Power>().IsActive(PowerTag.AttEnergy1)
                    ? energy : 0;
            }
        }

        public int RestoreTurn
        {
            get
            {
                int bonus1 = actorData.GetIntData(
                        GetComponent<MetaInfo>().SubTag, DataTag.EnergyRestore);
                int bonus2 = (maxHP - GetComponent<HP>().CurrentHP) * 100;

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
            energyData = FindObjects.GameLogic.GetComponent<EnergyData>();

            minDrain = energyData.ModNormal;
            maxHP = 14;
        }
    }
}
