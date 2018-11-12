using UnityEngine;

public class PCActions : MonoBehaviour
{
    private bool checkEnergy;
    private bool checkSchedule;
    private PlayerInput input;
    private SchedulingSystem schedule;
    private WizardMode wizard;

    private void Start()
    {
        input = gameObject.GetComponent<PlayerInput>();
        schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
        wizard = FindObjects.GameLogic.GetComponent<WizardMode>();
    }

    private void Update()
    {
        checkSchedule = schedule.IsCurrentActor(gameObject);
        checkEnergy = gameObject.GetComponent<Energy>().HasEnoughEnergy();

        if (!checkSchedule)
        {
            return;
        }

        if (!checkEnergy)
        {
            schedule.NextTurn();
            return;
        }

        if (gameObject.GetComponent<FieldOfView>() != null)
        {
            gameObject.GetComponent<FieldOfView>().UpdateFOV();
        }

        if (input.GameCommand() != Command.Invalid)
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
                    wizard.RenderAll();
                    return;

                case Command.PrintEnergy:
                    wizard.PrintEnergy();
                    return;

                case Command.AddEnergy:
                    wizard.AddEnergy();
                    return;

                    //case PlayerInput.Command.Confirm:
                    //    break;

                    //case PlayerInput.Command.Cancel:
                    //    break;

                    //case PlayerInput.Command.Invalid:
                    //    break;

                    //default:
                    //    FindObjects.GameLogic.GetComponent<TestMove>().
                    //        MoveAround(gameObject);
                    //    break;
            }
        }
    }
}
