using Fungus.Actor.FOV;
using Fungus.GameSystem;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor
{
    public class MoveExamineMarker : MonoBehaviour, IMove
    {
        private DungeonBoard board;
        private ConvertCoordinates coord;

        public void MoveGameObject(int targetX, int targetY)
        {
            int range = FindObjects.PC.GetComponent<FieldOfView>().MaxRange;
            int[] source = coord.Convert(FindObjects.PC.transform.position);
            int[] target = new int[] { targetX, targetY };

            if (board.IsInsideRange(FOVShape.Square, range, source, target))
            {
                transform.position = coord.Convert(targetX, targetY);
            }
        }

        public void MoveGameObject(int[] position)
        {
            MoveGameObject(position[0], position[1]);
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        }
    }
}
