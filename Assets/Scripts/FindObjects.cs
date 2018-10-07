using UnityEngine;

//A helper class that stores references to other game objects.
public class FindObjects : MonoBehaviour
{
    public static GameObject GameLogic { get; private set; }
    public static GameObject[] MainUIList { get; private set; }

    private void Start()
    {
        //Object.
        GameLogic = GameObject.FindGameObjectWithTag("GameLogic");

        //ObjectS.
        MainUIList = GameObject.FindGameObjectsWithTag("MainUI");
    }
}
