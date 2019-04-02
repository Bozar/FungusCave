using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class ViewLogAction : MonoBehaviour
    {
        private HeaderAction header;
        private SubMode mode;

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubMode>();
            header = FindObjects.GameLogic.GetComponent<HeaderAction>();
        }

        private void Update()
        {
            Command cmd = GetComponent<PlayerInput>().GameCommand();
            switch (cmd)
            {
                case Command.Cancel:
                    mode.SwitchModeLog(false);
                    break;

                case Command.Next:
                case Command.Previous:
                    header.SwitchMode(cmd);
                    break;
            }
        }
    }
}
