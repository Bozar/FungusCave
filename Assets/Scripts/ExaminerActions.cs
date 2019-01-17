using Fungus.Actor.AI;
using Fungus.GameSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class ExaminerActions : MonoBehaviour
    {
        private ConvertCoordinates coord;
        private SubGameMode mode;

        private void LockTarget()
        {
            List<GameObject> targets
                = GetComponent<ISortTargets>().SortActorsInSight();

            foreach (GameObject target in targets)
            {
                int[] pos = coord.Convert(target.transform.position);
                Debug.Log(pos[0] + "," + pos[1]);
            }
        }

        private void MoveExaminer()
        {
            int[] target = coord.Convert(
                GetComponent<PlayerInput>().GameCommand(),
                FindObjects.Examiner.transform.position);

            GetComponent<IMove>().MoveGameObject(target);
        }

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubGameMode>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
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
                    Debug.Log("Next");
                    LockTarget();
                    break;

                case Command.Previous:
                    Debug.Log("Previous");
                    break;
            }
        }
    }
}
