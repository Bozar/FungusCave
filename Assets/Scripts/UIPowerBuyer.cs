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
        private PowerTag[] orderedPower;
        private Dictionary<PowerTag, UITag> powerTagUI;
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
            Power pcPower = FindObjects.PC.GetComponent<Power>();
            string label = "[ Power List ]";

            getUI(UITag.BuyPowerListLabel).text = label;

            PowerSlotTag slot;
            int potion;

            foreach (PowerTag tag in powerTagUI.Keys)
            {
                getUI(powerTagUI[tag]).text
                    = GetComponent<PowerData>().GetPowerName(tag);

                if (pcPower.HasPower(tag))
                {
                    getUI(powerTagUI[tag]).color
                        = GetComponent<GameColor>().PickColor(ColorName.Green);
                }
                else if (pcPower.IsBuyable(tag, out slot, out potion))
                {
                    getUI(powerTagUI[tag]).color
                        = GetComponent<GameColor>().PickColor(ColorName.White);
                }
                else
                {
                    getUI(powerTagUI[tag]).color
                        = GetComponent<GameColor>().PickColor(ColorName.Grey);
                }
            }

            // TODO: Change index dynamically.
            WrapPowerName(0);
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

            powerTagUI = new Dictionary<PowerTag, UITag>
            {
                { PowerTag.DefHP1, UITag.BuyPowerHP1 },
                { PowerTag.DefHP2, UITag.BuyPowerHP2 },
                { PowerTag.DefEnergy1, UITag.BuyPowerEnergy1 },
                { PowerTag.DefEnergy2, UITag.BuyPowerEnergy2 },
                { PowerTag.DefInfection1, UITag.BuyPowerInfection1 },
                { PowerTag.DefInfection2, UITag.BuyPowerInfection2 },
                { PowerTag.AttDamage1, UITag.BuyPowerAtkHP },
                { PowerTag.AttEnergy1, UITag.BuyPowerAtkEnergy },
                { PowerTag.AttInfection1, UITag.BuyPowerAtkInfection }
            };

            orderedPower = new PowerTag[]
            {
                PowerTag.DefHP1, PowerTag.DefHP2,
                PowerTag.DefEnergy1, PowerTag.DefEnergy2,
                PowerTag.DefInfection1, PowerTag.DefInfection2,
                PowerTag.AttDamage1,
                PowerTag.AttEnergy1,
                PowerTag.AttInfection1
            };
        }

        private void WrapPowerName(int index)
        {
            string bra = GetComponent<GameColor>().GetColorfulText(
                "< ", ColorName.White);
            string ket = GetComponent<GameColor>().GetColorfulText(
                " >", ColorName.White);
            string powerName = getUI(powerTagUI[orderedPower[index]]).text;

            getUI(powerTagUI[orderedPower[index]]).text = bra + powerName + ket;
        }
    }
}
