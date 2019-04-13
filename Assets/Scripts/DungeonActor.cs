using Fungus.GameSystem.ObjectManager;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.WorldBuilding
{
    public class DungeonActor : MonoBehaviour
    {
        private ProgressData progress;

        public List<SubObjectTag> GetActor()
        {
            // TODO: Change composition based on dungeon level.
            int maxSoldier = progress.MaxSoldier;
            int maxActor = progress.MaxActor;
            int potion = 0;
            int nextIndex;

            List<SubObjectTag> actors = new List<SubObjectTag>();
            List<SubObjectTag> soldier = GetComponent<ActorGroupData>()
                .GetSoldier(ActorGroupTag.Fungus);
            SubObjectTag minion = SubObjectTag.Beetle;

            while (potion < maxSoldier)
            {
                nextIndex = GetComponent<RandomNumber>().Next(
                    GetComponent<DungeonBlueprint>().Seed, 0, soldier.Count);
                actors.Add(soldier[nextIndex]);

                potion += GetComponent<ActorData>().GetIntData(
                    soldier[nextIndex], DataTag.Potion);
            }

            while (actors.Count < maxActor)
            {
                actors.Add(minion);
            }

            return actors;
        }

        private void Start()
        {
            progress = GetComponent<ProgressData>();
        }
    }
}
