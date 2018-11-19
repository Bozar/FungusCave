using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UITag { NONE, Seed, Message, Modeline, Terrain, HPData, StressData };

// A helper class that stores references to other game objects. The ONLY game
// object which it CAN be and MUST be attached to is GameLogic.
public class FindObjects : MonoBehaviour
{
    private static Dictionary<UITag, GameObject> mainUIDict;

    public static GameObject GameLogic { get; private set; }

    public static GameObject GetUIObject(UITag tag)
    {
        return mainUIDict[tag];
    }

    private void Awake()
    {
        GameLogic = gameObject;
        mainUIDict = new Dictionary<UITag, GameObject>();
    }

    private void InitializeUIDict()
    {
        // NOTE: If a GameObject's tag is MainUI AND its name is stored in
        // UITags, it can be found in MainUIDict.
        UITag tempDictKey;
        GameObject[] tempGOArray = GameObject.FindGameObjectsWithTag("MainUI");

        foreach (var go in tempGOArray)
        {
            if (Enum.IsDefined(typeof(UITag), go.name))
            {
                tempDictKey = (UITag)Enum.Parse(typeof(UITag), go.name);

                if (!mainUIDict.ContainsKey(tempDictKey))
                {
                    mainUIDict.Add(tempDictKey, go);
                    go.GetComponent<Text>().text = "";
                }
            }
        }
    }

    private void Start()
    {
        InitializeUIDict();
    }
}
