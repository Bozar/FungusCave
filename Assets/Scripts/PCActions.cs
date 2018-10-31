using UnityEngine;

public class PCActions : MonoBehaviour
{
    private bool checkEnergy;
    private bool checkSchedule;
    private PlayerInput input;
    private SchedulingSystem schedule;

    private void Start()
    {
        input = gameObject.GetComponent<PlayerInput>();
        schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
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

        if (input.IsMovementCommand())
        {
            gameObject.GetComponent<Move>().MoveActor(input.GameCommand());
        }
        else
        {
            switch (input.GameCommand())
            {
                // Test commands.
                case Command.Initialize:
                    FindObjects.GameLogic.GetComponent<Initialize>()
                        .InitializeGame();
                    break;

                case Command.RenderAll:
                    FindObjects.GameLogic.GetComponent<Test>().RenderAll
                        = !FindObjects.GameLogic.GetComponent<Test>().RenderAll;
                    break;

                case Command.PrintEnergy:
                    schedule.CurrentActor.GetComponent<Energy>().PrintEnergy();
                    break;

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
