using Fungus.Actor.ObjectManager;
using Fungus.Actor.WorldBuilding;
using Fungus.Render;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class AttachScripts : MonoBehaviour
    {
        private void Awake()
        {
            //gameObject.AddComponent<Singleton>();
            gameObject.AddComponent<SaveLoad>();
            gameObject.AddComponent<FindObjects>();
            gameObject.AddComponent<Initialize>();
            gameObject.AddComponent<ObjectPool>();

            gameObject.AddComponent<UserInterface>();
            gameObject.AddComponent<UIMessage>();
            gameObject.AddComponent<UIModeline>();

            gameObject.AddComponent<RandomNumber>();
            gameObject.AddComponent<ConvertCoordinates>();
            gameObject.AddComponent<Direction>();
            gameObject.AddComponent<SchedulingSystem>();
            gameObject.AddComponent<GameColor>();

            gameObject.AddComponent<DungeonBoard>();
            gameObject.AddComponent<DungeonBlueprint>();
            gameObject.AddComponent<BlueprintSponge>();
            gameObject.AddComponent<BlueprintPool>();
            gameObject.AddComponent<BlueprintFungus>();
            gameObject.AddComponent<CreateWorld>();

            gameObject.AddComponent<ActorBoard>();
            gameObject.AddComponent<ObjectData>();

            gameObject.AddComponent<WizardMode>();

            //gameObject.AddComponent<Test>();
            //gameObject.AddComponent<TestMove>();
        }
    }
}
