using UnityEngine;

public class NPCActions : MonoBehaviour
{
    private bool checkEnergy;
    private bool checkSchedule;
    private int[] position;
    private SchedulingSystem schedule;

    private void Start()
    {
        schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
    }

    private void Update()
    {
        checkSchedule = schedule.IsCurrentActor(gameObject);
        checkEnergy = gameObject.GetComponent<Energy>().HasEnoughEnergy();

        position = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(gameObject.transform.position);

        if (!checkSchedule)
        {
            return;
        }

        if (!checkEnergy)
        {
            schedule.NextTurn();
            return;
        }

        switch (gameObject.GetComponent<NPCAI>().DummyAI())
        {
            case Command.Wait:
                gameObject.GetComponent<Move>().MoveActor(Command.Wait);
                FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
                    "Dummy (" + position[0] + "," + position[1] + ") waits.");
                break;
        }
    }
}
