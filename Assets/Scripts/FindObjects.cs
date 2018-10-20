using System;
using System.Collections.Generic;
using UnityEngine;

// A helper class that stores references to other game objects. The ONLY game
// object which it can be and MUST be attached to is GameLogic.
public class FindObjects : MonoBehaviour
{
    private int tempDictKey;
    private GameObject[] tempGOArray;

    public enum UITags { NONE, Seed };

    public static GameObject GameLogic { get; private set; }
    public static Dictionary<int, GameObject> MainUIDict { get; private set; }

    private void Awake()
    {
        // Object.
        GameLogic = gameObject;

        MainUIDict = new Dictionary<int, GameObject>();
    }

    private void Start()
    {
        // ObjectS.
        tempGOArray = GameObject.FindGameObjectsWithTag("MainUI");

        foreach (var go in tempGOArray)
        {
            if (Enum.IsDefined(typeof(UITags), go.name))
            {
                tempDictKey = (int)Enum.Parse(typeof(UITags), go.name);

                if (!MainUIDict.ContainsKey(tempDictKey))
                {
                    MainUIDict.Add(tempDictKey, go);
                }
            }
        }
    }
}
