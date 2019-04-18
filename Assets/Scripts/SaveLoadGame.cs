using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public class SaveLoadGame : MonoBehaviour
    {
        private string dungeonFile;
        private string gameFile;
        private ISaveLoadBinary[] saveGame;
        private ISaveLoadBinary[] saveLevel;

        public void LoadDungeonLevel()
        {
            IDataTemplate[] data
                = GetComponent<SaveLoadFile>().LoadBinary(dungeonFile);

            foreach (IDataTemplate dt in data)
            {
                switch (dt.DTTag)
                {
                    case DataTemplateTag.Dungeon:
                        break;

                    case DataTemplateTag.Seed:
                        GetComponent<RandomNumber>().Load(dt);
                        break;

                    case DataTemplateTag.Progress:
                        GetComponent<ProgressData>().Load(dt);
                        break;

                    case DataTemplateTag.Actor:
                        break;

                    default:
                        break;
                }
            }

            if (GetComponent<WizardMode>().IsWizardMode)
            {
                GetComponent<SaveLoadFile>().BackupBinary(dungeonFile);
            }
            GetComponent<SaveLoadFile>().DeleteBinary(dungeonFile);
        }

        public void SaveDungeonLevel()
        {
            Stack<IDataTemplate> dt = new Stack<IDataTemplate>();

            foreach (ISaveLoadBinary slb in saveLevel)
            {
                slb.Save(out IDataTemplate data);
                dt.Push(data);
            }
            GetComponent<SaveLoadFile>().SaveBinary(dt.ToArray(), dungeonFile);
        }

        private void Awake()
        {
            dungeonFile = "dungeon.bin";
            gameFile = "save.bin";
        }

        private void Start()
        {
            saveLevel = new ISaveLoadBinary[]
            {
                GetComponent<ProgressData>(), GetComponent<RandomNumber>(),
            };
        }
    }
}
