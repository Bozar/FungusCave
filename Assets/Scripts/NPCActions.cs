﻿using Fungus.Actor.AI;
using Fungus.Actor.FOV;
using Fungus.GameSystem;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class NPCActions : MonoBehaviour
    {
        private bool checkEnergy;
        private bool checkSchedule;
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

            if (GetComponent<FieldOfView>() != null)
            {
                GetComponent<FieldOfView>().UpdateFOV();
            }

            switch (GetComponent<ActorAI>().DummyAI())
            {
                case Command.Wait:
                    GetComponent<Move>().MoveActor(Command.Wait);
                    return;

                case Command.Approach:
                    position = GetComponent<AutoExplore>().GetDestination();
                    GetComponent<Move>().MoveActor(position);
                    return;

                case Command.Attack:
                    position = GetComponent<NPCMemory>().PCPosition;
                    GetComponent<Attack>().MeleeAttack(position[0], position[1]);
                    return;
            }
        }
    }
}
