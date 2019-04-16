using Fungus.GameSystem.Data;
using Fungus.GameSystem.ObjectManager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.WorldBuilding
{
    public class ActorGroup : MonoBehaviour
    {
        private ProgressData progress;

        public SubObjectTag[] GetActor()
        {
            DungeonLevel level = (DungeonLevel)Enum.Parse(typeof(DungeonLevel),
                progress.CurrentDungeonLevel);
            int maxSoldier = progress.MaxSoldier;
            int maxActor = progress.MaxActor;
            int potion = 0;
            int nextIndex;
            Stack<SubObjectTag> actors = new Stack<SubObjectTag>();

            Dictionary<CombatRoleTag, SubObjectTag[]> group
                = GetComponent<ActorGroupData>().GetActorGroup(level);
            SubObjectTag[] soldier = group[CombatRoleTag.Soldier];
            SubObjectTag minion = group[CombatRoleTag.Minion][0];

            while (potion < maxSoldier)
            {
                nextIndex = GetComponent<RandomNumber>().Next(
                    GetComponent<DungeonBlueprint>().Seed, 0, soldier.Length);
                actors.Push(soldier[nextIndex]);

                potion += GetComponent<ActorData>().GetIntData(
                    soldier[nextIndex], DataTag.Potion);
            }

            while (actors.Count < maxActor)
            {
                actors.Push(minion);
            }

            return actors.ToArray();
        }

        private void Start()
        {
            progress = GetComponent<ProgressData>();
        }
    }
}
