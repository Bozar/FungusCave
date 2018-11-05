using System;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectTag { TEMP, Dummy, PC };

public class ObjectPool : MonoBehaviour
{
    private Dictionary<ObjectTag, Stack<GameObject>> pool;

    private void Awake()
    {
        pool = new Dictionary<ObjectTag, Stack<GameObject>>();

        foreach (var tag in Enum.GetValues(typeof(ObjectTag)))
        {
            pool.Add((ObjectTag)tag, new Stack<GameObject>());
        }
    }
}
