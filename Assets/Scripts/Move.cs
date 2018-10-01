using System.Collections;
using UnityEngine;

public class Move : ActorTemplate
{
    private int countStep;
    private float moveStep = 0.5f;
    private float moveX;
    private float moveY;
    private string newDirection;
    private bool repoted;
    private WaitForSeconds wait5Seconds;
    private GameObject wallTile;

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

    private IEnumerator moveAndWait()
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
            transform.position += new Vector3(moveX, moveY);
            countStep++;
        }
    }

    private void Start()
    {
        repoted = false;
        wait5Seconds = new WaitForSeconds(5.0f);
        countStep = 0;
        wallTile = Resources.Load("Prefabs/Wall") as GameObject;

        StartCoroutine(moveAndWait());

        for (int i = 0; i < 5; i++)
        {
            Instantiate(wallTile, new Vector3(i * moveStep, 1),
                Quaternion.identity);
        }
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
