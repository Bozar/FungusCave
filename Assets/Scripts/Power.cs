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
            PowerSlotTag slot;
            int potion;

            if (IsBuyable(power, out slot, out potion))
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

        public bool IsBuyable(PowerTag power)
        {
            PowerSlotTag slot;
            int potion;

            return IsBuyable(power, out slot, out potion);
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

        private void GainRandomPowers()
        {
            int candidate;
            List<int> powerIndex = new List<int>();
            List<PowerTag> availablePowers = new List<PowerTag>()
            {
                PowerTag.DefEnergy1,
                PowerTag.DefEnergy2,
                PowerTag.DefInfection1,
                PowerTag.DefInfection2,
                PowerTag.DefHP1,
                PowerTag.DefHP2,
                PowerTag.AttEnergy1,
                PowerTag.AttInfection1,
                PowerTag.AttDamage1
            };

            for (int i = 0; i < 3; i++)
            {
                do
                {
                    candidate
                        = FindObjects.GameLogic.GetComponent<RandomNumber>()
                        .Next(SeedTag.Dungeon, 0, availablePowers.Count);
                } while (powerIndex.IndexOf(candidate) > -1);

                powerIndex.Add(candidate);
                GainPower((PowerSlotTag)i, availablePowers[candidate]);
            }
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
            //GainRandomPowers();
            //GainPower(PowerSlotTag.Slot1, PowerTag.DefEnergy1);
        }
    }
}
