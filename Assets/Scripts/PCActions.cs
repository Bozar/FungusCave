using Fungus.Actor.AI;
using Fungus.Actor.FOV;
using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class PCActions : MonoBehaviour
    {
        private int[] autoMove;
        private bool checkEnergy;
        private bool checkSchedule;
        private Initialize init;
        private PlayerInput input;
        private SchedulingSystem schedule;
        private WizardMode wizard;

        private void Start()
        {
            input = GetComponent<PlayerInput>();
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            wizard = FindObjects.GameLogic.GetComponent<WizardMode>();
            init = FindObjects.GameLogic.GetComponent<Initialize>();
        }

        private void Update()
        {
            checkSchedule = schedule.IsCurrentActor(gameObject);
            checkEnergy = GetComponent<Energy>().HasEnoughEnergy();

            if (!checkSchedule || !init.Initialized)
            {
                return;
            }

            if (!checkEnergy)
            {
                schedule.NextActor();
                return;
            }

            if (GetComponent<FieldOfView>() != null)
            {
                GetComponent<FieldOfView>().UpdateFOV();
            }

            if (input.GameCommand() != Command.INVALID)
            {
                FindObjects.GameLogic.GetComponent<UIModeline>().PrintText();
            }

            if (GetComponent<AutoExplore>().ContinueAutoExplore)
            {
                autoMove = GetComponent<AutoExplore>().GetDestination();

                if (autoMove != null)
                {
                    GetComponent<Move>().MoveActor(autoMove);
                }
                return;
            }

            if (input.IsMovementCommand())
            {
                GetComponent<Move>().MoveActor(input.GameCommand());
                return;
            }

            switch (input.GameCommand())
            {
                case Command.AutoExplore:
                    GetComponent<AutoExplore>().Initialize();
                    GetComponent<AutoExplore>().GetDestination();
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
