using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class TurnIndicator : MonoBehaviour, ITurnCounter
    {
        private GameMessage gameMessage;
        private string lineSeparator;
        private int maxTurn;
        private int minTurn;
        private UIMessage uiMessage;

        public int BarSpearator { get; private set; }
        public int CurrentTurn { get; private set; }

        public void Count()
        {
            CurrentTurn += 1;
            CurrentTurn = (CurrentTurn > maxTurn) ? minTurn : CurrentTurn;
        }

        public void Trigger()
        {
            string[] text = gameMessage.GetText(1);

            if ((text.Length > 0) && (text[0] != lineSeparator))
            {
                uiMessage.StoreText(lineSeparator);
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
            uiMessage = FindObjects.GameLogic.GetComponent<UIMessage>();
            gameMessage = FindObjects.GameLogic.GetComponent<GameMessage>();
        }
    }
}
