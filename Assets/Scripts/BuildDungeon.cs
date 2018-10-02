using UnityEngine;
using UnityEngine.UI;

public class BuildDungeon : MonoBehaviour
{
    public static readonly int[] buildWalls = { 1, 1, 0, 1, 1 };

    private Text message;
    private float moveStep;
    private GameObject newPC;
    private GameObject newWall;
    private GameObject pcTile;
    private Transform relativePosition;
    private GameObject wallTile;

    private void Start()
    {
        wallTile = Resources.Load("Prefabs/Wall") as GameObject;
        pcTile = Resources.Load("Prefabs/PC") as GameObject;

        relativePosition
            = GameObject.FindGameObjectWithTag("GameManager").transform;

        newPC = Instantiate(pcTile);
        newPC.transform.SetParent(relativePosition);
        newPC.transform.localPosition = new Vector3(0, 4);
        moveStep = newPC.GetComponent<Move>().moveStep;

        for (int i = 0; i < buildWalls.Length; i++)
        {
            if (buildWalls[i] == 1)
            {
                newWall = Instantiate(wallTile);
                newWall.transform.SetParent(relativePosition);
                newWall.transform.localPosition = new Vector3(i * moveStep, 5);
            }
        }

        message = GameObject.FindGameObjectWithTag("Message").GetComponent<Text>();
        message.text = "Hello World\nThis is a test\n3\n4\n5";
    }
}
