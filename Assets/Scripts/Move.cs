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

    private Text message;
    private float moveX;
    private float moveY;
    private string newDirection;
    private WaitForSeconds wait5Seconds;

    public void MoveAround(Transform actor)
    {
        moveX = 0;
        moveY = 0;
        newDirection = GetMoveKey();

        if (!IsWalkable(newDirection, actor))
        {
            Debug.Log("You are blocked.");
            message.text = "You are blocked";
            return;
        }

        switch (newDirection)
        {
            case "left":
                moveX = -moveStep;
                break;

            case "right":
                moveX = moveStep;
                break;

            case "down":
                moveY = -moveStep;
                break;

            case "up":
                moveY = moveStep;
                break;
        }

        if (newDirection != "wait")
        //if (!string.IsNullOrEmpty(newDirection))
        {
            message.text = "Hello World\n2\n3\n4\n5";

            actor.position += new Vector3(moveX, moveY);
            countStep++;
        }
    }

    private string GetMoveKey()
    {
        if (Input.GetKeyDown("left") || Input.GetKeyDown("h"))
        {
            return "left";
        }
        else if (Input.GetKeyDown("right") || Input.GetKeyDown("l"))
        {
            return "right";
        }
        else if (Input.GetKeyDown("down") || Input.GetKeyDown("j"))
        {
            return "down";
        }
        else if (Input.GetKeyDown("up") || Input.GetKeyDown("k"))
        {
            return "up";
        }

        return "wait";
    }

    private bool IsWalkable(string direction, Transform actor)
    {
        float x = actor.position.x;
        float y = actor.position.y;

        switch (direction)
        {
            case "left":
                x -= moveStep;
                break;

            case "right":
                x += moveStep;
                break;

            case "up":
                y += moveStep;
                break;

            case "down":
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
        message = GameObject.FindGameObjectWithTag("Message")
            .GetComponent<Text>();

        countStep = 0;

        StartCoroutine(MoveAndWait());
    }
}
