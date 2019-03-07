using Fungus.GameSystem.ObjectManager;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.WorldBuilding
{
    public class CreateWorld : MonoBehaviour
    {
        private ActorBoard actor;
        private DungeonBlueprint blueprint;
        private DungeonBoard board;
        private ObjectPool oPool;
        private int[] position;

        public void Initialize()
        {
            CreateBuildings();
            CreateActors();
            CreateDopplegangers();
        }

        // TODO: Create actors based on dungeon level.
        private void CreateActors()
        {
            position = GetPassablePosition();
            oPool.CreateObject(MainObjectTag.Actor, SubObjectTag.PC, position);

            // NOTE: 30 enemies seem to be fine.
            int maxDummies = 10;
            List<SubObjectTag> soldier = GetComponent<ActorGroupData>()
                .GetSoldier(ActorGroupTag.Fungus);
            SubObjectTag nextSoldier;
            int nextIndex;

            for (int i = 0; i < maxDummies; i++)
            {
                nextIndex = GetComponent<RandomNumber>().Next(
                    SeedTag.Dungeon, 0, soldier.Count);
                nextSoldier = soldier[nextIndex];

                position = GetPassablePosition();

                oPool.CreateObject(MainObjectTag.Actor, nextSoldier, position);
            }
        }

        private void CreateBuildings()
        {
            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    if (!board.CheckBlock(SubObjectTag.Floor, x, y))
                    {
                        oPool.CreateObject(
                            MainObjectTag.Building, board.GetBlockTag(x, y),
                            x, y);
                    }
                }
            }
        }

        private void CreateDopplegangers()
        {
            oPool.CreateObject(
                MainObjectTag.Doppleganger, SubObjectTag.Examiner, 0, 0);
            oPool.CreateObject(
                MainObjectTag.Doppleganger, SubObjectTag.Guide, 0, 0);
        }

        private int[] GetPassablePosition()
        {
            int[] pos;

            do
            {
                pos = blueprint.RandomIndex();
            } while (board.CheckBlock(SubObjectTag.Wall, pos)
            || board.CheckBlock(SubObjectTag.Fungus, pos)
            || actor.HasActor(pos));

            return pos;
        }

        private void Start()
        {
            board = GetComponent<DungeonBoard>();
            actor = GetComponent<ActorBoard>();
            blueprint = GetComponent<DungeonBlueprint>();
            oPool = GetComponent<ObjectPool>();
        }
    }
}
