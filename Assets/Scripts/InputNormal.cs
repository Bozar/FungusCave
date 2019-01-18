using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public class InputNormal : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            bool upLeft = Input.GetKeyDown(KeyCode.Y)
                || Input.GetKeyDown(KeyCode.Keypad7);

            bool upRight = Input.GetKeyDown(KeyCode.U)
                || Input.GetKeyDown(KeyCode.Keypad9);

            bool downLeft = Input.GetKeyDown(KeyCode.B)
                || Input.GetKeyDown(KeyCode.Keypad1);

            bool downRight = Input.GetKeyDown(KeyCode.N)
                || Input.GetKeyDown(KeyCode.Keypad3);

            if (upLeft)
            {
                return Command.UpLeft;
            }
            else if (upRight)
            {
                return Command.UpRight;
            }
            else if (downLeft)
            {
                return Command.DownLeft;
            }
            else if (downRight)
            {
                return Command.DownRight;
            }
            else if (Input.GetKeyDown(KeyCode.Period))
            {
                return Command.Wait;
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                return Command.AutoExplore;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                return Command.Examine;
            }
            // Test key combinations.
            else if (Input.GetKey(KeyCode.LeftControl)
                && Input.GetKeyDown(KeyCode.F))
            {
                return Command.Up;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                return Command.Initialize;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                return Command.RenderAll;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                return Command.PrintEnergy;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                return Command.AddEnergy;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                return Command.PrintSchedule;
            }
            else if (Input.GetKeyDown(KeyCode.Equals))
            {
                return Command.GainHP;
            }
            else if (Input.GetKeyDown(KeyCode.Minus))
            {
                return Command.LoseHP;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                return Command.DrinkPotion;
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                return Command.PrintEnergyCost;
            }
            return Command.INVALID;
        }
    }
}
