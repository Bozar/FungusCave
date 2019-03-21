using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIPowerBuyer : MonoBehaviour, IUpdateUI
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
            PrintPowerSlot();
            PrintPowerList();
            PrintPowerDescription();
        }

        private void PrintPowerDescription()
        {
            getUI(UITag.BuyPowerName).text = "[ First Aid ]";
            getUI(UITag.BuyPowerCost).text = "Cost: 5/8";
            getUI(UITag.BuyPowerDescription).text
                = "Restore 1 HP at the start of every turn.";
        }

        private void PrintPowerList()
        {
            getUI(UITag.BuyPowerListLabel).text = "[ Power List ]";
            getUI(UITag.BuyPowerHP1).text = "< First Aid >";
            getUI(UITag.BuyPowerHP2).text = "Reaper";

            getUI(UITag.BuyPowerEnergy1).text = "Vigor";
            getUI(UITag.BuyPowerEnergy2).text = "Adrenaline";

            getUI(UITag.BuyPowerInfection1).text = "Immunity";
            getUI(UITag.BuyPowerInfection2).text = "Fast Heal";

            getUI(UITag.BuyPowerAtkHP).text = "Bleed";
            getUI(UITag.BuyPowerAtkEnergy).text = "Siphon";
            getUI(UITag.BuyPowerAtkInfection).text = "Plague";
        }

        private void PrintPowerSlot()
        {
            string label = "[ Power Slot ]";

            getUI(UITag.BuyPowerSlotLabel).text = label;
            getUI(UITag.BuyPowerSlot1).text = "Adrenaline";
            getUI(UITag.BuyPowerSlot2).text = "Empty";
            getUI(UITag.BuyPowerSlot3).text = "Empty";
        }

        private void Start()
        {
            getUI = FindObjects.GetUIText;
        }
    }
}
