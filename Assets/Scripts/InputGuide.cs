using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public class InputGuide : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            if (Input.GetKey(KeyCode.LeftControl)
                || Input.GetKey(KeyCode.RightControl))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    return Command.Reload;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return Command.Confirm;
            }
            return Command.INVALID;
        }
    }
}
