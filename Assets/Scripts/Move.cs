using UnityEngine;

public class Move : ActorTemplate
{
    private int moveStep;
    private float moveX;
    private float moveY;
    private bool repoted;

    private void MoveAround()
    {
        moveX = Input.GetAxis("Horizontal") * moveStep;
        moveY = Input.GetAxis("Vertical") * moveStep;
        transform.position += new Vector3(moveX, moveY);
    }

    // Use this for initialization
    private void Start()
    {
        moveStep = 1;
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
