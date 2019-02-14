﻿using Fungus.Actor;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor
{
    public enum PowerSlotTag { Slot1, Slot2, Slot3 }

    public enum PowerTag
    {
        INVALID,

        DefEnergy1, DefEnergy2,
        DefInfection1, DefInfection2,
        DefHP1, DefHP2,

        AttEnergy1, AttEnergy2,
        AttInfection1, AttInfection2,
        AttDamage1, AttDamage2
    }
}

namespace Fungus.GameSystem.ObjectManager
{
    public class PowerData : MonoBehaviour
    {
        private Dictionary<PowerTag, string> powerNames;

        public string GetPowerName(PowerTag tag)
        {
            return powerNames[tag];
        }

        private void Awake()
        {
            powerNames = new Dictionary<PowerTag, string>
            {
                { PowerTag.DefEnergy1, "Vigor I" },
                { PowerTag.DefEnergy2, "Vigor II" },
                { PowerTag.DefInfection1, "Immunity I" },
                { PowerTag.DefInfection2, "Immunity II" },
                { PowerTag.DefHP1, "Health I" },
                { PowerTag.DefHP2, "Health II" },

                { PowerTag.AttEnergy1, "Siphon" },
                { PowerTag.AttInfection1, "Plague" },
                { PowerTag.AttDamage1, "Bleed" }
            };
        }
    }
}
