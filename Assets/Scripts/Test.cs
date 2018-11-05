using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public static readonly int height = 17;
    public static readonly int width = 24;

    private GameObject[] mainUI;
    private Text message;

    public bool RenderAll { get; set; }

    private void Awake()
    {
        RenderAll = false;
    }

    private void Start()
    {
        FindObjects.GameLogic.GetComponent<ObjectPool>()
            .CreateObject(ObjectTag.PC, 0, 0);

        for (int i = 0; i < 2; i++)
        {
            FindObjects.GameLogic.GetComponent<ObjectPool>()
                .CreateObject(ObjectTag.Dummy, i * 2, i + 1);
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
