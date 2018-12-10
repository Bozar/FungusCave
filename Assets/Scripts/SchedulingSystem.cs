using Fungus.Actor.ObjectManager;
using Fungus.Actor.Turn;
using Fungus.GameSystem.ObjectManager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.Turn
{
    public class SchedulingSystem : MonoBehaviour
    {
        private LinkedListNode<GameObject> currentNode;
        private LinkedListNode<GameObject> firstNode;
        private LinkedListNode<GameObject> nextNode;
        private LinkedList<GameObject> schedule;

        public GameObject CurrentActor
        {
            get { return currentNode.Value; }
        }

        public void AddActor(GameObject actor)
        {
            CheckErrors(actor);
            schedule.AddLast(actor);

            if (schedule.Count == 1)
            {
                currentNode = schedule.First;
            }
        }

        public bool IsCurrentActor(GameObject actor)
        {
            return actor == CurrentActor;
        }

        public void NextActor()
        {
            EnableComponent(false);
            CurrentActor.GetComponent<InternalClock>().EndTurn();

            NextNode();

            CurrentActor.GetComponent<InternalClock>().StartTurn();
            EnableComponent(true);
        }

        public void PrintSchedule()
        {
            int actorIndex = 1;
            int[] position;

            Debug.Log("==========");
            Debug.Log("Total actors: " + schedule.Count);
            Debug.Log("Current actor: "
                + CurrentActor.GetComponent<ObjectMetaInfo>().SubTag);

            foreach (var actor in schedule)
            {
                position = GetComponent<ConvertCoordinates>().Convert(
                    actor.transform.position);

                Debug.Log(actorIndex
                    + ": [" + position[0] + "," + position[1] + "] "
                    + actor.GetComponent<ObjectMetaInfo>().SubTag);

                actorIndex++;
            }
        }

        public bool RemoveActor(GameObject actor)
        {
            bool actorIsRemoved;
            bool currentNodeIsRemoved;

            firstNode = schedule.First;
            nextNode = currentNode.Next;

            currentNodeIsRemoved = currentNode.Value == actor;
            actorIsRemoved = schedule.Remove(actor);

            if (actorIsRemoved && currentNodeIsRemoved)
            {
                UpdateCurrentNode();
            }

            return actorIsRemoved;
        }

        private void Awake()
        {
            schedule = new LinkedList<GameObject>();
        }

        private void CheckErrors(GameObject actor)
        {
            if (actor.GetComponent<ObjectMetaInfo>().MainTag
                != MainObjectTag.Actor)
            {
                throw new Exception("Invalid actor tag: "
                    + actor.GetComponent<ObjectMetaInfo>().MainTag);
            }
            else if (schedule.Contains(actor))
            {
                throw new Exception("Actor already exists in the schedule: "
                    + actor.GetComponent<ObjectMetaInfo>().SubTag);
            }
        }

        private void EnableComponent(bool enable)
        {
            // TODO: Update after Unity 2018.3.
            if (CurrentActor.GetComponent<PCActions>() != null)
            {
                CurrentActor.GetComponent<PCActions>().enabled = enable;
            }
            else if (CurrentActor.GetComponent<NPCActions>() != null)
            {
                CurrentActor.GetComponent<NPCActions>().enabled = enable;
            }
        }

        private void NextNode()
        {
            firstNode = schedule.First;
            nextNode = currentNode.Next;

            UpdateCurrentNode();
        }

        private void UpdateCurrentNode()
        {
            currentNode = nextNode ?? firstNode;
        }
    }
}
