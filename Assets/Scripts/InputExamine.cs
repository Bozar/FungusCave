using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public class InputExamine : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            bool next = Input.GetKeyDown(KeyCode.PageDown)
                || Input.GetKeyDown(KeyCode.O);

            bool previous = Input.GetKeyDown(KeyCode.PageUp)
                || Input.GetKeyDown(KeyCode.I);

            if (next)
            {
                return Command.Next;
            }
            else if (previous)
            {
                return Command.Previous;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                return Command.Cancel;
            }
            return Command.INVALID;
        }
    }
}
