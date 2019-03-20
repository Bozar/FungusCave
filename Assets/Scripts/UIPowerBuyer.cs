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
            getUI(UITag.BuyPowerSlotLabel).GetComponent<Text>().text
                = "Power Slot";
            getUI(UITag.BuyPowerSlot1).GetComponent<Text>().text
                = "[ Adrenaline ]";
            getUI(UITag.BuyPowerSlot2).GetComponent<Text>().text
               = "[ Empty ]";
            getUI(UITag.BuyPowerSlot3).GetComponent<Text>().text
               = "[ Empty ]";
            getUI(UITag.BuyPowerPotion).GetComponent<Text>().text
                = "Potion: 2";

            getUI(UITag.BuyPowerList).GetComponent<Text>().text
               = "[Defend, HP]\n\nFirst Aid\nReaper\n\n"
               + "[Defend, Energy]\n\nVigor\nAdrenaline\n\n"
               + "[Defend, Infection]\n\nImmunity\nFast Heal\n\n"
               + "[Attack]\n\nSiphon\nPlague\nBleed";

            getUI(UITag.BuyPowerDescription).GetComponent<Text>().text
                = "Cost: 5\n\nPrerequisite: First Aid\n\nDescription.";
        }

        private void Start()
        {
            getUI = FindObjects.GetUIObject;
        }
    }
}
