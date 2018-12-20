using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
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
        private UIMessage message;
        private UIModeline modeline;
        private int poolEnergy;
        private SchedulingSystem schedule;
        private int[] startPosition;
        private int[] targetPosition;
        private DungeonTerrain terrain;
        private bool useDiagonalFactor;
        private WizardMode wizard;

        public void MoveActor(Command direction)
        {
            startPosition = coordinates.Convert(transform.position);
            targetPosition = coordinates.Convert(direction,
                startPosition[0], startPosition[1]);

            MoveActor(targetPosition[0], targetPosition[1]);
        }

        public void MoveActor(int[] position)
        {
            MoveActor(position[0], position[1]);
        }

        public void MoveActor(int targetX, int targetY)
        {
            startPosition = coordinates.Convert(transform.position);
            useDiagonalFactor = direction.CheckDirection(
                RelativePosition.Diagonal, startPosition, targetX, targetY);

            if (!GetComponent<Energy>().HasEnoughEnergy())
            {
                return;
            }

            if (IsWait(targetX, targetY))
            {
                schedule.NextActor();
                return;
            }

            if (actorBoard.HasActor(targetX, targetY))
            {
                GetComponent<Attack>().MeleeAttack(targetX, targetY);
                return;
            }

            if (!terrain.IsPassable(targetX, targetY))
            {
                modeline.PrintText("You are blocked");
                return;
            }

            actorBoard.RemoveActor(startPosition[0], startPosition[1]);
            actorBoard.AddActor(gameObject, targetX, targetY);

            transform.position = coordinates.Convert(targetX, targetY);
            GetComponent<Energy>().LoseEnergy(GetEnergyCost());
        }

        private void Awake()
        {
            poolEnergy = 200;
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

            slow = GetComponent<Infection>().HasInfection(InfectionTag.Slow)
                ? GetComponent<Infection>().ModEnergy : 0;

            totalEnergy = (int)Math.Floor(
                (baseEnergy + pool) * ((100 + directionFactor + slow) * 0.01));

            if (wizard.PrintEnergyCost
                && actorBoard.CheckActorTag(SubObjectTag.PC, gameObject))
            {
                message.StoreText("Move energy: " + totalEnergy);
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
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            wizard = FindObjects.GameLogic.GetComponent<WizardMode>();
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();

            baseEnergy = FindObjects.GameLogic.GetComponent<ObjectData>().
                GetIntData(GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.EnergyMove);
        }
    }
}
