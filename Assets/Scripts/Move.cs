using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    private static bool Instance;
    private int[] buildWalls = BuildDungeon.buildWalls;

    private int countStep;

    private GameObject[] mainUI;
    private Text message;
    private Vector3 moveHere;

    private int newDirection;

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
            case (int)UserInput.Command.Left:
                moveHere
                    = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                    .Convert(-1, 0);
                break;

            case (int)UserInput.Command.Right:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(1, 0);
                break;

            case (int)UserInput.Command.Down:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(0, -1);
                break;

            case (int)UserInput.Command.Up:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(0, 1);
                break;

            case (int)UserInput.Command.UpLeft:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(-1, 1);
                break;

            case (int)UserInput.Command.UpRight:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(1, 1);
                break;

            case (int)UserInput.Command.DownLeft:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(-1, -1);
                break;

            case (int)UserInput.Command.DownRight:
                moveHere
                  = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                  .Convert(1, -1);
                break;
        }

        if (newDirection != (int)UserInput.Command.Invalid)
        //if (!string.IsNullOrEmpty(newDirection))
        {
            message.text = "Hello World\n2\n3\n4\n5\n6";

            actor.position += moveHere;
            countStep++;
        }
    }

    private bool IsWalkable(int direction, Transform actor)
    {
        int x = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(actor.position.x);
        int y = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(actor.position.y);

        switch (direction)
        {
            case (int)UserInput.Command.Left:
                x -= 1;
                break;

            case (int)UserInput.Command.Right:
                x += 1;
                break;

            case (int)UserInput.Command.Up:
                y += 1;
                break;

            case (int)UserInput.Command.Down:
                y -= 1;
                break;

            case (int)UserInput.Command.UpLeft:
                x -= 1;
                y += 1;
                break;

            case (int)UserInput.Command.UpRight:
                x += 1;
                y += 1;
                break;

            case (int)UserInput.Command.DownLeft:
                x -= 1;
                y -= 1;
                break;

            case (int)UserInput.Command.DownRight:
                x += 1;
                y -= 1;
                break;
        }

        if (x < 0 || y < 0 ||
           x >= BuildDungeon.width || y >= BuildDungeon.height)
        {
            return false;
        }
        else if (y == 3)
        {
            if (x < buildWalls.Length && x > -1)
            {
                return buildWalls[x] == 0;
            }
        }

        return true;
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
