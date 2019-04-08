using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class SettingAction : MonoBehaviour
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
                case Command.Confirm:
                    // TODO: Change this later.
                    Debug.Log("Cofirm");
                    FindObjects.GameLogic.GetComponent<GameSetting>().ShowOpening = true;
                    break;

                case Command.Cancel:
                    mode.SwitchModeSetting(false);
                    break;

                case Command.Next:
                case Command.Previous:
                    header.SwitchMode(cmd);
                    break;
            }
        }
    }
}
