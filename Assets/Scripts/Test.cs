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
    public bool RenderAll { get; set; }

    private void Awake()
    {
        RenderAll = false;
    }

    private void Start()
    {
        pcTile = Resources.Load("PC") as GameObject;
        dummyTile = Resources.Load("Dummy") as GameObject;

        newPC = Instantiate(pcTile);
        newPC.transform.position = new Vector3(0, 0);
        newPC.AddComponent<PCActions>();
        newPC.AddComponent<TileOverlay>();
        newPC.AddComponent<FieldOfView>();
        newPC.AddComponent<RenderSprite>();
        newPC.AddComponent<FOVRhombus>();
        newPC.AddComponent<FOVSimple>();
        newPC.AddComponent<Energy>();
        newPC.AddComponent<PlayerInput>();
        newPC.AddComponent<Move>();
        newPC.AddComponent<InternalClock>();
        newPC.AddComponent<Attack>();
        newPC.AddComponent<Defend>();

        gameObject.GetComponent<SchedulingSystem>().AddActor(newPC);
        gameObject.GetComponent<ActorBoard>().AddActor(newPC, 0, 0);

        for (int i = 0; i < 2; i++)
        {
            newDummy = Instantiate(dummyTile);
            newDummy.transform.position
                   = gameObject.GetComponent<ConvertCoordinates>()
                   .Convert(i * 2, i + 1);

            gameObject.GetComponent<ActorBoard>().AddActor(
                newDummy, i * 2, i + 1);

            newDummy.AddComponent<TileOverlay>();
            //newDummy.AddComponent<FieldOfView>();
            newDummy.AddComponent<RenderSprite>();
            newDummy.AddComponent<PlayerInput>();
            newDummy.AddComponent<PCActions>().enabled = false;
            newDummy.AddComponent<Move>();
            newDummy.AddComponent<Energy>();
            newDummy.AddComponent<InternalClock>();
            newDummy.AddComponent<Attack>();
            newDummy.AddComponent<Defend>();

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
