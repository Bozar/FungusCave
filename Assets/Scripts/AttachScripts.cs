using Fungus.GameSystem.Data;
using Fungus.GameSystem.Progress;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class AttachScripts : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.AddComponent<ActorBoard>();
            gameObject.AddComponent<ActorData>();
            gameObject.AddComponent<ActorGroup>();
            gameObject.AddComponent<ActorGroupData>();

            gameObject.AddComponent<BlueprintFungus>();
            gameObject.AddComponent<BlueprintPool>();
            gameObject.AddComponent<BlueprintSponge>();

            gameObject.AddComponent<CombatMessage>();
            gameObject.AddComponent<ConvertCoordinates>();
            gameObject.AddComponent<CreateWorld>();

            gameObject.AddComponent<DamageData>();
            gameObject.AddComponent<Direction>();

            gameObject.AddComponent<DungeonBlueprint>();
            gameObject.AddComponent<DungeonBoard>();
            gameObject.AddComponent<DungeonProgress>();
            gameObject.AddComponent<DungeonProgressData>();
            gameObject.AddComponent<DungeonTerrain>();

            gameObject.AddComponent<EnergyData>();
            gameObject.AddComponent<FindObjects>();

            gameObject.AddComponent<GameColor>();
            gameObject.AddComponent<GameData>();
            gameObject.AddComponent<GameSetting>();
            gameObject.AddComponent<GameText>();

            gameObject.AddComponent<HeaderAction>();
            gameObject.AddComponent<HeaderStatus>();
            gameObject.AddComponent<InfectionData>();
            gameObject.AddComponent<Initialize>();

            gameObject.AddComponent<NourishFungus>();
            gameObject.AddComponent<ObjectPool>();
            gameObject.AddComponent<PotionData>();
            gameObject.AddComponent<PowerData>();

            gameObject.AddComponent<RandomNumber>();
            gameObject.AddComponent<SaveLoadFile>();
            gameObject.AddComponent<SaveLoadGame>();

            gameObject.AddComponent<SchedulingSystem>();
            gameObject.AddComponent<SpawnBeetle>();
            gameObject.AddComponent<SubMode>();

            gameObject.AddComponent<UIBuyPower>();
            gameObject.AddComponent<UIExamine>();
            gameObject.AddComponent<UIHeader>();
            gameObject.AddComponent<UILog>();

            gameObject.AddComponent<UIMessage>();
            gameObject.AddComponent<UIModeline>();
            gameObject.AddComponent<UIOpening>();
            gameObject.AddComponent<UISetting>();
            gameObject.AddComponent<UIStatus>();

            gameObject.AddComponent<UserInterface>();
            gameObject.AddComponent<WizardMode>();
        }
    }
}
