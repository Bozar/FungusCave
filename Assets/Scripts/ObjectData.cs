using System.Collections.Generic;
using UnityEngine;

public enum DataTag { HP }

public class ObjectData : MonoBehaviour
{
    private Dictionary<SubObjectTag, Dictionary<DataTag, int>> intData;
    private int invalidData;

    public int GetIntData(SubObjectTag oTag, DataTag dTag)
    {
        Dictionary<DataTag, int> dataDict = new Dictionary<DataTag, int>();
        int gameData;

        if (intData.TryGetValue(oTag, out dataDict))
        {
            if (dataDict.TryGetValue(dTag, out gameData))
            {
                return gameData;
            }
        }

        return invalidData;
    }

    private void AddIntData(SubObjectTag oTag, DataTag dTag, int data)
    {
        Dictionary<DataTag, int> dataDict = new Dictionary<DataTag, int>();

        if (intData.TryGetValue(oTag, out dataDict))
        {
            if (!dataDict.ContainsKey(dTag))
            {
                dataDict.Add(dTag, data);
            }
        }
    }

    private void Awake()
    {
        invalidData = -99999;

        intData = new Dictionary<SubObjectTag, Dictionary<DataTag, int>>
        {
            { SubObjectTag.PC, new Dictionary<DataTag, int>() }
        };
    }

    private void InitializeData()
    {
        AddIntData(SubObjectTag.PC, DataTag.HP, 10);
    }

    private void Start()
    {
        InitializeData();
    }
}
