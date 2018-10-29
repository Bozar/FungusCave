using UnityEngine;

public class Move : MonoBehaviour
{
    private DungeonBoard board;
    private ConvertCoordinates coordinates;
    private bool isFloor;
    private bool isPool;
    private int x;
    private int y;

    public void MoveActor(PlayerInput.Command direction)
    {
        NewPosition(direction);

        // TODO: check NPC's position.
        if (!IsWalkable())
        {
            FindObjects.GameLogic.GetComponent<UIModeline>().PrintText(
                "You are blocked");
            return;
        }

        gameObject.transform.position = coordinates.Convert(x, y);

        if (gameObject.GetComponent<FieldOfView>() != null)
        {
            gameObject.GetComponent<FieldOfView>().UpdateFOV();
        }
    }

    private bool IsWalkable()
    {
        isFloor = board.CheckBlock(DungeonBoard.DungeonBlock.Floor, x, y);
        isPool = board.CheckBlock(DungeonBoard.DungeonBlock.Pool, x, y);

        return isFloor || isPool;
    }

    private void NewPosition(PlayerInput.Command direction)
    {
        x = coordinates.Convert(gameObject.transform.position.x);
        y = coordinates.Convert(gameObject.transform.position.y);

        switch (direction)
        {
            case PlayerInput.Command.Left:
                x -= 1;
                break;

            case PlayerInput.Command.Right:
                x += 1;
                break;

            case PlayerInput.Command.Up:
                y += 1;
                break;

            case PlayerInput.Command.Down:
                y -= 1;
                break;

            case PlayerInput.Command.UpLeft:
                x -= 1;
                y += 1;
                break;

            case PlayerInput.Command.UpRight:
                x += 1;
                y += 1;
                break;

            case PlayerInput.Command.DownLeft:
                x -= 1;
                y -= 1;
                break;

            case PlayerInput.Command.DownRight:
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
