using Fungus.Actor;
using Fungus.Actor.AI;
using Fungus.GameSystem;
using UnityEngine;

public class NPCActions : MonoBehaviour
{
    private bool checkEnergy;
    private bool checkSchedule;
    private Initialize init;
    private SchedulingSystem schedule;
    //private int[] position;

    private void Start()
    {
        schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
        init = FindObjects.GameLogic.GetComponent<Initialize>();
    }

    private void Update()
    {
        checkSchedule = schedule.IsCurrentActor(gameObject);
        checkEnergy = gameObject.GetComponent<Energy>().HasEnoughEnergy();

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

        if (gameObject.GetComponent<FieldOfView>() != null)
        {
            gameObject.GetComponent<FieldOfView>().UpdateFOV();
        }

        switch (gameObject.GetComponent<ActorAI>().DummyAI())
        {
            case Command.Wait:
                gameObject.GetComponent<Move>().MoveActor(Command.Wait);
                //FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
                //    "Dummy (" + position[0] + "," + position[1] + ") waits.");
                break;
        }
    }
}
