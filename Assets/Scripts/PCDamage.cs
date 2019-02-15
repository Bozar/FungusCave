using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IDamage
    {
        int CurrentDamage { get; }
    }

    public class PCDamage : MonoBehaviour, IDamage
    {
        private ActorData actorData;
        private DamageData damageData;

        public int CurrentDamage
        {
            get
            {
                int weak, power, baseDamage, finalDamage;

                baseDamage = actorData.GetIntData(
                    GetComponent<MetaInfo>().SubTag, DataTag.Damage);
                weak = GetComponent<Infection>().HasInfection(InfectionTag.Weak)
                    ? damageData.InfectionWeak : 0;
                power = GetComponent<Power>().PowerIsActive(PowerTag.AttDamage1)
                    ? damageData.PowerAttDamage1 : 0;

                finalDamage = baseDamage + power - weak;
                finalDamage = Math.Max(0, finalDamage);

                return finalDamage;
            }
        }

        private void Start()
        {
            damageData = FindObjects.GameLogic.GetComponent<DamageData>();
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
        }
    }
}
