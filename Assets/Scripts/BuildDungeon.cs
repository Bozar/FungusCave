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
    private GameObject wallTile;

    private void Start()
    {
        wallTile = Resources.Load("Prefabs/Wall") as GameObject;
        pcTile = Resources.Load("Prefabs/PC") as GameObject;

        newPC = Instantiate(pcTile);
        newPC.transform.position = new Vector3(0, 0);
        moveStep = Move.Instance.moveStep;

        for (int i = 0; i < buildWalls.Length; i++)
        {
            if (buildWalls[i] == 1)
            {
                newWall = Instantiate(wallTile);
                newWall.transform.position = new Vector3(i * moveStep, 1.5f);
            }
        }

        message = GameObject.FindGameObjectWithTag("Message")
            .GetComponent<Text>();
        message.text = "Hello World\nThis is a test\n3\n4\n5";
    }
}
