using Fungus.Actor;
using UnityEngine;

namespace Fungus.GameSystem.WorldBuilding
{
    public class ActorBoard : MonoBehaviour
    {
        private GameObject[,] board;
        private DungeonBoard dungeon;

        public void AddActor(GameObject actor, int x, int y)
        {
            if (dungeon.IndexOutOfRange(x, y))
            {
                return;
            }

            board[x, y] = actor;
        }

        public bool CheckActorTag<T>(T actorTag, int x, int y)
        {
            return CheckActorTag(actorTag, GetActor(x, y));
        }

        public bool CheckActorTag<T>(T actorTag, GameObject actor)
        {
            bool checkMainTag;
            bool checkSubTag;

            if (actor == null)
            {
                return false;
            }

            checkMainTag = actor.GetComponent<MetaInfo>().MainTag
                .Equals(actorTag);
            checkSubTag = actor.GetComponent<MetaInfo>().SubTag
                .Equals(actorTag);

            return checkMainTag || checkSubTag;
        }

        public GameObject GetActor(int x, int y)
        {
            if (dungeon.IndexOutOfRange(x, y))
            {
                return null;
            }

            return board[x, y];
        }

        public bool HasActor(int x, int y)
        {
            return GetActor(x, y) != null;
        }

        public bool HasActor(int[] position)
        {
            return HasActor(position[0], position[1]);
        }

        public void RemoveActor(int x, int y)
        {
            AddActor(null, x, y);
        }

        private void Start()
        {
            dungeon = GetComponent<DungeonBoard>();
            board = new GameObject[dungeon.Width, dungeon.Height];
        }
    }
}
