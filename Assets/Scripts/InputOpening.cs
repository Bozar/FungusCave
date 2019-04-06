using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public class InputOpening : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return Command.Confirm;
            }
            return Command.INVALID;
        }
    }
}
