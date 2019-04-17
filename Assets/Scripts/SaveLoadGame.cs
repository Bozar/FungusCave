using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public class SaveLoadGame : MonoBehaviour
    {
        private List<ISaveLoad> dungeonData;
        private string dungeonFile;
        private List<ISaveLoad> gameData;
        private string gameFile;

        public void AddDungeonData(ISaveLoad data)
        {
            dungeonData.Add(data);
        }

        public void AddGameData(ISaveLoad data)
        {
            gameData.Add(data);
        }

        public void LoadDungeonData()
        {
            LoadData(dungeonFile);
        }

        public void LoadGameData()
        {
            LoadData(gameFile);
        }

        public void RemoveDungeonData(ISaveLoad data)
        {
            dungeonData.Remove(data);
        }

        public void RemoveGameData(ISaveLoad data)
        {
            gameData.Remove(data);
        }

        public void SaveDungeonData()
        {
            SaveData(dungeonData, dungeonFile);
        }

        public void SaveGameData()
        {
            SaveData(gameData, gameFile);
        }

        private void Awake()
        {
            dungeonFile = "dungeon.bin";
            gameFile = "save.bin";
        }

        private void LoadData(string fileName)
        {
            ISaveLoad[] data = GetComponent<SaveLoadFile>().LoadBinary(fileName);
            foreach (ISaveLoad sl in data)
            {
                sl.Load();
            }
            // TODO: Remove the save file.
        }

        private void SaveData(List<ISaveLoad> data, string fileName)
        {
            if (data.Count < 1)
            {
                return;
            }

            foreach (ISaveLoad sl in data)
            {
                sl.Save();
            }
            GetComponent<SaveLoadFile>().SaveBinary(data.ToArray(), fileName);
        }
    }
}
