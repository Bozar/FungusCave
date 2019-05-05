using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public class LoadEventArgs : EventArgs
    {
        public IDataTemplate[] GameData;

        public LoadEventArgs(IDataTemplate[] gameData)
        {
            GameData = gameData;
        }
    }

    public class SaveEventArgs : EventArgs
    {
        public Stack<IDataTemplate> GameData;

        public SaveEventArgs(Stack<IDataTemplate> gameData)
        {
            GameData = gameData;
        }
    }

    public class SaveLoadGame : MonoBehaviour
    {
        public event EventHandler<LoadEventArgs> LoadingDungeon;

        public event EventHandler<LoadEventArgs> LoadingGame;

        public event EventHandler<SaveEventArgs> SavingDungeon;

        public event EventHandler<SaveEventArgs> SavingGame;

        public string DungeonFile { get; private set; }

        public string GameFile { get; private set; }

        public void LoadDungeonLevel(LoadEventArgs e)
        {
            OnLoadingDungeon(e);
            if (GetComponent<WizardMode>().IsWizardMode)
            {
                GetComponent<SaveLoadFile>().BackupBinary(DungeonFile);
            }
            GetComponent<SaveLoadFile>().DeleteBinary(DungeonFile);
        }

        public void LoadGame(LoadEventArgs e)
        {
            OnLoadingGame(e);
            if (GetComponent<WizardMode>().IsWizardMode)
            {
                GetComponent<SaveLoadFile>().BackupBinary(GameFile);
            }
            GetComponent<SaveLoadFile>().DeleteBinary(GameFile);
        }

        public void SaveDungeonLevel(SaveEventArgs e)
        {
            OnSavingDungeon(e);
            GetComponent<SaveLoadFile>().SaveBinary(e.GameData.ToArray(),
                DungeonFile);
        }

        public void SaveGame(SaveEventArgs e)
        {
            OnSavingGame(e);
            GetComponent<SaveLoadFile>().SaveBinary(e.GameData.ToArray(),
                GameFile);
        }

        protected virtual void OnLoadingDungeon(LoadEventArgs e)
        {
            LoadingDungeon?.Invoke(this, e);
        }

        protected virtual void OnLoadingGame(LoadEventArgs e)
        {
            LoadingGame?.Invoke(this, e);
        }

        protected virtual void OnSavingDungeon(SaveEventArgs e)
        {
            SavingDungeon?.Invoke(this, e);
        }

        protected virtual void OnSavingGame(SaveEventArgs e)
        {
            SavingGame?.Invoke(this, e);
        }

        private void Awake()
        {
            DungeonFile = "dungeon.bin";
            GameFile = "save.bin";
        }
    }
}
