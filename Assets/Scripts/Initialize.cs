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

            bool show = GetComponent<GameSetting>().ShowOpening;
            GetComponent<SubMode>().SwitchModeOpening(show);

            PrintMessageNewGame();
        }

        private void InitWorld()
        {
            GetComponent<CreateWorld>().Initialize();
            GetComponent<DungeonTerrain>().Initialize();
        }

        private void LoadDungeon()
        {
            IDataTemplate[] data = GetComponent<SaveLoadFile>().LoadBinary(
                GetComponent<SaveLoadGame>().DungeonFile);
            GetComponent<SaveLoadGame>().LoadDungeonLevel(
                new LoadEventArgs(data));

            InitBlueprint();
            InitWorld();
            PrintMessageNewGame();
        }

        private void LoadGame()
        {
            IDataTemplate[] data = GetComponent<SaveLoadFile>().LoadBinary(
                GetComponent<SaveLoadGame>().GameFile);
            GetComponent<SaveLoadGame>().LoadGame(
                new LoadEventArgs(data));

            InitWorld();
            PrintMessageLoadGame();
        }

        private void PrintMessage(string welcome)
        {
            string level = GetComponent<DungeonProgressData>().GetDungeonLevel()
                .ToString();
            level = level.Replace("DL", "");
            welcome = welcome.Replace("%num%", level);

            GetComponent<CombatMessage>().StoreText(welcome);
        }

        private void PrintMessageLoadGame()
        {
            string welcome = GetComponent<GameText>().GetStringData(node,
               "LoadGame");
            PrintMessage(welcome);
        }

        private void PrintMessageNewGame()
        {
            string welcome = GetComponent<GameText>().GetStringData(node,
                "Welcome");
            PrintMessage(welcome);

            string help = GetComponent<GameText>().GetStringData(node, "Help");
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
