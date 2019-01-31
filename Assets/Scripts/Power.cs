using Fungus.Actor.ObjectManager;
using System;
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

    public class Power : MonoBehaviour
    {
        private bool isPC;
        private Dictionary<PowerTag, string> nameDict;
        private Dictionary<PowerTag, bool> npcPowerDict;
        private Dictionary<PowerSlotTag, PowerTag> pcPowerDict;

        public int Damage1 { get; private set; }
        public int Damage2 { get; private set; }

        public void GainPower(PowerSlotTag slot, PowerTag power)
        {
            if (isPC)
            {
                pcPowerDict[slot] = power;
            }
            else
            {
                GainPower(power);
            }
        }

        public void GainPower(PowerTag power)
        {
            if (isPC)
            {
                throw new Exception("Missing argument: power slot.");
            }

            if (!npcPowerDict.ContainsKey(power))
            {
                npcPowerDict.Add(power, false);
            }
        }

        public string GetPowerName(PowerSlotTag slot)
        {
            if (!isPC || (pcPowerDict[slot] == PowerTag.INVALID))
            {
                return "";
            }
            return nameDict[pcPowerDict[slot]];
        }

        public string GetPowerName(PowerTag tag)
        {
            return nameDict[tag];
        }

        public bool HasPower(PowerTag tag)
        {
            // TODO: Update after Unity 2018.3.
            bool isActive;

            if (isPC)
            {
                return PCHasPower(tag, out isActive);
            }
            return npcPowerDict.ContainsKey(tag);
        }

        public bool HasPower()
        {
            foreach (PowerTag tag in Enum.GetValues(typeof(PowerTag)))
            {
                if ((tag != PowerTag.INVALID) && HasPower(tag))
                {
                    return true;
                }
            }
            return false;
        }

        public bool PowerIsActive(PowerTag tag)
        {
            bool isActive;

            if (isPC)
            {
                if (PCHasPower(tag, out isActive))
                {
                    return isActive;
                }
            }
            else
            {
                if (npcPowerDict.TryGetValue(tag, out isActive))
                {
                    return isActive;
                }
            }
            return false;
        }

        public bool SlotIsActive(PowerSlotTag slot)
        {
            if (!isPC)
            {
                return false;
            }

            int currentSlot = (int)slot + 1;
            int currentStress = GetComponent<Stress>().CurrentStress;

            return currentSlot <= currentStress;
        }

        private void Awake()
        {
            nameDict = new Dictionary<PowerTag, string>
            {
                { PowerTag.DefEnergy1, "Vigor I" },
                { PowerTag.DefEnergy2, "Vigor II" },
                { PowerTag.DefInfection1, "Immunity I" },
                { PowerTag.DefInfection2, "Immunity II" },
                { PowerTag.DefHP1, "Health I" },
                { PowerTag.DefHP2, "Health II" },

                { PowerTag.AttEnergy1, "Siphon I" },
                { PowerTag.AttEnergy2, "Siphon II" },
                { PowerTag.AttInfection1, "Poison I" },
                { PowerTag.AttInfection2, "Poison II" },
                { PowerTag.AttDamage1, "Damage I" },
                { PowerTag.AttDamage2, "Damage II" }
            };

            // Damage increased by power.
            Damage1 = 1;
            Damage2 = 1;
        }

        private bool PCHasPower(PowerTag tag, out bool isActive)
        {
            foreach (var slotPower in pcPowerDict)
            {
                if (slotPower.Value == tag)
                {
                    isActive = SlotIsActive(slotPower.Key);
                    return true;
                }
            }

            isActive = false;
            return false;
        }

        private void Start()
        {
            isPC = GetComponent<ObjectMetaInfo>().IsPC;

            if (isPC)
            {
                pcPowerDict = new Dictionary<PowerSlotTag, PowerTag>();
                foreach (PowerSlotTag tag in Enum.GetValues(typeof(PowerSlotTag)))
                {
                    pcPowerDict.Add(tag, PowerTag.INVALID);
                }
            }
            else
            {
                npcPowerDict = new Dictionary<PowerTag, bool>();
            }
        }
    }
}
