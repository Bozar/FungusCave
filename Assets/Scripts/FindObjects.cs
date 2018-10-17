using UnityEngine;

// A helper class that stores references to other game objects. The ONLY game
// object which it can be and MUST be attached to is GameLogic.
public class FindObjects : MonoBehaviour
{
    public static GameObject GameLogic { get; private set; }
    public static GameObject[] MainUIList { get; private set; }

    private void Awake()
    {
        // Object.
        GameLogic = gameObject;
    }

    private void Start()
    {
        // ObjectS.
        MainUIList = GameObject.FindGameObjectsWithTag("MainUI");
    }
}
