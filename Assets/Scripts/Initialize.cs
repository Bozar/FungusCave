using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fungus.GameSystem
{
    public interface IInitialize
    {
        void Initialize();
    }

    public class Initialize : MonoBehaviour
    {
        public bool Initialized { get; private set; }

        public void InitializeGame()
        {
            if (Initialized)
            {
                SceneManager.LoadSceneAsync(0);
                //SceneManager.UnloadSceneAsync(0);
                return;
            }

            Initialized = true;

            GetComponent<RandomNumber>().Initialize();
            Debug.Log(GetComponent<RandomNumber>().RootSeed);

            GetComponent<BlueprintSponge>().DrawBlueprint();
            GetComponent<BlueprintPool>().DrawBlueprint();
            GetComponent<BlueprintFungus>().DrawBlueprint();
            GetComponent<CreateWorld>().Initialize();
            GetComponent<DungeonTerrain>().Initialize();

            GetComponent<SubMode>().SwitchModeOpening(
                GetComponent<GameSetting>().ShowOpening);

            //TestSave();
            //TestLoad();
        }

        private void TestLoad()
        {
            IFormatter formatter = new BinaryFormatter();
            MySave ms;

            using (FileStream s = File.OpenRead("serialized.bin"))
            {
                ms = (MySave)formatter.Deserialize(s);
            }

            Debug.Log(ms.MyNum);
            Debug.Log(ms.MyStr);

            Debug.Log(ms.MyDict[0].MyNum);
            Debug.Log(ms.MyDict[0].MyStr);
        }

        private void TestSave()
        {
            MySave ms = new MySave { };
            ms.MyNum = 42;
            ms.MyStr = "Hello world";
            ms.MyDict = new Dictionary<int, MySave>
            { { 0, new MySave { MyNum = 24, MyStr = "Good night" } } };
            IFormatter formatter = new BinaryFormatter();

            using (FileStream s = File.Create("serialized.bin"))
            { formatter.Serialize(s, ms); }

            Debug.Log("File save");
        }

        private void Update()
        {
            if (!Initialized)
            {
                InitializeGame();
            }
        }
    }

    [Serializable]
    public class MySave
    {
        public int MyNum;
        public string MyStr;
        internal Dictionary<int, MySave> MyDict;
    }
}
