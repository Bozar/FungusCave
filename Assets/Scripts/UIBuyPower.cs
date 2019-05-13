using Fungus.Actor;
using Fungus.Actor.InputManager;
using Fungus.GameSystem.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIBuyPower : MonoBehaviour, IUpdateUI, IResetData
    {
        private int cursorPosition;
        private UIText getUI;
        private string node;
        private PowerTag[] orderedPower;
        private Dictionary<PowerTag, UITag> powerTagUI;
        private Dictionary<PowerSlotTag, UITag> slotTagUI;

        private delegate Text UIText(UITag tag);

        public PowerTag HighlightedPower
        {
            get { return orderedPower[cursorPosition]; }
        }

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

        public void Reset()
        {
            cursorPosition = 0;
        }

        private void Awake()
        {
            node = "BuyPower";
            cursorPosition = 0;
        }

        private string GetPCHP()
        {
            string hp = GetComponent<GameText>().GetStringData(node, "HP");
            hp = hp.Replace("%num%",
                FindObjects.PC.GetComponent<HP>().CurrentHP.ToString());

            return hp;
        }

        private string GetPowerCost(PowerTag tag)
        {
            // Cost: %num1%/%num2%
            string powerCost = GetComponent<GameText>().GetStringData(node,
                "Cost");
            int reqPotion = GetComponent<PowerData>().GetPowerCost(tag);
            int hasPotion = FindObjects.PC.GetComponent<Potion>().CurrentPotion;

            powerCost = powerCost.Replace("%num1%", reqPotion.ToString());
            powerCost = powerCost.Replace("%num2%", hasPotion.ToString());

            return powerCost;
        }

        private string GetPowerName(PowerTag tag)
        {
            string powerName = GetComponent<PowerData>().GetPowerName(tag);
            powerName = "[ " + powerName + " ]";

            return powerName;
        }

        private string GetPowerStatus(PowerTag tag)
        {
            // Status: %str%
            string powerStatus = GetComponent<GameText>().GetStringData(node,
                "Status");
            string unlocked = GetComponent<GameText>().GetStringData(node,
                "Unlocked");
            string locked = GetComponent<GameText>().GetStringData(node,
                "Locked");
            string owned = GetComponent<GameText>().GetStringData(node,
                "Owned");

            if (FindObjects.PC.GetComponent<Power>().HasPower(tag))
            {
                powerStatus = powerStatus.Replace("%str%", owned);
            }
            else if (FindObjects.PC.GetComponent<Power>().IsBuyable(tag))
            {
                powerStatus = powerStatus.Replace("%str%", unlocked);
            }
            else
            {
                powerStatus = powerStatus.Replace("%str%", locked);
            }
            return powerStatus;
        }

        private void PrintPowerDescription()
        {
            PowerTag tag = orderedPower[cursorPosition];

            getUI(UITag.BuyPowerName).text = GetPowerName(tag);
            getUI(UITag.BuyPowerStatus).text = GetPowerStatus(tag);
            getUI(UITag.BuyPowerCost).text = GetPowerCost(tag);
            getUI(UITag.BuyPowerPCHP).text = GetPCHP();
            getUI(UITag.BuyPowerDescription).text
                = GetComponent<GameText>().GetStringData(
                    "PowerDescription", tag);
        }

        private void PrintPowerList()
        {
            Power pcPower = FindObjects.PC.GetComponent<Power>();
            string label = "[ %str% ]";
            label = label.Replace("%str%",
                GetComponent<GameText>().GetStringData(node, "ListLabel"));

            getUI(UITag.BuyPowerListLabel).text = label;

            foreach (PowerTag tag in powerTagUI.Keys)
            {
                getUI(powerTagUI[tag]).text
                    = GetComponent<PowerData>().GetPowerName(tag);

                if (pcPower.HasPower(tag))
                {
                    getUI(powerTagUI[tag]).color
                        = GetComponent<GameColor>().PickColor(ColorName.Green);
                }
                else if (pcPower.IsBuyable(tag))
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

            WrapPowerName(cursorPosition);
        }

        private void PrintPowerSlot()
        {
            string label = "[ %str% ]";
            label = label.Replace("%str%",
                GetComponent<GameText>().GetStringData(node, "SlotLabel"));
            string empty = GetComponent<GameText>().GetStringData(node,
                "EmptySlot");
            empty = GetComponent<GameColor>().GetColorfulText(
                empty, ColorName.Grey);

            getUI(UITag.BuyPowerSlotLabel).text = label;

            foreach (PowerSlotTag tag in slotTagUI.Keys)
            {
                if (FindObjects.PC.GetComponent<Power>().HasPower(
                    tag, out PowerTag power, out bool isActive))
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
            GetComponent<UserInterface>().MovingCursor += UIBuyPower_MovingCursor;

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

        private void UIBuyPower_MovingCursor(object sender, MoveCursorEventArgs e)
        {
            if (e.Commander != CommanderTag.UIBuyPower)
            {
                return;
            }
            cursorPosition = GetComponent<UserInterface>().MoveCursorUpDown(
                e.Direction, cursorPosition, orderedPower.Length - 1);
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
