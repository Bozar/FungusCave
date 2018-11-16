using System;
using UnityEngine;

public class Move : MonoBehaviour
{
    private ActorBoard actorBoard;
    private int baseEnergy;
    private DungeonBoard board;
    private double cardinalFactor;
    private bool checkX;
    private bool checkY;
    private ConvertCoordinates coordinates;
    private double diagonalFactor;
    private bool isFloor;
    private bool isPool;
    private int poolEnergy;
    private int[] startPosition;
    private int[] targetPosition;
    private bool useDiagonalFactor;

    public bool IsPassable(int x, int y)
    {
        isFloor = board.CheckBlock(SubObjectTag.Floor, x, y);
        isPool = board.CheckBlock(SubObjectTag.Pool, x, y);

        return isFloor || isPool;
    }

    public void MoveActor(Command direction)
    {
        startPosition = coordinates.Convert(gameObject.transform.position);
        targetPosition = DirectionToPosition(direction,
            startPosition[0], startPosition[1]);

        MoveActor(targetPosition[0], targetPosition[1]);
    }

    public void MoveActor(int targetX, int targetY)
    {
        startPosition = coordinates.Convert(gameObject.transform.position);
        useDiagonalFactor = FindObjects.GameLogic.GetComponent<Direction>()
            .CheckDirection(RelativePosition.Diagonal,
            startPosition, targetX, targetY);

        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy())
        {
            return;
        }

        if (IsWait(targetX, targetY))
        {
            FindObjects.GameLogic.GetComponent<SchedulingSystem>().NextActor();
            return;
        }

        if (actorBoard.HasActor(targetX, targetY))
        {
            gameObject.GetComponent<Attack>().DealDamage(targetX, targetY);
            return;
        }

        if (!IsPassable(targetX, targetY))
        {
            FindObjects.GameLogic.GetComponent<UIModeline>().PrintText(
                "You are blocked");
            return;
        }

        actorBoard.RemoveActor(startPosition[0], startPosition[1]);
        actorBoard.AddActor(gameObject, targetX, targetY);

        gameObject.transform.position = coordinates.Convert(targetX, targetY);
        gameObject.GetComponent<Energy>().CurrentEnergy -= GetEnergyCost();
    }

    private void Awake()
    {
        baseEnergy = 1000;
        poolEnergy = 200;
        diagonalFactor = 1.4;
        cardinalFactor = 1.0;
    }

    private int[] DirectionToPosition(Command direction, int startX, int startY)
    {
        switch (direction)
        {
            case Command.Left:
                startX -= 1;
                break;

            case Command.Right:
                startX += 1;
                break;

            case Command.Up:
                startY += 1;
                break;

            case Command.Down:
                startY -= 1;
                break;

            case Command.UpLeft:
                startX -= 1;
                startY += 1;
                break;

            case Command.UpRight:
                startX += 1;
                startY += 1;
                break;

            case Command.DownLeft:
                startX -= 1;
                startY -= 1;
                break;

            case Command.DownRight:
                startX += 1;
                startY -= 1;
                break;
        }

        return new int[] { startX, startY };
    }

    private int GetEnergyCost()
    {
        int totalEnergy;
        int pool;
        double direction;

        isPool = board.CheckBlock(SubObjectTag.Pool, startPosition);

        direction = useDiagonalFactor ? diagonalFactor : cardinalFactor;
        pool = isPool ? poolEnergy : 0;

        totalEnergy = (int)Math.Floor(baseEnergy * direction + pool);

        return totalEnergy;
    }

    private bool IsWait(int x, int y)
    {
        checkX = startPosition[0] == x;
        checkY = startPosition[1] == y;

        return checkX && checkY;
    }

    private void Start()
    {
        coordinates = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
    }
}
