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
        private int weakDamage;

        public int CurrentDamage
        {
            get
            {
                int weak, finalDamage;

                weak = GetComponent<Infection>().HasInfection(InfectionTag.Weak)
                    ? weakDamage : 0;

                finalDamage = baseDamage - weak;
                finalDamage = Math.Max(0, finalDamage);

                return finalDamage;
            }
        }

        private void Start()
        {
            baseDamage = FindObjects.GameLogic.GetComponent<ActorData>()
                .GetIntData(GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.Damage);

            weakDamage = GetComponent<Infection>().WeakDamage;
        }
    }
}
