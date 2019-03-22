using Fungus.Actor;
using Fungus.GameSystem.ObjectManager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIPowerBuyer : MonoBehaviour, IUpdateUI
    {
        private UIText getUI;
        private Dictionary<PowerSlotTag, UITag> slotTagUI;

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
            string empty = "Empty";
            empty = GetComponent<GameColor>().GetColorfulText(
                empty, ColorName.Grey);

            getUI(UITag.BuyPowerSlotLabel).text = label;

            PowerTag power;
            bool isActive;

            foreach (PowerSlotTag tag in slotTagUI.Keys)
            {
                if (FindObjects.PC.GetComponent<Power>().HasPower(
                    tag, out power, out isActive))
                {
                    getUI(slotTagUI[tag]).text
                        = GetComponent<PowerData>().GetPowerName(power);
                }
                else
                {
                    getUI(slotTagUI[tag]).text = empty;
                }
            }
        }

        private void Start()
        {
            getUI = FindObjects.GetUIText;

            slotTagUI = new Dictionary<PowerSlotTag, UITag>
            {
                { PowerSlotTag.Slot1, UITag.BuyPowerSlot1 },
                { PowerSlotTag.Slot2, UITag.BuyPowerSlot2 },
                { PowerSlotTag.Slot3, UITag.BuyPowerSlot3 }
            };
        }
    }
}
