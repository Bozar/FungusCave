using UnityEngine;

public class AttachScripts : MonoBehaviour
{
    private void Awake()
    {
        //gameObject.AddComponent<Singleton>();
        gameObject.AddComponent<FindObjects>();
        gameObject.AddComponent<UserInput>();
        gameObject.AddComponent<ConvertCoordinates>();
        gameObject.AddComponent<SchedulingSystem>();
        gameObject.AddComponent<DungeonBoard>();
        gameObject.AddComponent<TileOverlay>();
        gameObject.AddComponent<RandomNumber>();
        gameObject.AddComponent<SaveLoad>();
        gameObject.AddComponent<Initialize>();
        gameObject.AddComponent<DungeonBlueprint>();
        gameObject.AddComponent<DungeonObjects>();
        gameObject.AddComponent<BlueprintSponge>();
        gameObject.AddComponent<BlueprintPool>();
        gameObject.AddComponent<BlueprintFungus>();

        gameObject.AddComponent<Test>();
        gameObject.AddComponent<Move>();
    }
}
