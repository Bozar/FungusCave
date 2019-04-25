using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IMove
    {
        void MoveGameObject(int targetX, int targetY);

        void MoveGameObject(int[] position);
    }

    public class MoveActor : MonoBehaviour, IMove
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private UIModeline modeline;
        private DungeonTerrain terrain;

        public void MoveGameObject(int[] position)
        {
            MoveGameObject(position[0], position[1]);
        }

        public void MoveGameObject(int targetX, int targetY)
        {
            int[] start = coord.Convert(transform.position);

            if (!GetComponent<Energy>().HasEnoughEnergy())
            {
                return;
            }

            if (!terrain.IsPassable(targetX, targetY))
            {
                // Auto-explore does not work sometimes and will let the actor
                // bump into wall. This is a workaround to prevent printing an
                // error message for NPC.
                if (GetComponent<MetaInfo>().IsPC)
                {
                    modeline.PrintStaticText("You are blocked");
                }
                return;
            }

            terrain.ChangeStatus(true, start[0], start[1]);
            terrain.ChangeStatus(false, targetX, targetY);

            actor.RemoveActor(start[0], start[1]);
            actor.AddActor(gameObject, targetX, targetY);

            transform.position = coord.Convert(targetX, targetY);

            int moveEnergy = GetComponent<Energy>().GetMoveEnergy(
                start, new int[2] { targetX, targetY });
            GetComponent<Energy>().LoseEnergy(moveEnergy);
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();
        }
    }
}
