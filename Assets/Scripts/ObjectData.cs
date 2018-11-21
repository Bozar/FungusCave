using System.Collections.Generic;
using UnityEngine;

public enum DataTag { HP, Stress, Damage, MaxInfections }

public class ObjectData : MonoBehaviour
{
    private Dictionary<SubObjectTag, Dictionary<DataTag, int>> intData;
    private int invalidData;

    public int GetIntData(SubObjectTag oTag, DataTag dTag)
    {
        Dictionary<DataTag, int> dataDict;
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
        if (!intData.ContainsKey(oTag))
        {
            intData.Add(oTag, new Dictionary<DataTag, int>());
        }

        if (!intData[oTag].ContainsKey(dTag))
        {
            intData[oTag].Add(dTag, data);
        }
    }

    private void Awake()
    {
        invalidData = -99999;
        intData = new Dictionary<SubObjectTag, Dictionary<DataTag, int>>();

        InitializeData();
    }

    private void InitializeData()
    {
        AddIntData(SubObjectTag.PC, DataTag.HP, 10);
        AddIntData(SubObjectTag.PC, DataTag.Stress, 3);
        AddIntData(SubObjectTag.PC, DataTag.Damage, 3);
        AddIntData(SubObjectTag.PC, DataTag.MaxInfections, 2);

        AddIntData(SubObjectTag.Dummy, DataTag.HP, 3);
    }
}
