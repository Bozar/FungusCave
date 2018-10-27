using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private string printText;

    private void LateUpdate()
    {
        UpdateSeed();
        FindObjects.GameLogic.GetComponent<UIMessage>().StoreText("Hello world");
        FindObjects.GameLogic.GetComponent<UIMessage>().StoreText("Test input");
        FindObjects.GameLogic.GetComponent<UIMessage>().PrintText();
    }

    private void UpdateSeed()
    {
        printText = FindObjects.GameLogic.GetComponent<RandomNumber>()
            .Seed.ToString();
        int textLength = printText.Length;

        for (int i = 1; textLength > i * 3; i++)
        {
            printText = printText.Insert(i * 4 - 1, "-");
        }

        FindObjects.MainUIDict[(int)FindObjects.UITags.Seed].
            GetComponent<Text>().text = printText;
    }
}
