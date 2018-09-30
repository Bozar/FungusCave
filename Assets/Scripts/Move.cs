using UnityEngine;

public class Move : ActorTemplate
{
    private int moveX;
    private int moveY;
    private string newDirection;
    private bool repoted;

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

    private void MoveAround()
    {
        moveX = 0;
        moveY = 0;
        newDirection = GetMoveKey();

        switch (newDirection)
        {
            case "left":
                moveX = -1;
                break;

            case "right":
                moveX = 1;
                break;

            case "down":
                moveY = -1;
                break;

            case "up":
                moveY = 1;
                break;
        }

        transform.position += new Vector3(moveX, moveY);
    }

    // Use this for initialization
    private void Start()
    {
        repoted = false;

        //transform.position = new Vector3(0f, 0f);
        //pcPosition = transform.position;
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
