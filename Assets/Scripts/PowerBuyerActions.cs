using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class PowerBuyerActions : MonoBehaviour
    {
        private SubGameMode mode;

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubGameMode>();
        }

        private void Update()
        {
            switch (GetComponent<PlayerInput>().GameCommand())
            {
                case Command.Cancel:
                    mode.SwitchModePowerBuyer(false);
                    break;
            }
        }
    }
}
