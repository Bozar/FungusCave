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
                = "[ Power Slot ]";
            getUI(UITag.BuyPowerSlot1).GetComponent<Text>().text
                = "Adrenaline";
            getUI(UITag.BuyPowerSlot2).GetComponent<Text>().text
               = "Empty";
            getUI(UITag.BuyPowerSlot3).GetComponent<Text>().text
               = "Empty";
            getUI(UITag.BuyPowerPotion).GetComponent<Text>().text
                = "[ Potion: 2 ]";

            getUI(UITag.BuyPowerListLabel).GetComponent<Text>().text
                = "[ Power List ]";
            getUI(UITag.BuyPowerHP1).GetComponent<Text>().text
                = "< First Aid >";
            getUI(UITag.BuyPowerHP2).GetComponent<Text>().text
                = "Reaper";

            getUI(UITag.BuyPowerEnergy1).GetComponent<Text>().text
                = "Vigor";
            getUI(UITag.BuyPowerEnergy2).GetComponent<Text>().text
               = "Adrenaline";

            getUI(UITag.BuyPowerInfection1).GetComponent<Text>().text
                = "Immunity";
            getUI(UITag.BuyPowerInfection2).GetComponent<Text>().text
                = "Fast Heal";

            getUI(UITag.BuyPowerAtkHP).GetComponent<Text>().text
                = "Bleed";
            getUI(UITag.BuyPowerAtkEnergy).GetComponent<Text>().text
               = "Siphon";
            getUI(UITag.BuyPowerAtkInfection).GetComponent<Text>().text
               = "Plague";

            getUI(UITag.BuyPowerDescription).GetComponent<Text>().text
                = "Cost: 5\n\nPrerequisite: First Aid\n\nDescription.";
        }

        private void Start()
        {
            getUI = FindObjects.GetUIObject;
        }
    }
}
