using Fungus.Actor;
using System;
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

namespace Fungus.GameSystem.Data
{
    public class PowerData : MonoBehaviour
    {
        public int GetPowerCost(PowerTag tag)
        {
            return GetComponent<GameData>().GetIntData("PowerCost", tag);
        }

        public string GetPowerName(PowerTag tag)
        {
            return GetComponent<GameText>().GetStringData("PowerName", tag);
        }

        public PowerTag GetPrerequisite(PowerTag tag)
        {
            string power = GetComponent<GameData>().GetStringData(
                "PowerPrerequisite", tag);

            return (PowerTag)Enum.Parse(typeof(PowerTag), power);
        }
    }
}
