using Fungus.GameSystem.Progress;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public class SaveLoadGame : MonoBehaviour
    {
        private ISaveLoadBinary[] saveGame;
        private ISaveLoadBinary[] saveLevel;

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
                        GetComponent<RandomNumber>().Load(dt);
                        break;

                    case DataTemplateTag.Progress:
                        GetComponent<DungeonProgressData>().Load(dt);
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

        public void SaveDungeonLevel()
        {
            Stack<IDataTemplate> dt = new Stack<IDataTemplate>();

            foreach (ISaveLoadBinary slb in saveLevel)
            {
                slb.Save(out IDataTemplate data);
                dt.Push(data);
            }
            GetComponent<SaveLoadFile>().SaveBinary(dt.ToArray(), DungeonFile);
        }

        private void Awake()
        {
            DungeonFile = "dungeon.bin";
            GameFile = "save.bin";
        }

        private void Start()
        {
            saveLevel = new ISaveLoadBinary[]
            {
                GetComponent<DungeonProgressData>(),
                GetComponent<RandomNumber>(),
            };
        }
    }
}
