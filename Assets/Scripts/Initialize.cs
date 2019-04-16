using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
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
            var lb = GetComponent<SaveLoad>().LoadBinary("test.bin");
            MySave ms = lb[0] as MySave;

            Debug.Log(ms.MyNum);
            Debug.Log(ms.MyStr);

            Debug.Log(ms.MyDict[0].MyNum);
            Debug.Log(ms.MyDict[0].MyStr);

            Debug.Log(ms.NewSave);
        }

        private void TestSave()
        {
            MySave ms = new MySave { };
            ms.MyNum = 42;
            ms.MyStr = "Hello world";
            ms.MyDict = new Dictionary<int, MySave>
            { { 0, new MySave { MyNum = 24, MyStr = "Good night" } } };
            ms.NewSave = new NewSave { Name = "My new save" }.Name;

            var mySave = new ISaveLoad[] { ms };
            GetComponent<SaveLoad>().SaveBinary(mySave, "test.bin");

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
    public class MySave : ISaveLoad
    {
        public Dictionary<int, MySave> MyDict;
        public int MyNum;
        public string MyStr;
        public string NewSave;

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }

    public class NewSave
    {
        public string Name;
    }
}
