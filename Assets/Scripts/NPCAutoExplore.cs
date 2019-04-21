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

        public bool IsStartPoint(int x, int y)
        {
            return actor.CheckActorTag(SubObjectTag.PC, actor.GetActor(x, y));
        }

        public bool IsValidDestination(int[] check)
        {
            return true;
        }

        private void Start()
        {
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
        }
    }
}
