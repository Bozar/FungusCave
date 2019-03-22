using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class PowerBuyerActions : MonoBehaviour
    {
        private SubGameMode mode;
        private UIPowerBuyer ui;

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubGameMode>();
            ui = FindObjects.GameLogic.GetComponent<UIPowerBuyer>();
        }

        private void Update()
        {
            Command cmd = GetComponent<PlayerInput>().GameCommand();

            switch (cmd)
            {
                case Command.Cancel:
                    mode.SwitchModePowerBuyer(false);
                    break;

                case Command.Down:
                case Command.Up:
                    ui.MoveBracket(cmd);
                    break;
            }
        }
    }
}
