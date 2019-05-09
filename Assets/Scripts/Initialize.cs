using Fungus.GameSystem.Data;
using Fungus.GameSystem.Progress;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Fungus.GameSystem
{
    public interface IInitialize
    {
        void Initialize();
    }

    public class Initialize : MonoBehaviour
    {
        private readonly string node = "EnterExit";
        private string dungeon;
        private string game;

        public bool Initialized { get; private set; }

        public void SaveAndQuit()
        {
            GetComponent<SaveLoadGame>().SaveGame(
                new SaveEventArgs(new Stack<IDataTemplate>()));
            Application.Quit();
        }

        private void InitBlueprint()
        {
            GetComponent<BlueprintSponge>().DrawBlueprint();
            GetComponent<BlueprintPool>().DrawBlueprint();
            GetComponent<BlueprintFungus>().DrawBlueprint();
        }

        private void InitializeGame()
        {
            GetComponent<RandomNumber>().Initialize();

            InitBlueprint();
            InitWorld();
        }

        private void InitWorld()
        {
            GetComponent<CreateWorld>().Initialize();
            GetComponent<DungeonTerrain>().Initialize();

            bool show = (GetComponent<DungeonProgressData>().GetDungeonLevel()
                == DungeonLevel.DL1)
                ? GetComponent<GameSetting>().ShowOpening
                : false;
            GetComponent<SubMode>().SwitchModeOpening(show);

            PrintWelcomeMessage();
        }

        private void LoadDungeon()
        {
            IDataTemplate[] data = GetComponent<SaveLoadFile>().LoadBinary(
                GetComponent<SaveLoadGame>().DungeonFile);
            GetComponent<SaveLoadGame>().LoadDungeonLevel(
                new LoadEventArgs(data));

            InitBlueprint();
            InitWorld();
        }

        private void LoadGame()
        {
            IDataTemplate[] data = GetComponent<SaveLoadFile>().LoadBinary(
                GetComponent<SaveLoadGame>().GameFile);
            GetComponent<SaveLoadGame>().LoadGame(
                new LoadEventArgs(data));

            InitWorld();
        }

        private void PrintWelcomeMessage()
        {
            string welcome = GetComponent<GameText>().GetStringData(node,
                "Welcome");
            string level = GetComponent<DungeonProgressData>().GetDungeonLevel()
                .ToString();
            level = level.Replace("DL", "");
            welcome = welcome.Replace("%num%", level);
            string help = GetComponent<GameText>().GetStringData(node, "Help");

            GetComponent<CombatMessage>().StoreText(welcome);
            if (GetComponent<DungeonProgressData>().GetDungeonLevel()
                == DungeonLevel.DL1)
            {
                GetComponent<CombatMessage>().StoreText(help);
            }
        }

        private void Start()
        {
            dungeon = Path.Combine(
                GetComponent<SaveLoadFile>().BinaryDirectory,
                GetComponent<SaveLoadGame>().DungeonFile);
            game = Path.Combine(
                GetComponent<SaveLoadFile>().BinaryDirectory,
                GetComponent<SaveLoadGame>().GameFile);
        }

        private void Update()
        {
            if (!Initialized)
            {
                if (File.Exists(game))
                {
                    LoadGame();
                }
                else if (File.Exists(dungeon))
                {
                    LoadDungeon();
                }
                else
                {
                    InitializeGame();
                }
                Debug.Log(GetComponent<RandomNumber>().RootSeed);
                Initialized = true;
            }
        }
    }
}
