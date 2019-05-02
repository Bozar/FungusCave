using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Progress;
using Fungus.GameSystem.SaveLoadData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fungus.Actor.Turn
{
    public class GuideAction : MonoBehaviour
    {
        private DungeonProgress progress;
        private SaveLoadGame saveLoad;

        private void Reload()
        {
            if ((FindObjects.PC.GetComponent<HP>().CurrentHP < 1)
                || progress.IsWin())
            {
                SceneManager.LoadSceneAsync(0);
            }
            // progress.LevelCleared() is a sub condition of progress.IsWin().
            else if (progress.LevelCleared())
            {
                Stack<IDataTemplate> dt = new Stack<IDataTemplate>();
                saveLoad.SaveDungeonLevel(new SaveLoadEventArgs(dt));
                SceneManager.LoadSceneAsync(0);
            }
        }

        private void Start()
        {
            progress = FindObjects.GameLogic.GetComponent<DungeonProgress>();
            saveLoad = FindObjects.GameLogic.GetComponent<SaveLoadGame>();
        }

        private void Update()
        {
            switch (GetComponent<PlayerInput>().GameCommand())
            {
                case Command.Confirm:
                    Reload();
                    break;
            }
        }
    }
}
