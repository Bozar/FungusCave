using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class PowerBuyerActions : MonoBehaviour
    {
        private bool confirmToBuy;
        private SubGameMode mode;
        private Power pcPower;
        private UIPowerBuyer ui;

        private void Awake()
        {
            confirmToBuy = false;
        }

        private void ReadyToBuy(bool isReady)
        {
            confirmToBuy = isReady;

            FindObjects.GetUIText(UITag.BuyPowerModeline).text = isReady
                ? "Press Space again to confirm."
                : "";
        }

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubGameMode>();
            ui = FindObjects.GameLogic.GetComponent<UIPowerBuyer>();
            pcPower = FindObjects.PC.GetComponent<Power>();
        }

        private void TryBuyPower()
        {
            PowerTag power = ui.HighlightedPower;

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
                    mode.SwitchModePowerBuyer(false);
                    break;

                case Command.Down:
                case Command.Up:
                    ui.MoveBracket(cmd);
                    break;

                case Command.Confirm:
                    TryBuyPower();
                    reset = false;
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
