using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class NPCAutoExplore : MonoBehaviour, IAutoExplore
    {
        private ActorBoard actor;

        public SeedTag GetSeedTag()
        {
            return SeedTag.NPCAction;
        }

        public bool GetStartPoint(out int[] startPoint)
        {
            startPoint = new int[2];
            return false;
        }

        public bool IsStartPoint(int[] position)
        {
            int x = position[0];
            int y = position[1];

            return actor.CheckActorTag(SubObjectTag.PC, actor.GetActor(x, y));
        }

        private void Start()
        {
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
        }
    }
}
