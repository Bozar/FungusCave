using UnityEngine;
using UnityEngine.SceneManagement;

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

        FindObjects.GameLogic.GetComponent<RandomNumber>().InitializeSeed();

        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().Seed);
        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RNG.NextDouble());
        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RNG.NextDouble());
        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RNG.NextDouble());

        FindObjects.GameLogic.GetComponent<BlueprintSponge>().DrawBlueprint();
        FindObjects.GameLogic.GetComponent<BlueprintPool>().DrawBlueprint();
        FindObjects.GameLogic.GetComponent<BlueprintFungus>().DrawBlueprint();
        FindObjects.GameLogic.GetComponent<CreateWorld>().Initialize();

        FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
            FindObjects.GameLogic.GetComponent<RandomNumber>().Seed.ToString());
    }

    private void Update()
    {
        if (!Initialized)
        {
            InitializeGame();
        }
    }
}
