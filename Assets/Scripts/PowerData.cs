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
        private Dictionary<PowerTag, PowerTag> prerequisite;

        public int GetPowerCost(PowerTag tag)
        {
            return powerCost[tag];
        }

        public string GetPowerName(PowerTag tag)
        {
            return powerNames[tag];
        }

        public PowerTag GetPrerequisite(PowerTag tag)
        {
            return prerequisite[tag];
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
                { PowerTag.DefEnergy1, 5 },
                { PowerTag.DefInfection1, 5 },
                { PowerTag.DefHP1, 5 },

                { PowerTag.DefEnergy2, 9 },
                { PowerTag.DefInfection2, 9 },
                { PowerTag.DefHP2, 9 },

                { PowerTag.AttEnergy1, 9 },
                { PowerTag.AttInfection1, 9 },
                { PowerTag.AttDamage1, 9 }
            };

            prerequisite = new Dictionary<PowerTag, PowerTag>
            {
                { PowerTag.DefEnergy1, PowerTag.INVALID },
                { PowerTag.DefInfection1, PowerTag.INVALID },
                { PowerTag.DefHP1, PowerTag.INVALID },

                { PowerTag.DefEnergy2, PowerTag.DefEnergy1},
                { PowerTag.DefInfection2, PowerTag.DefInfection1},
                { PowerTag.DefHP2, PowerTag.DefHP1},

                { PowerTag.AttEnergy1, PowerTag.INVALID },
                { PowerTag.AttInfection1, PowerTag.INVALID },
                { PowerTag.AttDamage1, PowerTag.INVALID }
            };
        }
    }
}
