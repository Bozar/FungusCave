using UnityEngine;

public class Move : MonoBehaviour
{
    private ActorBoard actorBoard;
    private int baseEnergy;
    private DungeonBoard board;
    private ConvertCoordinates coordinates;
    private double diagonalFactor;
    private bool isFloor;
    private bool isPool;
    private double linearFactor;
    private int poolEnergy;
    private int[] startPosition;
    private bool useDiagonalFactor;
    private int x;
    private int y;

    public void MoveActor(Command direction)
    {
        NewPosition(direction);
        useDiagonalFactor = MoveDiagonally(direction);

        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy())
        {
            return;
        }

        if (direction == Command.Wait)
        {
            FindObjects.GameLogic.GetComponent<SchedulingSystem>().NextTurn();
            return;
        }

        if (actorBoard.HasActor(x, y))
        {
            gameObject.GetComponent<Attack>().DealDamage(x, y);
            return;
        }

        if (!IsWalkable())
        {
            FindObjects.GameLogic.GetComponent<UIModeline>().PrintText(
                "You are blocked");
            return;
        }

        gameObject.GetComponent<Energy>().CurrentEnergy -= GetEnergyCost();
        gameObject.transform.position = coordinates.Convert(x, y);

        actorBoard.RemoveActor(startPosition[0], startPosition[1]);
        actorBoard.AddActor(gameObject, x, y);

        if (gameObject.GetComponent<FieldOfView>() != null)
        {
            gameObject.GetComponent<FieldOfView>().UpdateFOV();
        }
    }

    private void Awake()
    {
        baseEnergy = 1000;
        poolEnergy = 200;
        diagonalFactor = 1.4;
        linearFactor = 1.0;
    }

    private int GetEnergyCost()
    {
        int totalEnergy;
        int pool;
        double direction;

        isPool = board.CheckBlock(DungeonBlock.Pool, startPosition);

        direction = useDiagonalFactor ? diagonalFactor : linearFactor;
        pool = isPool ? poolEnergy : 0;

        totalEnergy = (int)System.Math.Floor(baseEnergy * direction + pool);

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
        startPosition = new int[] { x, y };

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
        actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
    }
}
