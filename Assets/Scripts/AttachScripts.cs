using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class AttachScripts : MonoBehaviour
    {
        private void Awake()
        {
            //gameObject.AddComponent<Singleton>();
            //gameObject.AddComponent<Test>();
            //gameObject.AddComponent<TestMove>();

            gameObject.AddComponent<ActorBoard>();
            gameObject.AddComponent<BlueprintFungus>();
            gameObject.AddComponent<BlueprintPool>();
            gameObject.AddComponent<BlueprintSponge>();

            gameObject.AddComponent<ConvertCoordinates>();
            gameObject.AddComponent<CreateWorld>();
            gameObject.AddComponent<Direction>();
            gameObject.AddComponent<DungeonBlueprint>();
            gameObject.AddComponent<DungeonBoard>();

            gameObject.AddComponent<FindObjects>();
            gameObject.AddComponent<GameColor>();
            gameObject.AddComponent<Initialize>();
            gameObject.AddComponent<ObjectData>();
            gameObject.AddComponent<ObjectPool>();

            gameObject.AddComponent<RandomNumber>();
            gameObject.AddComponent<SaveLoad>();
            gameObject.AddComponent<SchedulingSystem>();
            gameObject.AddComponent<UIMessage>();
            gameObject.AddComponent<UIModeline>();
            gameObject.AddComponent<UserInterface>();
            gameObject.AddComponent<WizardMode>();
        }
    }
}
