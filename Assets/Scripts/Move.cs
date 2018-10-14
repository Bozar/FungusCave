using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    private static bool Instance;

    private int countStep;

    private GameObject[] mainUI;
    private Text message;
    private Vector3 moveHere;

    private UserInput.Command newDirection;

    private WaitForSeconds wait5Seconds;

    public void MoveAround(Transform actor)
    {
        newDirection = FindObjects.GameLogic.GetComponent<UserInput>().
            OutputCommand();

        if (!IsWalkable(newDirection, actor))
        {
            Debug.Log("You are blocked.");
            message.text = "You are blocked";
            return;
        }

        switch (newDirection)
        {
            case UserInput.Command.Left:
                moveHere
                    = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                    .Convert(-1, 0);
                break;

            case UserInput.Command.Right:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(1, 0);
                break;

            case UserInput.Command.Down:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(0, -1);
                break;

            case UserInput.Command.Up:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(0, 1);
                break;

            case UserInput.Command.UpLeft:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(-1, 1);
                break;

            case UserInput.Command.UpRight:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(1, 1);
                break;

            case UserInput.Command.DownLeft:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(-1, -1);
                break;

            case UserInput.Command.DownRight:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(1, -1);
                break;
        }

        if (newDirection == UserInput.Command.EndTurn)
        {
            FindObjects.GameLogic.GetComponent<SchedulingSystem>().CurrentActor
                .GetComponent<PCMove>().enabled = false;
            FindObjects.GameLogic.GetComponent<SchedulingSystem>()
                .GotoNextActor();
            FindObjects.GameLogic.GetComponent<SchedulingSystem>().CurrentActor
                .GetComponent<PCMove>().enabled = true;
        }
        else if (newDirection == UserInput.Command.Initialize)
        {
            FindObjects.GameLogic.GetComponent<Initialize>().InitializeGame();
        }
        else if (newDirection != UserInput.Command.Invalid)
        //if (!string.IsNullOrEmpty(newDirection))
        {
            message.text =
                FindObjects.GameLogic.GetComponent<RandomNumber>().Seed +
                "\n2\n3\n4\n5\n6";

            FindObjects.GameLogic.GetComponent<TileOverlay>()
                .CoverTile(false, actor.position);

            actor.position += moveHere;
            countStep++;

            FindObjects.GameLogic.GetComponent<TileOverlay>()
                .CoverTile(actor.position);
        }
    }

    private bool IsWalkable(UserInput.Command direction, Transform actor)
    {
        int x = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(actor.position.x);
        int y = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(actor.position.y);

        switch (direction)
        {
            case UserInput.Command.Left:
                x -= 1;
                break;

            case UserInput.Command.Right:
                x += 1;
                break;

            case UserInput.Command.Up:
                y += 1;
                break;

            case UserInput.Command.Down:
                y -= 1;
                break;

            case UserInput.Command.UpLeft:
                x -= 1;
                y += 1;
                break;

            case UserInput.Command.UpRight:
                x += 1;
                y += 1;
                break;

            case UserInput.Command.DownLeft:
                x -= 1;
                y -= 1;
                break;

            case UserInput.Command.DownRight:
                x += 1;
                y -= 1;
                break;
        }

        return FindObjects.GameLogic.GetComponent<BuildDungeon>()
            .CheckTerrain(BuildDungeon.DungeonBlock.Floor, x, y)
            || FindObjects.GameLogic.GetComponent<BuildDungeon>()
            .CheckTerrain(BuildDungeon.DungeonBlock.Pool, x, y);
    }

    private IEnumerator MoveAndWait()
    {
        while (true)
        {
            yield return wait5Seconds;

            if (countStep % 5 == 0)
            {
                Debug.Log("This step: " + countStep);
            }
            else
            {
                Debug.Log("Current step: " + countStep);
            }
        }
    }

    private void Start()
    {
        if (Instance)
        {
            Debug.Log("Move already exists.");
            return;
        }

        Instance = true;

        wait5Seconds = new WaitForSeconds(5.0f);
        mainUI = GameObject.FindGameObjectsWithTag("MainUI");

        for (int i = 0; i < mainUI.Length; i++)
        {
            if (mainUI[i].name == "Message")
            {
                message = mainUI[i].GetComponent<Text>();
                break;
            }
        }

        countStep = 0;

        StartCoroutine(MoveAndWait());
    }
}
