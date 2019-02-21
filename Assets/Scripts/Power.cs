using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor
{
    public class Power : MonoBehaviour
    {
        private Dictionary<PowerSlotTag, PowerTag> powerDict;

        public void GainPower(PowerSlotTag slot, PowerTag power)
        {
            powerDict[slot] = power;
        }

        public bool HasPower(PowerSlotTag slot,
            out PowerTag tag, out bool isActive)
        {
            if (powerDict.TryGetValue(slot, out tag))
            {
                if (tag != PowerTag.INVALID)
                {
                    isActive
                        = GetComponent<Stress>().CurrentStress
                        >= ((int)slot + 1);
                    return true;
                }
            }

            tag = PowerTag.INVALID;
            isActive = false;
            return false;
        }

        public bool IsActive(PowerTag power)
        {
            bool isActive;
            PowerTag powerInSlot;

            foreach (PowerSlotTag slot in Enum.GetValues(typeof(PowerSlotTag)))
            {
                if (HasPower(slot, out powerInSlot, out isActive)
                    && (power == powerInSlot))
                {
                    return isActive;
                }
            }
            return false;
        }

        private void Start()
        {
            powerDict = new Dictionary<PowerSlotTag, PowerTag>();
            foreach (PowerSlotTag slot in Enum.GetValues(typeof(PowerSlotTag)))
            {
                powerDict.Add(slot, PowerTag.INVALID);
            }

            // TODO: Delete these lines.
            GainPower(PowerSlotTag.Slot1, PowerTag.AttEnergy1);
            GainPower(PowerSlotTag.Slot2, PowerTag.AttDamage1);
            GainPower(PowerSlotTag.Slot3, PowerTag.DefInfection2);
        }
    }
}
