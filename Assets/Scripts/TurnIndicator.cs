using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class TurnIndicator : MonoBehaviour, ITurnCounter
    {
        private string lineSeparator;
        private int maxTurn;
        private UIMessage message;
        private int minTurn;

        public int BarSpearator { get; private set; }
        public int CurrentTurn { get; private set; }

        public void Count()
        {
            CurrentTurn += 1;
            CurrentTurn = (CurrentTurn > maxTurn) ? minTurn : CurrentTurn;
        }

        public void Trigger()
        {
            if (message.LastLine != lineSeparator)
            {
                message.StoreText(lineSeparator);
            }
        }

        private void Awake()
        {
            maxTurn = 5;
            minTurn = 0;

            // [ X X X | X X ]
            CurrentTurn = minTurn;
            BarSpearator = 3;

            // ----------
            lineSeparator = new string('-', 40);
        }

        private void Start()
        {
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
        }
    }
}
