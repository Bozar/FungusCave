using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIPowerBuyer : MonoBehaviour, IUpdateUI
    {
        private UIObject getUI;

        private delegate GameObject UIObject(UITag tag);

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
            getUI(UITag.PowerBuyer).GetComponent<Text>().text
               = "[Defend, HP]\nFirst Aid\nReaper\n\n"
               + "[Defend, Energy]\n\nVigor\nAdrenaline\n\n"
               + "[Defend, Infection]\n\nImmunity\nFast Heal\n\n"
               + "[Attack]\n\nSiphon\nPlague\nBleed";
        }

        private void Start()
        {
            getUI = FindObjects.GetUIObject;
        }
    }
}
