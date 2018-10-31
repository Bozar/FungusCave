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
                //case PlayerInput.Command.EndTurn:
                //    break;

                //case PlayerInput.Command.Initialize:
                //    break;

                //case PlayerInput.Command.RenderAll:
                //    break;

                //case PlayerInput.Command.PrintEnergy:
                //    break;

                //case PlayerInput.Command.Confirm:
                //    break;

                //case PlayerInput.Command.Cancel:
                //    break;

                //case PlayerInput.Command.Invalid:
                //    break;

                default:
                    FindObjects.GameLogic.GetComponent<TestMove>().
                        MoveAround(gameObject);
                    break;
            }
        }
    }
}
