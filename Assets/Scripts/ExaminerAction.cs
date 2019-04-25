using Fungus.Actor.AI;
using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class ExaminerAction : MonoBehaviour
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private SubMode mode;

        private void LockTarget(Command direction)
        {
            List<GameObject> targets
                = GetComponent<ISortTargets>().SortActorsInSight();

            if (targets.Count < 1)
            {
                return;
            }

            int[] examiner = coord.Convert(transform.position);
            GameObject currentTarget = actor.GetActor(examiner[0], examiner[1]);
            int currentIndex = targets.IndexOf(currentTarget);

            int newIndex;
            switch (direction)
            {
                case Command.Next:
                    newIndex = ((currentIndex + 1) >= targets.Count)
                        ? 0 : (currentIndex + 1);
                    break;

                case Command.Previous:
                    newIndex = ((currentIndex - 1) < 0)
                        ? (targets.Count - 1) : (currentIndex - 1);
                    break;

                default:
                    return;
            }

            int[] newPos = coord.Convert(targets[newIndex].transform.position);
            GetComponent<IMove>().MoveGameObject(newPos);
        }

        private void MoveExaminer()
        {
            int[] target = coord.Convert(
                GetComponent<PlayerInput>().GameCommand(),
                FindObjects.GetStaticActor(SubObjectTag.Examiner)
                .transform.position);

            GetComponent<IMove>().MoveGameObject(target);
        }

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubMode>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
        }

        private void Update()
        {
            switch (GetComponent<PlayerInput>().GameCommand())
            {
                case Command.Left:
                case Command.Right:
                case Command.Up:
                case Command.Down:
                case Command.UpLeft:
                case Command.UpRight:
                case Command.DownLeft:
                case Command.DownRight:
                    MoveExaminer();
                    break;

                case Command.Cancel:
                    mode.SwitchModeExamine(false);
                    break;

                case Command.Next:
                    LockTarget(Command.Next);
                    break;

                case Command.Previous:
                    LockTarget(Command.Previous);
                    break;
            }
        }
    }
}
