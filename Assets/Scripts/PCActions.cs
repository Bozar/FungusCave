using Fungus.Actor.AI;
using Fungus.Actor.FOV;
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
        private Initialize init;
        private PlayerInput input;
        private SchedulingSystem schedule;
        private WizardMode wizard;

        private void MoveOrAttack(Command direction)
        {
            int[] target = coord.Convert(
                direction, FindObjects.PC.transform.position);

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
            input = GetComponent<PlayerInput>();
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            wizard = FindObjects.GameLogic.GetComponent<WizardMode>();
            init = FindObjects.GameLogic.GetComponent<Initialize>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
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
                FindObjects.GameLogic.GetComponent<UIModeline>().PrintText();
            }

            if (GetComponent<AutoExplore>().ContinueAutoExplore)
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
                    GetComponent<AutoExplore>().Initialize();
                    GetComponent<AutoExplore>().GetDestination();
                    return;

                case Command.Examine:
                    FindObjects.GameLogic.GetComponent<SubGameMode>()
                        .SwitchModeExamine(true);
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
