using UnityEngine;

public class AttachScripts : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<Singleton>();
        gameObject.AddComponent<FindObjects>();
        gameObject.AddComponent<UserInput>();
        gameObject.AddComponent<ConvertCoordinates>();
        gameObject.AddComponent<SchedulingSystem>();
        gameObject.AddComponent<BuildDungeon>();
        gameObject.AddComponent<TileOverlay>();
        gameObject.AddComponent<RandomNumber>();
        gameObject.AddComponent<SaveLoad>();

        gameObject.AddComponent<Test>();
        gameObject.AddComponent<Move>();
    }
}
