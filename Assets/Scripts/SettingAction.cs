using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class SettingAction : MonoBehaviour
    {
        private HeaderAction header;
        private SubMode mode;
        private GameSetting setting;
        private UserInterface ui;

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubMode>();
            header = FindObjects.GameLogic.GetComponent<HeaderAction>();
            setting = FindObjects.GameLogic.GetComponent<GameSetting>();
            ui = FindObjects.GameLogic.GetComponent<UserInterface>();
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

                case Command.Down:
                case Command.Up:
                    ui.CommanderMoveCursor(new MoveCursorEventArgs
                    {
                        Commander = CommanderTag.UISetting,
                        Direction = cmd
                    });
                    break;

                case Command.Next:
                case Command.Previous:
                    header.SwitchMode(cmd);
                    break;
            }
        }
    }
}
