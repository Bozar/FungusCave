using Fungus.GameSystem.Progress;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
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
        public event EventHandler<SaveEventArgs> SavingDungeon;

        public string DungeonFile { get; private set; }

        public string GameFile { get; private set; }

        public void LoadDungeonLevel()
        {
            IDataTemplate[] data
                = GetComponent<SaveLoadFile>().LoadBinary(DungeonFile);

            foreach (IDataTemplate dt in data)
            {
                switch (dt.DTTag)
                {
                    case DataTemplateTag.Seed:
                        GetComponent<RandomNumber>().LoadBinary(dt);
                        break;

                    case DataTemplateTag.Progress:
                        GetComponent<DungeonProgressData>().LoadBinary(dt);
                        break;

                    default:
                        break;
                }
            }

            if (GetComponent<WizardMode>().IsWizardMode)
            {
                GetComponent<SaveLoadFile>().BackupBinary(DungeonFile);
            }
            GetComponent<SaveLoadFile>().DeleteBinary(DungeonFile);
        }

        public void SaveDungeonLevel(SaveEventArgs e)
        {
            OnSavingDungeon(e);
            GetComponent<SaveLoadFile>().SaveBinary(e.GameData.ToArray(),
                DungeonFile);
        }

        protected virtual void OnSavingDungeon(SaveEventArgs e)
        {
            SavingDungeon?.Invoke(this, e);
        }

        private void Awake()
        {
            DungeonFile = "dungeon.bin";
            GameFile = "save.bin";
        }
    }
}
