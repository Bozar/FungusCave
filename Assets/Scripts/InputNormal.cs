using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public class InputNormal : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            bool help = Input.GetKeyDown(KeyCode.Slash)
                || (Input.GetKeyDown(KeyCode.LeftShift)
                && Input.GetKeyDown(KeyCode.Question));

            if (Input.GetKeyDown(KeyCode.Period))
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
            else if (Input.GetKeyDown(KeyCode.C))
            {
                return Command.BuyPower;
            }
            else if (help)
            {
                return Command.Help;
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
