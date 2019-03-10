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
        private int countNPC;
        private ObjectPool oPool;
        private int[] position;

        public void Initialize()
        {
            CreateBuilding();
            CreatePC();
            CreateDoppleganger();

            countNPC = 0;
            CreateNPC();
        }

        private void CreateBuilding()
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

        private void CreateDoppleganger()
        {
            oPool.CreateObject(
                MainObjectTag.Doppleganger, SubObjectTag.Examiner, 0, 0);
            oPool.CreateObject(
                MainObjectTag.Doppleganger, SubObjectTag.Guide, 0, 0);
        }

        private void CreateNPC()
        {
            List<SubObjectTag> actors = GetComponent<DungeonActor>().GetActor();

            for (int i = 0; i < actors.Count; i++)
            {
                do
                {
                    position = GetPassablePosition();
                } while (IsTooClose(position));
                oPool.CreateObject(MainObjectTag.Actor, actors[i], position);
            }
        }

        private void CreatePC()
        {
            position = GetPassablePosition();
            oPool.CreateObject(MainObjectTag.Actor, SubObjectTag.PC, position);
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

        private bool IsTooClose(int[] position)
        {
            int minDistance = 9;
            int maxNPC = 3;
            int[] pcPosition = GetComponent<ConvertCoordinates>().Convert(
                FindObjects.PC.transform.position);

            if (GetComponent<DungeonBoard>().GetDistance(pcPosition, position)
                < minDistance)
            {
                countNPC++;
                return countNPC > maxNPC;
            }
            return false;
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
