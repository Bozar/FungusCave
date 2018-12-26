using UnityEngine;

namespace Fungus.Actor
{
    public class MoveExamineMarker : MonoBehaviour, IMove
    {
        public void MoveGameObject(int targetX, int targetY)
        {
            Debug.Log(GetComponent<PlayerInput>().GameCommand());
        }

        public void MoveGameObject(int[] position)
        {
            MoveGameObject(position[0], position[1]);
        }
    }
}
