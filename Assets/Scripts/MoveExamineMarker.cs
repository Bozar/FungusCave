using Fungus.Actor.FOV;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor
{
    public class MoveExamineMarker : MonoBehaviour, IMove
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private DungeonBoard dungeon;
        private FieldOfView fov;
        private SubMode mode;

        public void MoveGameObject(int targetX, int targetY)
        {
            int range = FindObjects.PC.GetComponent<FieldOfView>().MaxRange;
            int[] source = coord.Convert(FindObjects.PC.transform.position);
            int[] target = new int[] { targetX, targetY };

            if (dungeon.IsInsideRange(FOVShape.Square, range, source, target))
            {
                transform.position = coord.Convert(targetX, targetY);
            }
            else
            {
                return;
            }

            if (fov.CheckFOV(FOVStatus.Insight, target)
                && actor.HasActor(target)
                && !actor.CheckActorTag(SubObjectTag.PC, target[0], target[1]))
            {
                mode.SwitchUIExamineMessage(true);
            }
            else
            {
                mode.SwitchUIExamineMessage(false);
            }
            return;
        }

        public void MoveGameObject(int[] position)
        {
            MoveGameObject(position[0], position[1]);
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
            mode = FindObjects.GameLogic.GetComponent<SubMode>();

            fov = FindObjects.PC.GetComponent<FieldOfView>();
        }
    }
}
