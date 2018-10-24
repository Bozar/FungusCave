using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialize : MonoBehaviour
{
    private bool initialized;

    public void InitializeGame()
    {
        if (initialized)
        {
            SceneManager.LoadSceneAsync(0);
            SceneManager.UnloadSceneAsync(0);
            return;
        }

        initialized = true;

        FindObjects.GameLogic.GetComponent<RandomNumber>().InitializeSeed();

        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().Seed);
        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RNG.NextDouble());
        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RNG.NextDouble());
        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RNG.NextDouble());

        FindObjects.GameLogic.GetComponent<BlueprintSponge>().DrawBlueprint();
        FindObjects.GameLogic.GetComponent<BlueprintPool>().DrawBlueprint();
        FindObjects.GameLogic.GetComponent<BlueprintFungus>().DrawBlueprint();
        FindObjects.GameLogic.GetComponent<DungeonObjects>().CreateBuildings();

        GameObject.FindGameObjectWithTag("PC").GetComponent<FieldOfView>()
            .UpdateFOV();
    }
}
