using Fungus.Actor.AI;
using Fungus.Actor.FOV;
using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class NPCAction : MonoBehaviour
    {
        private bool checkEnergy;
        private bool checkSchedule;
        private ConvertCoordinates coord;
        private Initialize init;
        private int[] position;
        private SchedulingSystem schedule;

        private void Awake()
        {
            position = new int[2];
        }

        private void Start()
        {
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            init = FindObjects.GameLogic.GetComponent<Initialize>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        }

        private void Update()
        {
            checkSchedule = schedule.IsCurrentActor(gameObject);
            checkEnergy = GetComponent<Energy>().HasEnoughEnergy();

            //position = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            //    .Convert(gameObject.transform.position);

            if (!checkSchedule || !init.Initialized)
            {
                return;
            }

            if (!checkEnergy)
            {
                schedule.NextActor();
                return;
            }

            GetComponent<FieldOfView>()?.UpdateFOV();

            switch (GetComponent<ActorAI>().DummyAI())
            {
                case Command.Wait:
                    schedule.NextActor();
                    return;

                case Command.Approach:
                    position = GetComponent<AutoExplore>().GetDestination();
                    if (position == null)
                    {
                        schedule.NextActor();
                    }
                    else
                    {
                        GetComponent<IMove>().MoveGameObject(position);
                    }
                    return;

                case Command.Attack:
                    //position = GetComponent<NPCMemory>().PCPosition;
                    position = coord.Convert(FindObjects.PC.transform.position);
                    GetComponent<Attack>().MeleeAttack(position[0], position[1]);
                    return;
            }
        }
    }
}
