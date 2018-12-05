using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.WorldBuilding
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
        }

        // TODO: Create actors based on dungeon level.
        private void CreateActors()
        {
            position = GetPassablePosition();
            oPool.CreateObject(MainObjectTag.Actor, SubObjectTag.PC, position);

            for (int i = 0; i < 2; i++)
            {
                position = GetPassablePosition();
                oPool.CreateObject(MainObjectTag.Actor, SubObjectTag.Dummy,
                    position);
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

        private int[] GetPassablePosition()
        {
            int[] pos;

            do
            {
                pos = blueprint.RandomIndex();
            } while (actor.HasActor(pos)
            || board.CheckBlock(SubObjectTag.Wall, pos)
            || board.CheckBlock(SubObjectTag.Fungus, pos));

            return pos;
        }

        private void Start()
        {
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
            blueprint = FindObjects.GameLogic.GetComponent<DungeonBlueprint>();
            oPool = FindObjects.GameLogic.GetComponent<ObjectPool>();
        }
    }
}
