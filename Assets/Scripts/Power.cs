using Fungus.Actor.ObjectManager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor
{
    public class Power : MonoBehaviour
    {
        private bool isPC;
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

        public bool HasPower(PowerSlotTag slot, out PowerTag tag)
        {
            if (pcPowerDict.TryGetValue(slot, out tag))
            {
                if (tag != PowerTag.INVALID)
                {
                    return true;
                }
            }
            tag = PowerTag.INVALID;
            return false;
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
