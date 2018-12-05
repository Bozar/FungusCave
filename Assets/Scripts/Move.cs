using Fungus.Actor.WorldBuilding;
using Fungus.GameSystem;
using Fungus.Render;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Move : MonoBehaviour
    {
        private ActorBoard actorBoard;
        private int baseEnergy;
        private DungeonBoard board;
        private ConvertCoordinates coordinates;
        private Direction direction;
        private int poolEnergy;
        private int[] startPosition;
        private int[] targetPosition;
        private bool useDiagonalFactor;

        public bool IsPassable(int x, int y)
        {
            bool isFloor;
            bool isPool;

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
            useDiagonalFactor = direction.CheckDirection(
                RelativePosition.Diagonal, startPosition, targetX, targetY);

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
                gameObject.GetComponent<Attack>().MeleeAttack(targetX, targetY);
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
            gameObject.GetComponent<Energy>().LoseEnergy(GetEnergyCost());
        }

        private void Awake()
        {
            baseEnergy = 1000;
            poolEnergy = 200;
        }

        private int[] DirectionToPosition(Command direction,
            int startX, int startY)
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
            int slow;
            int directionFactor;

            pool = board.CheckBlock(SubObjectTag.Pool, startPosition)
                ? poolEnergy : 0;

            directionFactor = useDiagonalFactor
                ? direction.DiagonalFactor
                : direction.CardinalFactor;

            slow = gameObject.GetComponent<Infection>()
                .HasInfection(InfectionTag.Slow)
                ? gameObject.GetComponent<Infection>().ModEnergy : 0;

            totalEnergy = (int)Math.Floor(
                (baseEnergy + pool) * ((100 + directionFactor + slow) * 0.01));

            if (FindObjects.GameLogic.GetComponent<WizardMode>().PrintEnergyCost
                && actorBoard.CheckActorTag(SubObjectTag.PC, gameObject))
            {
                FindObjects.GameLogic.GetComponent<UIMessage>()
                    .StoreText("Move energy: " + totalEnergy);
            }

            return totalEnergy;
        }

        private bool IsWait(int x, int y)
        {
            bool checkX = startPosition[0] == x;
            bool checkY = startPosition[1] == y;

            return checkX && checkY;
        }

        private void Start()
        {
            coordinates = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
            direction = FindObjects.GameLogic.GetComponent<Direction>();
        }
    }
}
