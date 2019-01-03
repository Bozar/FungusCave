using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class NPCAutoExplore : MonoBehaviour, IAutoExplore
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;

        public SeedTag GetSeedTag()
        {
            return SeedTag.NPCAction;
        }

        public bool GetStartPoint(out Stack<int[]> startPoint)
        {
            int[] pcPosition = coord.Convert(FindObjects.PC.transform.position);

            startPoint = new Stack<int[]>();
            startPoint.Push(pcPosition);

            return true;
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
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        }
    }
}
