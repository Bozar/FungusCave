using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IMove
    {
        void MoveGameObject(int targetX, int targetY);

        void MoveGameObject(int[] position);
    }

    public class MoveActor : MonoBehaviour, IMove
    {
        private ActorBoard actor;
        private int baseEnergy;
        private DungeonBoard board;
        private ConvertCoordinates coord;
        private Direction direction;
        private UIMessage message;
        private UIModeline modeline;
        private int poolEnergy;
        private DungeonTerrain terrain;
        private bool useDiagonalFactor;
        private WizardMode wizard;

        public void MoveGameObject(int[] position)
        {
            MoveGameObject(position[0], position[1]);
        }

        public void MoveGameObject(int targetX, int targetY)
        {
            int[] start = coord.Convert(transform.position);
            useDiagonalFactor = direction.CheckDirection(
                RelativePosition.Diagonal, start, targetX, targetY);

            if (!GetComponent<Energy>().HasEnoughEnergy())
            {
                return;
            }

            if (!terrain.IsPassable(targetX, targetY))
            {
                modeline.PrintStaticText("You are blocked");
                return;
            }

            terrain.ChangeStatus(true, start[0], start[1]);
            terrain.ChangeStatus(false, targetX, targetY);

            actor.RemoveActor(start[0], start[1]);
            actor.AddActor(gameObject, targetX, targetY);

            transform.position = coord.Convert(targetX, targetY);
            GetComponent<Energy>().LoseEnergy(GetEnergyCost(start));
        }

        private void Awake()
        {
            poolEnergy = 200;
        }

        private int GetEnergyCost(int[] startPoint)
        {
            int totalEnergy;
            int pool;
            int slow;
            int directionFactor;

            pool = board.CheckBlock(SubObjectTag.Pool, startPoint)
                ? poolEnergy : 0;

            directionFactor = useDiagonalFactor
                ? direction.DiagonalFactor : direction.CardinalFactor;

            slow = GetComponent<Infection>().HasInfection(InfectionTag.Slow)
                ? GetComponent<Infection>().ModEnergy : 0;

            totalEnergy = (int)Math.Floor(
                (baseEnergy + pool) * ((100 + directionFactor + slow) * 0.01));

            if (wizard.PrintEnergyCost && GetComponent<ObjectMetaInfo>().IsPC)
            {
                message.StoreText("Move energy: " + totalEnergy);
            }

            return totalEnergy;
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
            direction = FindObjects.GameLogic.GetComponent<Direction>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            wizard = FindObjects.GameLogic.GetComponent<WizardMode>();
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();

            baseEnergy = FindObjects.GameLogic.GetComponent<EnergyData>().Move;
        }
    }
}
