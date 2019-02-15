﻿using Fungus.GameSystem;
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
        private int highRate;
        private InfectionData infectionData;
        private int lowRate;
        private int mediumRate;

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
            int hp = (11 - GetComponent<HP>().CurrentHP) * 5 + 15;
            hp = Math.Max(lowRate, hp);
            hp = Math.Min(mediumRate, hp);

            int poisoned
                = GetComponent<Infection>().HasInfection(InfectionTag.Poisoned)
                ? highRate : 0;

            int pool
                = dungeon.CheckBlock(SubObjectTag.Pool,
                coords.Convert(transform.position))
                ? mediumRate : 0;

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

            highRate = infectionData.HighInfectionRate;
            mediumRate = infectionData.MediumInfectionRate;
            lowRate = infectionData.LowInfectionRate;
        }
    }
}