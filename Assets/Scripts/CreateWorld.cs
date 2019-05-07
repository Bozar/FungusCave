using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.GameSystem.WorldBuilding
{
    public interface ILoadActorData
    {
        bool LoadedActorData { get; }

        void Load(DTActor data);
    }

    public class CreateWorld : MonoBehaviour, IInitialize
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
            CreateDoppleganger();

            if (GetComponent<SchedulingSystem>().ActorData == null)
            {
                countNPC = 0;
                CreatePC();
                CreateNPC();
            }
            else
            {
                LoadActor(GetComponent<SchedulingSystem>().ActorData);
            }
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
            SubObjectTag[] actors = new SubObjectTag[]
            {
                SubObjectTag.Examiner,
                SubObjectTag.Guide,
                SubObjectTag.BuyPower,
                SubObjectTag.ViewHelp,
                SubObjectTag.ViewLog,
                SubObjectTag.Setting,
                SubObjectTag.Opening,
            };

            foreach (SubObjectTag tag in actors)
            {
                oPool.CreateObject(MainObjectTag.Doppleganger, tag, -9, 0);
            }
        }

        private void CreateNPC()
        {
            SubObjectTag[] actors = GetComponent<ActorGroup>().GetActor();

            for (int i = 0; i < actors.Length; i++)
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
            int minDistance = 1 + GetComponent<ActorData>().GetIntData(
                SubObjectTag.DEFAULT, DataTag.SightRange);
            int maxNPC = 1;
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

        private void LoadActor(DTActor[] data)
        {
            GameObject go;
            ILoadActorData[] lad;

            foreach (DTActor a in data)
            {
                go = oPool.CreateObject(MainObjectTag.Actor,
                    a.ActorTag, a.Position);

                lad = go.GetComponents<ILoadActorData>();
                foreach (ILoadActorData l in lad)
                {
                    l.Load(a);
                }
            }
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
