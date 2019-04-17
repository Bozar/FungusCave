using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor
{
    public class Power : MonoBehaviour
    {
        private PowerData powerData;
        private Dictionary<PowerSlotTag, PowerTag> powerDict;

        public void BuyPower(PowerTag power)
        {
            if (IsBuyable(power, out PowerSlotTag slot, out int potion))
            {
                GainPower(slot, power);
                GetComponent<Potion>().LosePotion(potion);
            }
        }

        public void GainPower(PowerSlotTag slot, PowerTag power)
        {
            powerDict[slot] = power;
        }

        public bool HasPower(PowerTag tag)
        {
            foreach (PowerTag pt in powerDict.Values)
            {
                if (tag == pt)
                {
                    return true;
                }
            }
            return false;
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
            foreach (PowerSlotTag slot in Enum.GetValues(typeof(PowerSlotTag)))
            {
                if (HasPower(slot, out PowerTag powerInSlot, out bool isActive)
                    && (power == powerInSlot))
                {
                    return isActive;
                }
            }
            return false;
        }

        public bool IsBuyable(PowerTag power)
        {
            return IsBuyable(power, out _, out _);
        }

        public bool IsBuyable(PowerTag power,
            out PowerSlotTag slot, out int potion)
        {
            PowerSlotTag[] checkSlot = new PowerSlotTag[]
            { PowerSlotTag.Slot1, PowerSlotTag.Slot2, PowerSlotTag.Slot3 };

            bool slotIsFull = true;
            bool preIsMissing = true;
            PowerTag prePower = powerData.GetPrerequisite(power);

            slot = PowerSlotTag.Slot1;
            potion = 0;

            potion = powerData.GetPowerCost(power);
            if (GetComponent<Potion>().CurrentPotion < potion)
            {
                return false;
            }

            foreach (PowerSlotTag s in checkSlot)
            {
                if (powerDict[s] == prePower)
                {
                    preIsMissing = false;
                }
                if (powerDict[s] == PowerTag.INVALID)
                {
                    slotIsFull = false;
                    slot = s;

                    // If current slot is empty, it means we have checked all
                    // powers that PC has and `preIsMissing` will no longer
                    // change. We can break out of the loop safely.
                    break;
                }
            }
            if (slotIsFull || preIsMissing)
            {
                return false;
            }

            return true;
        }

        private void Start()
        {
            powerDict = new Dictionary<PowerSlotTag, PowerTag>();
            foreach (PowerSlotTag slot in Enum.GetValues(typeof(PowerSlotTag)))
            {
                powerDict.Add(slot, PowerTag.INVALID);
            }

            powerData = FindObjects.GameLogic.GetComponent<PowerData>();

            // TODO: Delete these lines.
            //GainPower(PowerSlotTag.Slot1, PowerTag.DefEnergy1);
        }
    }
}
