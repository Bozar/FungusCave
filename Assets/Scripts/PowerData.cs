using Fungus.Actor;
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

        AttEnergy1, AttInfection1, AttDamage1
    }
}

namespace Fungus.GameSystem.ObjectManager
{
    public class PowerData : MonoBehaviour
    {
        private Dictionary<PowerTag, int> powerCost;
        private Dictionary<PowerTag, string> powerNames;

        public int GetPowerCost(PowerTag tag)
        {
            return powerCost[tag];
        }

        public string GetPowerName(PowerTag tag)
        {
            return powerNames[tag];
        }

        private void Awake()
        {
            powerNames = new Dictionary<PowerTag, string>
            {
                { PowerTag.DefEnergy1, "Vigor" },
                { PowerTag.DefEnergy2, "Adrenaline" },
                { PowerTag.DefInfection1, "Immunity" },
                { PowerTag.DefInfection2, "Fast Heal" },
                { PowerTag.DefHP1, "First Aid" },
                { PowerTag.DefHP2, "Reaper" },

                { PowerTag.AttEnergy1, "Siphon" },
                { PowerTag.AttInfection1, "Plague" },
                { PowerTag.AttDamage1, "Bleed" }
            };

            powerCost = new Dictionary<PowerTag, int>
            {
                { PowerTag.DefEnergy1, 3 },
                { PowerTag.DefInfection1, 3 },
                { PowerTag.DefHP1, 3 },

                { PowerTag.DefEnergy2, 6 },
                { PowerTag.DefInfection2, 6 },
                { PowerTag.DefHP2, 6 },

                { PowerTag.AttEnergy1, 6 },
                { PowerTag.AttInfection1, 6 },
                { PowerTag.AttDamage1, 6 }
            };
        }
    }
}
