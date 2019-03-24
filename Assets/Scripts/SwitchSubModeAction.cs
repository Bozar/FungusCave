using Fungus.Actor.InputManager;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class SwitchSubModeAction : MonoBehaviour
    {
        private void Update()
        {
            switch (GetComponent<PlayerInput>().GameCommand())
            {
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
