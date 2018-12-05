using Fungus.Render;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fungus.GameSystem
{
    public class Initialize : MonoBehaviour
    {
        public bool Initialized { get; private set; }

        public void InitializeGame()
        {
            if (Initialized)
            {
                SceneManager.LoadSceneAsync(0);
                SceneManager.UnloadSceneAsync(0);
                return;
            }

            Initialized = true;

            FindObjects.GameLogic.GetComponent<RandomNumber>().InitializeSeeds();

            Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RootSeed);

            FindObjects.GameLogic.GetComponent<BlueprintSponge>().DrawBlueprint();
            FindObjects.GameLogic.GetComponent<BlueprintPool>().DrawBlueprint();
            FindObjects.GameLogic.GetComponent<BlueprintFungus>().DrawBlueprint();
            FindObjects.GameLogic.GetComponent<CreateWorld>().Initialize();

            FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
                FindObjects.GameLogic.GetComponent<RandomNumber>().RootSeed.ToString());
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
