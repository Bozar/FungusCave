using Fungus.Actor.AI;
using Fungus.Actor.FOV;
using Fungus.GameSystem;
using Fungus.Render;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class PCActions : MonoBehaviour
    {
        private bool checkEnergy;
        private bool checkSchedule;
        private Initialize init;
        private PlayerInput input;
        private SchedulingSystem schedule;
        private WizardMode wizard;

        private void Start()
        {
            input = gameObject.GetComponent<PlayerInput>();
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            wizard = FindObjects.GameLogic.GetComponent<WizardMode>();
            init = FindObjects.GameLogic.GetComponent<Initialize>();
        }

        private void Update()
        {
            checkSchedule = schedule.IsCurrentActor(gameObject);
            checkEnergy = gameObject.GetComponent<Energy>().HasEnoughEnergy();

            if (!checkSchedule || !init.Initialized)
            {
                return;
            }

            if (!checkEnergy)
            {
                schedule.NextActor();
                return;
            }

            if (gameObject.GetComponent<FieldOfView>() != null)
            {
                gameObject.GetComponent<FieldOfView>().UpdateFOV();
            }

            if (input.GameCommand() != Command.INVALID)
            {
                FindObjects.GameLogic.GetComponent<UIModeline>().PrintText();
            }

            if (gameObject.GetComponent<AutoExplore>().ContinueAutoExplore)
            {
                gameObject.GetComponent<AutoExplore>().AutoAction();
                return;
            }

            if (input.IsMovementCommand())
            {
                gameObject.GetComponent<Move>().MoveActor(input.GameCommand());
                return;
            }

            switch (input.GameCommand())
            {
                case Command.AutoExplore:
                    gameObject.GetComponent<AutoExplore>().Initialize();
                    gameObject.GetComponent<AutoExplore>().AutoAction();
                    return;
            }

            // Test commands.
            if (wizard.IsWizardMode)
            {
                switch (input.GameCommand())
                {
                    case Command.Initialize:
                        wizard.Initialize();
                        return;

                    case Command.RenderAll:
                        wizard.SwitchRenderAll();
                        return;

                    case Command.PrintEnergy:
                        wizard.PrintEnergy();
                        return;

                    case Command.AddEnergy:
                        wizard.AddEnergy();
                        return;

                    case Command.PrintSchedule:
                        wizard.PrintSchedule();
                        return;

                    case Command.GainHP:
                        wizard.GainHP();
                        return;

                    case Command.LoseHP:
                        wizard.LoseHP();
                        return;

                    case Command.DrinkPotion:
                        wizard.DrinkPotion();
                        return;

                    case Command.PrintEnergyCost:
                        wizard.SwitchPrintEnergyCost();
                        return;
                }
            }
        }
    }
}
