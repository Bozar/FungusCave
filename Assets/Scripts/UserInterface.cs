using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private void Update()
    {
        UpdateSeed();
    }

    private void UpdateSeed()
    {
        FindObjects.MainUIDict[(int)FindObjects.UITags.Seed].
            GetComponent<Text>().text = "54321";
    }
}
