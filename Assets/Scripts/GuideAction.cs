using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.SaveLoadData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fungus.Actor.Turn
{
    public class GuideAction : MonoBehaviour
    {
        private Progress progress;
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
                saveLoad.SaveDungeonLevel();
                SceneManager.LoadSceneAsync(0);
            }
        }

        private void Start()
        {
            progress = FindObjects.GameLogic.GetComponent<Progress>();
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
