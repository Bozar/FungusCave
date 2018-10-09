using UnityEngine;
using UnityEngine.UI;

public class BuildDungeon : MonoBehaviour
{
    public static readonly int[] buildWalls = { 1, 1, 0, 1, 1 };

    public static readonly int height = 17;
    public static readonly int width = 24;
    private GameObject[] mainUI;
    private Text message;

    private GameObject newPC;

    private GameObject newWall;
    private GameObject pcTile;
    private GameObject wallTile;

    private void Start()
    {
        wallTile = Resources.Load("Wall") as GameObject;
        pcTile = Resources.Load("PC") as GameObject;

        newPC = Instantiate(pcTile);
        newPC.transform.position = new Vector3(0, 0);

        for (int i = 0; i < buildWalls.Length; i++)
        {
            if (buildWalls[i] == 1)
            {
                newWall = Instantiate(wallTile);
                newWall.transform.position
                    = gameObject.GetComponent<ConvertCoordinates>()
                    .Convert(i, 3);
            }
        }

        mainUI = GameObject.FindGameObjectsWithTag("MainUI");

        for (int i = 0; i < mainUI.Length; i++)
        {
            if (mainUI[i].name == "Message")
            {
                message = mainUI[i].GetComponent<Text>();
                break;
            }
        }

        message.text = "Hello World\nThis is a test\n3\n4\n5\n6";
    }
}
