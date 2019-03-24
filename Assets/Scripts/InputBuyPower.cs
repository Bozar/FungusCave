using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public class InputBuyPower : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return Command.Cancel;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                return Command.Confirm;
            }
            return Command.INVALID;
        }
    }
}
