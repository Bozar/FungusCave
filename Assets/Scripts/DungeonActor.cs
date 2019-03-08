using Fungus.GameSystem.ObjectManager;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.WorldBuilding
{
    public class DungeonActor : MonoBehaviour
    {
        public List<SubObjectTag> GetActor()
        {
            // TODO: Change composition based on dungeon level.
            int maxSoldier = 20;
            int maxActor = 50;
            int potion = 0;
            int nextIndex;

            List<SubObjectTag> actors = new List<SubObjectTag>();
            List<SubObjectTag> soldier = GetComponent<ActorGroupData>()
                .GetSoldier(ActorGroupTag.Fungus);
            List<SubObjectTag> minion = GetComponent<ActorGroupData>()
                .GetMinion(ActorGroupTag.Fungus);

            while (potion < maxSoldier)
            {
                nextIndex = GetComponent<RandomNumber>().Next(
                    SeedTag.Dungeon, 0, soldier.Count);
                actors.Add(soldier[nextIndex]);

                potion += GetComponent<ActorData>().GetIntData(
                    soldier[nextIndex], DataTag.Potion);
            }

            while (actors.Count < maxActor)
            {
                nextIndex = GetComponent<RandomNumber>().Next(
                    SeedTag.Dungeon, 0, minion.Count);
                actors.Add(minion[nextIndex]);
            }

            return actors;
        }
    }
}
