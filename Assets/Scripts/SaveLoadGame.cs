using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public class SaveLoadGame : MonoBehaviour
    {
        private Stack<IDataTemplate> dtStack;
        private string dungeonFile;
        private string gameFile;

        public void LoadDungeonLevel()
        {
            IDataTemplate[] data
                = GetComponent<SaveLoadFile>().LoadBinary(dungeonFile);

            foreach (IDataTemplate dt in data)
            {
                switch (dt.DataTag)
                {
                    case DataTemplateTag.Dungeon:
                        break;

                    case DataTemplateTag.Seed:
                        GetComponent<RandomNumber>().Load(dt);
                        break;

                    case DataTemplateTag.Actor:
                        break;

                    default:
                        break;
                }
            }

            if (!GetComponent<WizardMode>().IsWizardMode)
            {
                GetComponent<SaveLoadFile>().DeleteBinary(dungeonFile);
            }
        }

        public void SaveDungeonLevel()
        {
            dtStack = new Stack<IDataTemplate>();

            SaveSeed();

            GetComponent<SaveLoadFile>().SaveBinary(
                dtStack.ToArray(), dungeonFile);
        }

        private void Awake()
        {
            dungeonFile = "dungeon.bin";
            gameFile = "save.bin";
        }

        private void SaveSeed()
        {
            GetComponent<RandomNumber>().Save(out IDataTemplate data);
            dtStack.Push(data);
        }
    }
}
