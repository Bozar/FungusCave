using System;
using System.Collections.Generic;
using UnityEngine;

public class SchedulingSystem : MonoBehaviour
{
    private LinkedList<GameObject> schedule = new LinkedList<GameObject>();

    private enum ValidTags
    {
        PC,
        Wall,
        NPC
    };

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
        return true;
    }

    public void PrintSchedule()
    {
        int actorIndex = 1;

        Debug.Log("==========");
        Debug.Log("Total actors: " + schedule.Count);

        foreach (var actor in schedule)
        {
            Debug.Log(actorIndex + ": " + actor.name);
            actorIndex++;
        }

        Debug.Log("==========");
    }

    public bool RemoveActor(GameObject actor)
    {
        return schedule.Remove(actor);
    }

    public void Test()
    {
        GameObject newPC = Instantiate(Resources.Load("PC") as GameObject);
        GameObject newWall = Instantiate(Resources.Load("Wall") as GameObject);
        GameObject newDummy = Instantiate(Resources.Load("Dummy") as GameObject);

        gameObject.GetComponent<SchedulingSystem>().AddActor(newPC);
        gameObject.GetComponent<SchedulingSystem>().AddActor(newWall);
        gameObject.GetComponent<SchedulingSystem>().AddActor(newDummy);
        gameObject.GetComponent<SchedulingSystem>().PrintSchedule();
    }
}
