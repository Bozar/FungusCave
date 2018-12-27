using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor
{
    public class MoveExamineMarker : MonoBehaviour, IMove
    {
        private ConvertCoordinates coord;

        public void MoveGameObject(int targetX, int targetY)
        {
            transform.position = coord.Convert(targetX, targetY);
        }

        public void MoveGameObject(int[] position)
        {
            MoveGameObject(position[0], position[1]);
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        }
    }
}
