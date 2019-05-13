using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class BuyPowerAction : MonoBehaviour
    {
        private UIBuyPower buyPower;
        private bool confirmToBuy;
        private HeaderAction header;
        private SubMode mode;
        private Power pcPower;
        private GameText text;
        private UserInterface ui;

        private void Awake()
        {
            confirmToBuy = false;
        }

        private void ReadyToBuy(bool isReady)
        {
            confirmToBuy = isReady;

            FindObjects.GetUIText(UITag.BuyPowerModeline).text = isReady
                ? text.GetStringData("BuyPower", "Confirm")
                : "";
        }

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubMode>();
            buyPower = FindObjects.GameLogic.GetComponent<UIBuyPower>();
            ui = FindObjects.GameLogic.GetComponent<UserInterface>();
            pcPower = FindObjects.PC.GetComponent<Power>();
            header = FindObjects.GameLogic.GetComponent<HeaderAction>();
            text = FindObjects.GameLogic.GetComponent<GameText>();
        }

        private void TryBuyPower()
        {
            PowerTag power = buyPower.HighlightedPower;

            if (pcPower.IsBuyable(power) && !pcPower.HasPower(power))
            {
                if (confirmToBuy)
                {
                    pcPower.BuyPower(power);
                    ReadyToBuy(false);
                }
                else
                {
                    ReadyToBuy(true);
                }
            }
        }

        private void Update()
        {
            Command cmd = GetComponent<PlayerInput>().GameCommand();
            bool reset = true;

            switch (cmd)
            {
                case Command.Cancel:
                    mode.SwitchModeBuyPower(false);
                    break;

                case Command.Down:
                case Command.Up:
                    ui.CommanderMoveCursor(new MoveCursorEventArgs
                    {
                        Commander = CommanderTag.UIBuyPower,
                        Direction = cmd
                    });
                    break;

                case Command.Confirm:
                    TryBuyPower();
                    reset = false;
                    break;

                case Command.Next:
                case Command.Previous:
                    header.SwitchMode(cmd);
                    break;

                default:
                    reset = false;
                    break;
            }

            if (reset)
            {
                ReadyToBuy(false);
            }
        }
    }
}
