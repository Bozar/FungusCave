using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public class InputPowerBuyer : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return Command.Cancel;
            }
            return Command.INVALID;
        }
    }
}
