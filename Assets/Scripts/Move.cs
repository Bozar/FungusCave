using UnityEngine;

public class Move : MonoBehaviour
{
    private DungeonBoard board;
    private ConvertCoordinates coordinates;
    private bool isFloor;
    private bool isPool;
    private int x;
    private int y;

    public void MoveActor(Command direction)
    {
        NewPosition(direction);

        // TODO: check NPC's position.
        if (!IsWalkable())
        {
            FindObjects.GameLogic.GetComponent<UIModeline>().PrintText(
                "You are blocked");
            return;
        }

        if (gameObject.GetComponent<Energy>().ConsumeEnergy(
            ConsumeType.Move, true))
        {
            gameObject.transform.position = coordinates.Convert(x, y);
        }
        else
        {
            FindObjects.GameLogic.GetComponent<UIModeline>().PrintText(
               "You are exhausted.");
        }

        if (gameObject.GetComponent<FieldOfView>() != null)
        {
            gameObject.GetComponent<FieldOfView>().UpdateFOV();
        }
    }

    private bool IsWalkable()
    {
        isFloor = board.CheckBlock(DungeonBlock.Floor, x, y);
        isPool = board.CheckBlock(DungeonBlock.Pool, x, y);

        return isFloor || isPool;
    }

    private void NewPosition(Command direction)
    {
        x = coordinates.Convert(gameObject.transform.position.x);
        y = coordinates.Convert(gameObject.transform.position.y);

        switch (direction)
        {
            case Command.Left:
                x -= 1;
                break;

            case Command.Right:
                x += 1;
                break;

            case Command.Up:
                y += 1;
                break;

            case Command.Down:
                y -= 1;
                break;

            case Command.UpLeft:
                x -= 1;
                y += 1;
                break;

            case Command.UpRight:
                x += 1;
                y += 1;
                break;

            case Command.DownLeft:
                x -= 1;
                y -= 1;
                break;

            case Command.DownRight:
                x += 1;
                y -= 1;
                break;
        }
    }

    private void Start()
    {
        coordinates = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
    }
}
