using Fungus.Actor.InputManager;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class InputSwitchSubMode : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return Command.Cancel;
            }
            else if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                return Command.Next;
            }
            else if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                return Command.Previous;
            }
            return Command.INVALID;
        }
    }
}
