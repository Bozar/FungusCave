using UnityEngine;

public class Initialize : MonoBehaviour
{
    private bool initialized;

    public void InitializeGame()
    {
        if (initialized)
        {
            return;
        }

        initialized = true;

        FindObjects.GameLogic.GetComponent<RandomNumber>().InitializeSeed();

        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().Seed);
        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RNG.NextDouble());
        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RNG.NextDouble());
        Debug.Log(FindObjects.GameLogic.GetComponent<RandomNumber>().RNG.NextDouble());

        FindObjects.GameLogic.GetComponent<DungeonBlueprint>()
           .DrawRandomly();
        FindObjects.GameLogic.GetComponent<DungeonObjects>().CreateBuildings();
    }
}
