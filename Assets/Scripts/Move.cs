using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Move : ActorTemplate
{
    //public readonly float moveStep = 1.0f;

    public readonly float moveStep = 0.5f;
    private int[] buildWalls = BuildDungeon.buildWalls;

    //private int[] buildWalls = { 1, 1, 0, 1, 1 };
    private int countStep;

    private Text message;
    private float moveX;
    private float moveY;
    private string newDirection;
    private bool repoted;
    private WaitForSeconds wait5Seconds;

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

    private bool IsWalkable(string direction)
    {
        float x = transform.position.x;
        float y = transform.position.y;

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
                GameObject.FindGameObjectWithTag("Wall")
                    .GetComponent<Renderer>().enabled
                    = (buildWalls[xIndex] == 0);

                if (buildWalls[xIndex] != 0)
                {
                    Debug.Log("Pass through walls.");
                    message.text = "Pass through walls.";
                }

                return true;
                //return buildWalls[xIndex] == 0;
            }
        }
        else if (x < 0 || y < 0)
        {
            GameObject.FindGameObjectWithTag("Wall")
                    .GetComponent<Renderer>().enabled = true;
            return false;
        }

        GameObject.FindGameObjectWithTag("Wall")
                    .GetComponent<Renderer>().enabled = true;
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

    private void MoveAround()
    {
        moveX = 0;
        moveY = 0;
        newDirection = GetMoveKey();

        if (!IsWalkable(newDirection))
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

            transform.position += new Vector3(moveX, moveY);
            countStep++;
        }
    }

    private void Start()
    {
        repoted = false;
        wait5Seconds = new WaitForSeconds(5.0f);
        message = GameObject.FindGameObjectWithTag("Message")
            .GetComponent<Text>();

        countStep = 0;

        StartCoroutine(MoveAndWait());
    }

    private void Update()
    {
        MoveAround();

        if (repoted)
        {
            return;
        }

        //if (GetComponentInParent<PCInfo>().ActorName == "Player")
        if (GetComponentInParent<PCInfo>().ActorName == "Player Character")
        {
            Debug.Log("This is player");
        }
        else
        {
            Debug.Log("This is stranger");
        }

        repoted = true;
    }
}
