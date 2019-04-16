using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class SettingAction : MonoBehaviour
    {
        private HeaderAction header;
        private SubMode mode;
        private GameSetting setting;

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubMode>();
            header = FindObjects.GameLogic.GetComponent<HeaderAction>();
            setting = FindObjects.GameLogic.GetComponent<GameSetting>();
        }

        private void SwitchSetting()
        {
            setting.ShowOpening = !setting.ShowOpening;
        }

        private void Update()
        {
            Command cmd = GetComponent<PlayerInput>().GameCommand();
            switch (cmd)
            {
                case Command.Confirm:
                    SwitchSetting();
                    GetComponent<SettingStatus>().IsModified = true;
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
