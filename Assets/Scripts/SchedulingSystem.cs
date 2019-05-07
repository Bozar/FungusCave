using Fungus.Actor;
using Fungus.Actor.Turn;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.Turn
{
    public class SchedulingSystem : MonoBehaviour, ISaveLoadBinary
    {
        private LinkedListNode<GameObject> currentNode;
        private LinkedListNode<GameObject> firstNode;
        private LinkedListNode<GameObject> nextNode;
        private LinkedList<GameObject> schedule;

        public DTActor[] ActorData { get; private set; }

        public int CountActor { get { return schedule.Count; } }

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

        public void LoadBinary(IDataTemplate[] dt)
        {
            foreach (IDataTemplate d in dt)
            {
                if (d.DTTag == DataTemplateTag.Schedule)
                {
                    DTSchedulingSystem value = d as DTSchedulingSystem;
                    ActorData = value.Actors;
                    return;
                }
            }
        }

        public void NextActor()
        {
            int minDistance = GetComponent<ActorData>().GetIntData(
                SubObjectTag.DEFAULT, DataTag.SightRange);

            EnableComponent(false);
            CurrentActor.GetComponent<InternalClock>().EndTurn();

            NextNode();

            CurrentActor.GetComponent<InternalClock>().StartTurn();
            // Do not call actor's Update() if he is far away from PC to speed up
            // the game.
            if (GetComponent<DungeonBoard>().GetDistance(
                CurrentActor, FindObjects.PC) > minDistance)
            {
                NextActor();
            }
            else
            {
                EnableComponent(true);
            }
        }

        public void PauseTurn(bool pause)
        {
            EnableComponent(!pause);
        }

        public void PrintSchedule()
        {
            int actorIndex = 1;
            int[] position;

            Debug.Log("==========");
            Debug.Log("Total actors: " + schedule.Count);
            Debug.Log("Current actor: "
                + CurrentActor.GetComponent<MetaInfo>().SubTag);

            foreach (var actor in schedule)
            {
                position = GetComponent<ConvertCoordinates>().Convert(
                    actor.transform.position);

                Debug.Log($"{actorIndex}: [{position[0]}, {position[1]}], "
                    + $"{actor.GetComponent<MetaInfo>().SubTag}, "
                    + $"HP: {actor.GetComponent<HP>().CurrentHP}, "
                    + $"Energy: {actor.GetComponent<Energy>().CurrentEnergy}");

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

        public void SaveBinary(Stack<IDataTemplate> dt)
        {
            Queue<DTActor> actors = new Queue<DTActor>();

            foreach (GameObject go in schedule)
            {
                actors.Enqueue(new DTActor
                {
                    ActorTag = go.GetComponent<MetaInfo>().SubTag,
                    Energy = go.GetComponent<Energy>().CurrentEnergy,
                    HP = go.GetComponent<HP>().CurrentHP,
                    Infection = go.GetComponent<Infection>().InfectionDict,
                    Position = GetComponent<ConvertCoordinates>().Convert(
                        go.transform.position),
                    Stress = go.GetComponent<Stress>().CurrentStress,

                    Potion = go.GetComponent<Potion>()?.CurrentPotion ?? -1,
                    Power = go.GetComponent<Power>()?.PowerDict
                });
            }

            var data = new DTSchedulingSystem { Actors = actors.ToArray() };
            dt.Push(data);
        }

        private void Awake()
        {
            schedule = new LinkedList<GameObject>();
        }

        private void CheckErrors(GameObject actor)
        {
            if (actor.GetComponent<MetaInfo>().MainTag
                != MainObjectTag.Actor)
            {
                throw new Exception("Invalid actor tag: "
                    + actor.GetComponent<MetaInfo>().MainTag);
            }
            else if (schedule.Contains(actor))
            {
                throw new Exception("Actor already exists in the schedule: "
                    + actor.GetComponent<MetaInfo>().SubTag);
            }
        }

        private void EnableComponent(bool enable)
        {
            if (CurrentActor.GetComponent<PCAction>() != null)
            {
                CurrentActor.GetComponent<PCAction>().enabled = enable;
            }
            else if (CurrentActor.GetComponent<NPCAction>() != null)
            {
                CurrentActor.GetComponent<NPCAction>().enabled = enable;
            }
        }

        private void NextNode()
        {
            firstNode = schedule.First;
            nextNode = currentNode.Next;

            UpdateCurrentNode();
        }

        private void SchedulingSystem_LoadingGame(object sender, LoadEventArgs e)
        {
            LoadBinary(e.GameData);
        }

        private void SchedulingSystem_SavingGame(object sender, SaveEventArgs e)
        {
            SaveBinary(e.GameData);
        }

        private void Start()
        {
            GetComponent<SaveLoadGame>().SavingGame
                += SchedulingSystem_SavingGame;
            GetComponent<SaveLoadGame>().LoadingGame
                += SchedulingSystem_LoadingGame;

            ActorData = null;
        }

        private void UpdateCurrentNode()
        {
            currentNode = nextNode ?? firstNode;
        }
    }
}
