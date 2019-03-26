using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class ViewMessageAction : MonoBehaviour
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
                    mode.SwitchModeBuyPower(false);
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
