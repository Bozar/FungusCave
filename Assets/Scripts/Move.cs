using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    //public static Move Instance;
    public readonly float moveStep = 0.5f;

    private static bool Instance;
    private int[] buildWalls = BuildDungeon.buildWalls;

    //private int[] buildWalls = { 1, 1, 0, 1, 1 };
    private int countStep;

    private GameObject[] mainUI;
    private Text message;
    private float moveX;
    private float moveY;
    private int newDirection;
    private WaitForSeconds wait5Seconds;

    public void MoveAround(Transform actor)
    {
        moveX = 0;
        moveY = 0;
        newDirection = gameObject.GetComponent<UserInput>().OutputCommand();

        if (!IsWalkable(newDirection, actor))
        {
            Debug.Log("You are blocked.");
            message.text = "You are blocked";
            return;
        }

        switch (newDirection)
        {
            case (int)UserInput.Command.Left:
                moveX = -moveStep;
                break;

            case (int)UserInput.Command.Right:
                moveX = moveStep;
                break;

            case (int)UserInput.Command.Down:
                moveY = -moveStep;
                break;

            case (int)UserInput.Command.Up:
                moveY = moveStep;
                break;
        }

        if (newDirection != (int)UserInput.Command.Invalid)
        //if (!string.IsNullOrEmpty(newDirection))
        {
            message.text = "Hello World\n2\n3\n4\n5";

            actor.position += new Vector3(moveX, moveY);
            countStep++;
        }
    }

    private bool IsWalkable(int direction, Transform actor)
    {
        float x = actor.position.x;
        float y = actor.position.y;

        switch (direction)
        {
            case (int)UserInput.Command.Left:
                x -= moveStep;
                break;

            case (int)UserInput.Command.Right:
                x += moveStep;
                break;

            case (int)UserInput.Command.Up:
                y += moveStep;
                break;

            case (int)UserInput.Command.Down:
                y -= moveStep;
                break;
        }

        if (Mathf.Round(y * 2) == 3)
        {
            int xIndex = (int)(x * 2);

            if (xIndex < buildWalls.Length && xIndex > -1)
            {
                return buildWalls[xIndex] == 0;
            }
        }
        else if (x < 0 || y < 0)
        {
            return false;
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
