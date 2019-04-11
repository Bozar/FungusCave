using Fungus.GameSystem.WorldBuilding;
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
        }

        private void Update()
        {
            if (!Initialized)
            {
                InitializeGame();
            }
        }
    }
}
