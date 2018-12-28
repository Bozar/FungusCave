using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class ExaminerActions : MonoBehaviour
    {
        private ConvertCoordinates coord;
        private SubGameMode mode;

        private void MoveExaminer()
        {
            int[] target = coord.Convert(
                GetComponent<PlayerInput>().GameCommand(),
                FindObjects.Examiner.transform.position);

            GetComponent<MoveExamineMarker>().MoveGameObject(target);
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
            }
        }
    }
}
