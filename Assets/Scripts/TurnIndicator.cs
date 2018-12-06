using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class TurnIndicator : MonoBehaviour, ITurnCounter
    {
        private int maxTurn;
        private int minTurn;

        public int CurrentTurn { get; private set; }
        public int Seperator { get; private set; }

        public void Count()
        {
            CurrentTurn += 1;
            CurrentTurn = (CurrentTurn > maxTurn) ? minTurn : CurrentTurn;
        }

        private void Awake()
        {
            maxTurn = 5;
            minTurn = 0;

            // [ X X X | X X ]
            CurrentTurn = minTurn;
            Seperator = 3;
        }
    }
}
