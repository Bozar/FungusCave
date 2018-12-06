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
        Energy1, Energy2,
        Immunity1, Immunity2,
        Damage1, Damage2,
        Poison1, Poison2
    }

    public class Power : MonoBehaviour
    {
        private bool isPC;
        private Dictionary<PowerTag, string> nameDict;
        private Dictionary<PowerTag, bool> npcPowerDict;
        private Dictionary<PowerSlotTag, PowerTag> pcPowerDict;

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
            int currentStress = gameObject.GetComponent<Stress>().CurrentStress;

            return currentSlot <= currentStress;
        }

        private void Awake()
        {
            nameDict = new Dictionary<PowerTag, string>
            {
                { PowerTag.Energy1, "Energy I" },
                { PowerTag.Energy2, "Energy II" },
                { PowerTag.Immunity1, "Immunity I" },
                { PowerTag.Immunity2, "Immunity II" },
                { PowerTag.Damage1, "Damage I" },
                { PowerTag.Damage2, "Damage II" },
                { PowerTag.Poison1, "Poison I" },
                { PowerTag.Poison2, "Poison II" }
            };
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
            isPC = gameObject.GetComponent<ObjectMetaInfo>().SubTag
                == SubObjectTag.PC;

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
