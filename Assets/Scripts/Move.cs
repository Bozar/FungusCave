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

    public bool IsWalkable(int x, int y)
    {
        isFloor = board.CheckBlock(SubObjectTag.Floor, x, y);
        isPool = board.CheckBlock(SubObjectTag.Pool, x, y);

        return isFloor || isPool;
    }

    public void MoveActor(Command direction)
    {
        NewPosition(direction);
        MoveActor(x, y);
    }

    public void MoveActor(int targetX, int targetY)
    {
        startPosition = coordinates.Convert(gameObject.transform.position);
        useDiagonalFactor = MoveDiagonally(targetX, targetY);

        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy())
        {
            return;
        }

        if (IsWait(targetX, targetY))
        {
            FindObjects.GameLogic.GetComponent<SchedulingSystem>().NextTurn();
            return;
        }

        if (actorBoard.HasActor(targetX, targetY))
        {
            gameObject.GetComponent<Attack>().DealDamage(targetX, targetY);
            return;
        }

        if (!IsWalkable(targetX, targetY))
        {
            FindObjects.GameLogic.GetComponent<UIModeline>().PrintText(
                "You are blocked");
            return;
        }

        actorBoard.RemoveActor(startPosition[0], startPosition[1]);
        actorBoard.AddActor(gameObject, targetX, targetY);

        if (gameObject.GetComponent<FieldOfView>() != null)
        {
            gameObject.GetComponent<FieldOfView>().UpdateFOV();
        }

        gameObject.transform.position = coordinates.Convert(targetX, targetY);
        gameObject.GetComponent<Energy>().CurrentEnergy -= GetEnergyCost();
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

        isPool = board.CheckBlock(SubObjectTag.Pool, startPosition);

        direction = useDiagonalFactor ? diagonalFactor : linearFactor;
        pool = isPool ? poolEnergy : 0;

        totalEnergy = (int)System.Math.Floor(baseEnergy * direction + pool);

        return totalEnergy;
    }

    private bool IsWait(int x, int y)
    {
        int[] pos = coordinates.Convert(gameObject.transform.position);

        return (pos[0] == x) && (pos[1] == y);
    }

    private bool MoveDiagonally(int x, int y)
    {
        bool checkX = System.Math.Abs(startPosition[0] - x) == 1;
        bool checkY = System.Math.Abs(startPosition[1] - y) == 1;

        return checkX && checkY;
    }

    private void NewPosition(Command direction)
    {
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
