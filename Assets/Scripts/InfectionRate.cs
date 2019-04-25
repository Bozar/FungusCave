using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.WorldBuilding;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class InfectionRate : MonoBehaviour, IInfectionRate
    {
        private ConvertCoordinates coords;
        private DungeonBoard dungeon;
        private InfectionData infectionData;

        public int GetInfectionRate(GameObject attacker)
        {
            throw new NotImplementedException();
        }

        public int GetInfectionRate()
        {
            int hp = (11 - GetComponent<HP>().CurrentHP) * 10;
            hp = Math.Max(0, hp);
            hp = Math.Min(infectionData.RateNormal, hp);

            int pool
                = dungeon.CheckBlock(SubObjectTag.Pool,
                coords.Convert(transform.position))
                ? infectionData.RateNormal : 0;

            int final = hp + pool;
            return final;
        }

        private void Start()
        {
            infectionData = FindObjects.GameLogic.GetComponent<InfectionData>();
            dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            coords = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        }
    }
}
