using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public class SpawnBeetle : MonoBehaviour, ISaveLoadBinary
    {
        private readonly string dataNode = "SpawnBeetle";
        private readonly string textNode = "SpawnBeetle";
        private int count;
        private int distance;
        private int maxBeetle;
        private int maxCount;
        private bool notWarned;
        private int warning;

        public void BeetleEmerge()
        {
            if ((count <= warning) && notWarned)
            {
                StoreMessage("Warning");

                notWarned = false;
            }
            else if (count < 1)
            {
                StoreMessage("Emerge");
                CreateBeetle();

                count = maxCount;
                notWarned = true;
            }
        }

        public void LoadBinary(IDataTemplate[] dt)
        {
            foreach (IDataTemplate d in dt)
            {
                if (d.DTTag == DataTemplateTag.Spawn)
                {
                    DTSpawnBeetle value = d as DTSpawnBeetle;
                    count = value.Count;
                    notWarned = value.NotWarned;
                    return;
                }
            }
        }

        public void SaveBinary(Stack<IDataTemplate> dt)
        {
            DTSpawnBeetle data = new DTSpawnBeetle
            {
                Count = count,
                NotWarned = notWarned
            };
            dt.Push(data);
        }

        private void CreateBeetle()
        {
            int[][] position = GetPosition();

            int actor = GetComponent<SchedulingSystem>().CountActor;
            int maxActor = GetComponent<GameData>().GetIntData("Dungeon",
                "MaxActor");

            if ((position.Length < maxBeetle) || (actor > maxActor))
            {
                return;
            }
            position = FilterPosition(position);

            DungeonLevel dl = GetComponent<DungeonProgressData>()
                .GetDungeonLevel();
            SubObjectTag minion = GetComponent<ActorGroupData>()
                .GetActorGroup(dl)[CombatRoleTag.Minion][0];

            foreach (int[] p in position)
            {
                GetComponent<ObjectPool>().CreateObject(
                    MainObjectTag.Actor,
                    minion,
                    p);
            }
        }

        private int[][] FilterPosition(int[][] position)
        {
            SeedTag seed = GetComponent<DungeonProgressData>().GetDungeonSeed();
            int[][] randomized = position.OrderBy(
                p => GetComponent<RandomNumber>().Next(seed))
                .ToArray();
            Stack<int[]> result = new Stack<int[]>();

            for (int i = 0; i < maxBeetle; i++)
            {
                result.Push(randomized[i]);
            }
            return result.ToArray();
        }

        private int[][] GetPosition()
        {
            int[] pcPosition = GetComponent<ConvertCoordinates>().Convert(
                FindObjects.PC.transform.position);
            Stack<int[]> position = new Stack<int[]>();

            for (int i = pcPosition[0] - distance;
                i < pcPosition[0] + distance;
                i++)
            {
                for (int j = pcPosition[1] - distance;
                    j < pcPosition[1] + distance;
                    j++)
                {
                    if (GetComponent<DungeonTerrain>().IsPassable(i, j))
                    {
                        position.Push(new int[] { i, j });
                    }
                }
            }
            return position.ToArray();
        }

        private bool HasFungus(int[] position)
        {
            List<int[]> neighbor = GetComponent<ConvertCoordinates>()
                 .SurroundCoord(Surround.Diagonal, position);
            neighbor = GetComponent<DungeonBoard>().FilterPositions(neighbor);

            foreach (int[] n in neighbor)
            {
                if (GetComponent<DungeonBoard>().CheckBlock(
                    SubObjectTag.Fungus, n))
                {
                    return true;
                }
            }
            return false;
        }

        private void NourishFungus_SpawnCountDown(object sender,
            ActorInfoEventArgs e)
        {
            count--;

            if (HasFungus(e.Position))
            {
                count--;
            }
        }

        private void SpawnBeetle_LoadingGame(object sender, LoadEventArgs e)
        {
            LoadBinary(e.GameData);
        }

        private void SpawnBeetle_SavingGame(object sender, SaveEventArgs e)
        {
            SaveBinary(e.GameData);
        }

        private void Start()
        {
            GetComponent<NourishFungus>().SpawnCountDown
                += NourishFungus_SpawnCountDown;
            GetComponent<SaveLoadGame>().SavingGame += SpawnBeetle_SavingGame;
            GetComponent<SaveLoadGame>().LoadingGame += SpawnBeetle_LoadingGame;

            maxCount = GetComponent<GameData>().GetIntData(dataNode, "MaxCount");
            distance = GetComponent<GameData>().GetIntData(dataNode, "Distance");
            maxBeetle = GetComponent<GameData>().GetIntData(dataNode, "Beetle");
            warning = GetComponent<GameSetting>().BeetleWarning;

            count = maxCount;
            notWarned = true;
        }

        private void StoreMessage(string node)
        {
            string text = GetComponent<GameText>().GetStringData(textNode,
                node);
            text = GetComponent<GameColor>().GetColorfulText(text,
                ColorName.Orange);
            GetComponent<CombatMessage>().StoreText(text);
        }
    }
}
