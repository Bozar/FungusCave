using UnityEngine;

public class AttachScripts : MonoBehaviour
{
    private void Awake()
    {
        //gameObject.AddComponent<Singleton>();
        gameObject.AddComponent<PlayerInput>();
        gameObject.AddComponent<SaveLoad>();
        gameObject.AddComponent<FindObjects>();
        gameObject.AddComponent<Initialize>();

        gameObject.AddComponent<UserInterface>();
        gameObject.AddComponent<UIMessage>();

        gameObject.AddComponent<RandomNumber>();
        gameObject.AddComponent<ConvertCoordinates>();
        gameObject.AddComponent<SchedulingSystem>();
        gameObject.AddComponent<GameColor>();

        gameObject.AddComponent<DungeonBoard>();
        gameObject.AddComponent<DungeonBlueprint>();
        gameObject.AddComponent<BlueprintSponge>();
        gameObject.AddComponent<BlueprintPool>();
        gameObject.AddComponent<BlueprintFungus>();
        gameObject.AddComponent<DungeonObjects>();

        gameObject.AddComponent<Test>();
        gameObject.AddComponent<Move>();
    }
}
