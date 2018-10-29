using UnityEngine;

public class PCActions : MonoBehaviour
{
    private PlayerInput input;

    private void Start()
    {
        input = gameObject.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (!FindObjects.GameLogic.GetComponent<SchedulingSystem>()
            .IsCurrentActor(gameObject))
        {
            return;
        }

        switch (input.GameCommand())
        {
            case PlayerInput.Command.Left:
            case PlayerInput.Command.Right:
            case PlayerInput.Command.Up:
            case PlayerInput.Command.Down:
            case PlayerInput.Command.UpLeft:
            case PlayerInput.Command.UpRight:
            case PlayerInput.Command.DownLeft:
            case PlayerInput.Command.DownRight:
            case PlayerInput.Command.Wait:
                gameObject.GetComponent<Move>().MoveActor(input.GameCommand());
                break;

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
