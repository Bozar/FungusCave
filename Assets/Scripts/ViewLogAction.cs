using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class ViewLogAction : MonoBehaviour
    {
        private SubMode mode;

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubMode>();
        }

        private void Update()
        {
            switch (GetComponent<PlayerInput>().GameCommand())
            {
                case Command.Cancel:
                    mode.SwitchModeLog(false);
                    break;

                case Command.Next:
                    Debug.Log("Next");
                    break;

                case Command.Previous:
                    Debug.Log("Previous");
                    break;
            }
        }
    }
}
