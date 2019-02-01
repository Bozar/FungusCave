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
        private int baseDamage;
        private int powerDamage;
        private int weakDamage;

        public int CurrentDamage
        {
            get
            {
                int weak, power, finalDamage;

                weak = GetComponent<Infection>().HasInfection(InfectionTag.Weak)
                    ? weakDamage : 0;
                power = GetComponent<Power>().PowerIsActive(PowerTag.AttDamage1)
                    ? powerDamage : 0;

                finalDamage = baseDamage + power - weak;
                finalDamage = Math.Max(0, finalDamage);

                return finalDamage;
            }
        }

        private void Start()
        {
            baseDamage = FindObjects.GameLogic.GetComponent<ActorData>()
                .GetIntData(GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.Damage);
            powerDamage = FindObjects.GameLogic.GetComponent<PowerData>()
                .AttDamage1;

            weakDamage = GetComponent<Infection>().WeakDamage;
        }
    }
}
