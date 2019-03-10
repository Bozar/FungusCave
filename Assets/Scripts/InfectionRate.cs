using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class InfectionRate : MonoBehaviour, IInfection
    {
        private ConvertCoordinates coords;
        private DungeonBoard dungeon;
        private InfectionData infectionData;

        public int InfectionDuration
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string GetHealthReport(HealthTag status)
        {
            throw new NotImplementedException();
        }

        public int GetInfectionRate(GameObject attacker)
        {
            throw new NotImplementedException();
        }

        public int GetInfectionRate()
        {
            int hp = (11 - GetComponent<HP>().CurrentHP) * 10;
            hp = Math.Max(0, hp);
            hp = Math.Min(infectionData.RateNormal, hp);

            int poisoned
                = GetComponent<Infection>().HasInfection(InfectionTag.Poisoned)
                ? infectionData.RateHigh : 0;

            int pool
                = dungeon.CheckBlock(SubObjectTag.Pool,
                coords.Convert(transform.position))
                ? infectionData.RateNormal : 0;

            // TODO: Check weather.
            int fog = 0;

            int final = hp + poisoned + pool + fog;
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
