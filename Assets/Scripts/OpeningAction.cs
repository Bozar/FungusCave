using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class OpeningAction : MonoBehaviour
    {
        private void Update()
        {
            switch (GetComponent<PlayerInput>().GameCommand())
            {
                case Command.Confirm:
                    FindObjects.GameLogic.GetComponent<SubMode>()
                        .SwitchModeOpening(false);
                    break;
            }
        }
    }
}
