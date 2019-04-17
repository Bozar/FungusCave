using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public enum SLDataTag { INVALID, Dungeon, Seed, Actor };

    public interface IDataTemplate
    {
        SLDataTag DataTag { get; }
    }

    public class SaveLoadGame : MonoBehaviour
    {
        //private string dungeonFile;
        //private string gameFile;

        public void LoadTestData()
        {
            IDataTemplate[] load = GetComponent<SaveLoadFile>().LoadBinary("test.bin");
            TestData data = load[0] as TestData;

            Debug.Log(data.MyNum);
            Debug.Log(data.MyStr);
            Debug.Log(data.MyDict[123]);
        }

        public void SaveTestData()
        {
            Stack<IDataTemplate> save = new Stack<IDataTemplate>();

            TestData data = new TestData { };
            data.MyNum = 42;
            data.MyStr = "Hello world";
            data.MyDict = new Dictionary<int, string>() { { 123, "Unity" } };

            save.Push(data);
            GetComponent<SaveLoadFile>().SaveBinary(save.ToArray(), "test.bin");
        }

        //private void Awake()
        //{
        //    dungeonFile = "dungeon.bin";
        //    gameFile = "save.bin";
        //}
    }

    [Serializable]
    public class TestData : IDataTemplate
    {
        public Dictionary<int, string> MyDict;
        public int MyNum;
        public string MyStr;

        public SLDataTag DataTag { get { return SLDataTag.INVALID; } }
    }
}
