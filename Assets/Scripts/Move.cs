using UnityEngine;

public class Move : MonoBehaviour
{
    private int baseEnergy;
    private DungeonBoard board;
    private ConvertCoordinates coordinates;
    private bool isDiagonalMovement;
    private bool isFloor;
    private bool isPool;
    private double moveDiagonally;
    private double moveHorizonally;
    private int x;
    private int y;

    public void MoveActor(Command direction)
    {
        NewPosition(direction);
        isDiagonalMovement = MoveDiagonally(direction);

        if (direction == Command.Wait)
        {
            FindObjects.GameLogic.GetComponent<SchedulingSystem>().NextTurn();
            return;
        }

        // TODO: check NPC's position.
        if (!IsWalkable())
        {
            FindObjects.GameLogic.GetComponent<UIModeline>().PrintText(
                "You are blocked");
            return;
        }

        if (gameObject.GetComponent<Energy>().HasEnoughEnergy())
        {
            gameObject.GetComponent<Energy>().CurrentEnergy -= GetEnergyCost();
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

    private void Awake()
    {
        baseEnergy = 1000;
        moveDiagonally = 1.4;
        moveHorizonally = 1.0;
    }

    private int GetEnergyCost()
    {
        int totalEnergy;
        double direction;

        if (isDiagonalMovement)
        {
            direction = moveDiagonally;
        }
        else
        {
            direction = moveHorizonally;
        }

        totalEnergy = (int)System.Math.Floor(baseEnergy * direction);

        return totalEnergy;
    }

    private bool IsWalkable()
    {
        isFloor = board.CheckBlock(DungeonBlock.Floor, x, y);
        isPool = board.CheckBlock(DungeonBlock.Pool, x, y);

        return isFloor || isPool;
    }

    private bool MoveDiagonally(Command direction)
    {
        switch (direction)
        {
            case Command.UpLeft:
            case Command.UpRight:
            case Command.DownLeft:
            case Command.DownRight:
                return true;
        }
        return false;
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
