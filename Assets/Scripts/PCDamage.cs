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
        private int powerDamage1;
        private int powerDamage2;
        private int weakDamage;

        public int CurrentDamage
        {
            get
            {
                int weak, power, finalDamage;

                weak = GetComponent<Infection>().HasInfection(InfectionTag.Weak)
                    ? weakDamage : 0;

                power = GetComponent<Power>().PowerIsActive(PowerTag.Damage1)
                    ? powerDamage1 : 0;
                power += GetComponent<Power>().PowerIsActive(PowerTag.Damage2)
                    ? powerDamage2 : 0;

                finalDamage = baseDamage + power - weak;
                finalDamage = Math.Max(0, finalDamage);

                return finalDamage;
            }
        }

        private void Start()
        {
            baseDamage = FindObjects.GameLogic.GetComponent<ObjectData>()
                .GetIntData(GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.Damage);

            weakDamage = GetComponent<Infection>().WeakDamage;
            powerDamage1 = GetComponent<Power>().Damage1;
            powerDamage2 = GetComponent<Power>().Damage2;
        }
    }
}
