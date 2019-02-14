using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCDamage : MonoBehaviour, IDamage
    {
        private int baseDamage;
        private int infectionDamage;

        public int CurrentDamage
        {
            get
            {
                int weak, finalDamage;

                weak = GetComponent<Infection>().HasInfection(InfectionTag.Weak)
                    ? infectionDamage : 0;

                finalDamage = baseDamage - weak;
                finalDamage = Math.Max(0, finalDamage);

                return finalDamage;
            }
        }

        private void Start()
        {
            baseDamage = FindObjects.GameLogic.GetComponent<ActorData>()
                .GetIntData(GetComponent<MetaInfo>().SubTag,
                DataTag.Damage);
            infectionDamage = FindObjects.GameLogic.GetComponent<DamageData>()
               .InfectionWeak;
        }
    }
}
