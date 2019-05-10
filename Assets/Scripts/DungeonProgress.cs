using Fungus.Actor;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public class DungeonProgress : MonoBehaviour, ISaveLoadBinary
    {
        private ActorData actorData;
        private int kill;
        private DungeonProgressData progress;

        public void CountKill(GameObject actor)
        {
            SubObjectTag tag = actor.GetComponent<MetaInfo>().SubTag;

            if (actorData.GetIntData(tag, DataTag.Potion) > 0)
            {
                kill++;
            }
        }

        public bool IsWin()
        {
            DungeonLevel current = GetComponent<DungeonProgressData>()
                .GetDungeonLevel();
            DungeonLevel next = GetComponent<DungeonProgressData>()
                .GetNextLevel();

            return (current == next) && LevelCleared();
        }

        public bool LevelCleared()
        {
            return kill >= progress.MaxSoldier;
        }

        public void LoadBinary(IDataTemplate[] dt)
        {
            foreach (IDataTemplate d in dt)
            {
                if (d.DTTag == DataTemplateTag.Kill)
                {
                    DTDungeonProgress value = d as DTDungeonProgress;
                    kill = value.KillCount;
                    return;
                }
            }
        }

        public void SaveBinary(Stack<IDataTemplate> dt)
        {
            DTDungeonProgress data = new DTDungeonProgress
            {
                KillCount = kill
            };
            dt.Push(data);
        }

        private void Awake()
        {
            kill = 0;
        }

        private void DungeonProgress_LoadingGame(object sender, LoadEventArgs e)
        {
            LoadBinary(e.GameData);
        }

        private void DungeonProgress_SavingGame(object sender, SaveEventArgs e)
        {
            SaveBinary(e.GameData);
        }

        private void Start()
        {
            progress = GetComponent<DungeonProgressData>();
            actorData = GetComponent<ActorData>();

            GetComponent<SaveLoadGame>().SavingGame
                += DungeonProgress_SavingGame;
            GetComponent<SaveLoadGame>().LoadingGame
                += DungeonProgress_LoadingGame;
        }
    }
}
