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
            gameObject.AddComponent<ActorData>();
            gameObject.AddComponent<ActorGroupData>();

            gameObject.AddComponent<BlueprintFungus>();
            gameObject.AddComponent<BlueprintPool>();
            gameObject.AddComponent<BlueprintSponge>();

            gameObject.AddComponent<CombatMessage>();
            gameObject.AddComponent<ConvertCoordinates>();
            gameObject.AddComponent<CreateWorld>();

            gameObject.AddComponent<DamageData>();
            gameObject.AddComponent<Direction>();

            gameObject.AddComponent<DungeonActor>();
            gameObject.AddComponent<DungeonBlueprint>();
            gameObject.AddComponent<DungeonBoard>();
            gameObject.AddComponent<DungeonTerrain>();

            gameObject.AddComponent<EnergyData>();
            gameObject.AddComponent<FindObjects>();

            gameObject.AddComponent<GameColor>();
            gameObject.AddComponent<GameSetting>();
            gameObject.AddComponent<GameText>();

            gameObject.AddComponent<HeaderAction>();
            gameObject.AddComponent<HeaderStatus>();
            gameObject.AddComponent<InfectionData>();
            gameObject.AddComponent<Initialize>();

            gameObject.AddComponent<ObjectPool>();
            gameObject.AddComponent<PotionData>();
            gameObject.AddComponent<PowerData>();
            gameObject.AddComponent<Progress>();
            gameObject.AddComponent<ProgressData>();

            gameObject.AddComponent<RandomNumber>();
            gameObject.AddComponent<SaveLoad>();
            gameObject.AddComponent<SchedulingSystem>();
            gameObject.AddComponent<SubMode>();

            gameObject.AddComponent<UIBuyPower>();
            gameObject.AddComponent<UIExamine>();
            gameObject.AddComponent<UIHeader>();
            gameObject.AddComponent<UILog>();

            gameObject.AddComponent<UIMessage>();
            gameObject.AddComponent<UIModeline>();
            gameObject.AddComponent<UIOpening>();
            gameObject.AddComponent<UIStatus>();
            gameObject.AddComponent<UserInterface>();

            gameObject.AddComponent<WizardMode>();
        }
    }
}
