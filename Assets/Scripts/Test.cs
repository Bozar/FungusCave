using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public static readonly int height = 17;
    public static readonly int width = 24;
    private GameObject dummyTile;
    private GameObject[] mainUI;
    private Text message;

    private GameObject newDummy;
    private GameObject newPC;

    private GameObject pcTile;

    private void Start()
    {
        pcTile = Resources.Load("PC") as GameObject;
        dummyTile = Resources.Load("Dummy") as GameObject;

        newPC = Instantiate(pcTile);
        newPC.transform.position = new Vector3(0, 0);
        newPC.AddComponent<PCMove>();
        newPC.AddComponent<TileOverlay>();
        newPC.AddComponent<FieldOfView>();
        newPC.AddComponent<RenderSprite>();

        gameObject.GetComponent<SchedulingSystem>().AddActor(newPC);

        for (int i = 0; i < 2; i++)
        {
            newDummy = Instantiate(dummyTile);
            newDummy.transform.position
                   = gameObject.GetComponent<ConvertCoordinates>()
                   .Convert(i * 2, i + 1);

            newDummy.AddComponent<PCMove>().enabled = false;
            newDummy.AddComponent<TileOverlay>();
            //newDummy.AddComponent<FieldOfView>();
            newDummy.AddComponent<RenderSprite>();

            gameObject.GetComponent<SchedulingSystem>().AddActor(newDummy);
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

        message.text =
            FindObjects.GameLogic.GetComponent<RandomNumber>().Seed +
            "\nThis is a test\n3\n4\n5\n6";
    }
}
