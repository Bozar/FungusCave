using Fungus.Actor.FOV;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class PCAutoExplore : MonoBehaviour, IAutoExplore, ITurnCounter
    {
        private DungeonBoard board;
        private int countAutoExplore;
        private UIModeline modeline;
        private GameSetting setting;

        public bool ContinueAutoExplore
        {
            get
            {
                bool count = countAutoExplore > 0;

                if (count && !FindEnemy() && FindUnknownGrid())
                {
                    return true;
                }
                countAutoExplore = 0;
                return false;
            }
        }

        public void Count()
        {
            if (countAutoExplore > 0)
            {
                countAutoExplore--;
            }
        }

        public SeedTag GetSeedTag()
        {
            return SeedTag.AutoExplore;
        }

        public bool IsStartPoint(int x, int y)
        {
            return GetComponent<FieldOfView>().CheckFOV(FOVStatus.Unknown,
                new int[] { x, y });
        }

        public void Trigger()
        {
            countAutoExplore = setting.AutoExploreStep;
        }

        private bool FindEnemy()
        {
            if (GetComponent<AIVision>().CanSeeTarget(MainObjectTag.Actor))
            {
                modeline.PrintStaticText("You see an enemy.");
                return true;
            }
            return false;
        }

        private bool FindUnknownGrid()
        {
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    if (GetComponent<FieldOfView>().CheckFOV(FOVStatus.Unknown,
                        i, j))
                    {
                        return true;
                    }
                }
            }
            modeline.PrintStaticText("You have explored everywhere.");
            return false;
        }

        private void Start()
        {
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            setting = FindObjects.GameLogic.GetComponent<GameSetting>();
        }
    }
}
