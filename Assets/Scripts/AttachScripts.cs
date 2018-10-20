﻿using UnityEngine;

public class AttachScripts : MonoBehaviour
{
    private void Awake()
    {
        //gameObject.AddComponent<Singleton>();
        gameObject.AddComponent<PlayerInput>();
        gameObject.AddComponent<SaveLoad>();
        gameObject.AddComponent<FindObjects>();
        gameObject.AddComponent<UserInterface>();
        gameObject.AddComponent<Initialize>();

        gameObject.AddComponent<RandomNumber>();
        gameObject.AddComponent<ConvertCoordinates>();
        gameObject.AddComponent<SchedulingSystem>();
        gameObject.AddComponent<TileOverlay>();
        gameObject.AddComponent<Color>();

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
