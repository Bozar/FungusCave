using System;
using System.Collections.Generic;
using UnityEngine;

public class SchedulingSystem : MonoBehaviour
{
    private LinkedListNode<GameObject> currentNode;
    private LinkedListNode<GameObject> firstNode;
    private LinkedListNode<GameObject> nextNode;
    private LinkedList<GameObject> schedule = new LinkedList<GameObject>();

    private enum ValidTags { PC, NPC };

    public GameObject CurrentActor
    {
        get { return currentNode.Value; }
    }

    public bool AddActor(GameObject actor)
    {
        if (!Enum.IsDefined(typeof(ValidTags), actor.tag))
        {
            Debug.Log("Invalid actor tag: " + actor.tag);
            return false;
        }
        else if (schedule.Contains(actor))
        {
            Debug.Log("Actor already exists in the schedule: " + actor.name);
            return false;
        }

        schedule.AddLast(actor);

        if (schedule.Count == 1)
        {
            currentNode = schedule.First;
        }

        return true;
    }

    public void GotoNextActor()
    {
        firstNode = schedule.First;
        nextNode = currentNode.Next;

        GotoNextNode();
    }

    public bool IsCurrentActor(GameObject actor)
    {
        bool verify = actor == CurrentActor;

        return verify;
    }

    public void PrintSchedule()
    {
        int actorIndex = 1;

        Debug.Log("==========");
        Debug.Log("Total actors: " + schedule.Count);
        Debug.Log("Current actor: " + CurrentActor.name);

        foreach (var actor in schedule)
        {
            Debug.Log(actorIndex + ": " + actor.name);
            actorIndex++;
        }

        Debug.Log("==========");
    }

    public bool RemoveActor(GameObject actor)
    {
        bool removed;
        bool currentNodeIsRemoved;

        firstNode = schedule.First;
        nextNode = currentNode.Next;

        currentNodeIsRemoved = currentNode.Value == actor;
        removed = schedule.Remove(actor);

        if (removed && currentNodeIsRemoved)
        {
            GotoNextNode();
        }

        return removed;
    }

    private void GotoNextNode()
    {
        if (nextNode != null)
        {
            currentNode = nextNode;
        }
        else
        {
            currentNode = firstNode;
        }
    }
}
