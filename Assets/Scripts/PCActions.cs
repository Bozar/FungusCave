using Fungus.Actor.AI;
using Fungus.Actor.FOV;
using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class PCActions : MonoBehaviour
    {
        private ActorBoard actor;
        private bool checkEnergy;
        private bool checkSchedule;
        private ConvertCoordinates coord;
        private SubGameMode gameMode;
        private Initialize init;
        private PlayerInput input;
        private UIModeline modeline;
        private SchedulingSystem schedule;
        private WizardMode wizard;

        private void MoveOrAttack(Command direction)
        {
            int[] target = coord.Convert(direction, transform.position);

            if (actor.HasActor(target))
            {
                GetComponent<Attack>().MeleeAttack(target[0], target[1]);
                return;
            }

            GetComponent<IMove>().MoveGameObject(target[0], target[1]);
            return;
        }

        private void Start()
        {
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            wizard = FindObjects.GameLogic.GetComponent<WizardMode>();
            init = FindObjects.GameLogic.GetComponent<Initialize>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
            gameMode = FindObjects.GameLogic.GetComponent<SubGameMode>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();

            input = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            checkSchedule = schedule.IsCurrentActor(gameObject);
            checkEnergy = GetComponent<Energy>().HasEnoughEnergy();

            if (!checkSchedule || !init.Initialized)
            {
                return;
            }

            if (GetComponent<FieldOfView>() != null)
            {
                GetComponent<FieldOfView>().UpdateFOV();
            }

            if (!checkEnergy)
            {
                schedule.NextActor();
                return;
            }

            if (input.GameCommand() != Command.INVALID)
            {
                modeline.PrintStaticText();
            }

            if (GetComponent<PCAutoExplore>().ContinueAutoExplore)
            {
                int[] target = GetComponent<AutoExplore>().GetDestination();

                if (target != null)
                {
                    GetComponent<IMove>().MoveGameObject(target);
                }
                return;
            }

            switch (input.GameCommand())
            {
                case Command.Left:
                case Command.Right:
                case Command.Up:
                case Command.Down:
                case Command.UpLeft:
                case Command.UpRight:
                case Command.DownLeft:
                case Command.DownRight:
                    MoveOrAttack(input.GameCommand());
                    return;

                case Command.Wait:
                    schedule.NextActor();
                    return;

                case Command.AutoExplore:
                    GetComponent<PCAutoExplore>().Trigger();
                    return;

                case Command.Examine:
                    gameMode.SwitchModeExamine(true);
                    return;

                case Command.Help:
                    modeline.PrintStaticText("Help is unavailable for now.");
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
