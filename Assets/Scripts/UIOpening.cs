using Fungus.GameSystem.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIOpening : MonoBehaviour, IUpdateUI
    {
        private UIText getUI;

        private delegate Text UIText(UITag tag);

        public void PrintStaticText()
        {
            return;
        }

        public void PrintStaticText(string text)
        {
            return;
        }

        public void PrintText()
        {
            PrintOpening();
        }

        private void PrintOpening()
        {
            string[] sceneModeline = GetComponent<GameText>().GetOpening();

            getUI(UITag.Opening).text = sceneModeline[0];
            getUI(UITag.OpeningModeline).text = sceneModeline[1];
        }

        private void Start()
        {
            getUI = FindObjects.GetUIText;
        }
    }
}
