using Fungus.Actor;
using Fungus.Actor.FOV;
using Fungus.Actor.InputManager;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem
{
    public class TestMove : MonoBehaviour
    {
        //private static bool Instance;

        private int countStep;

        private GameObject[] mainUI;
        private Text message;
        private Vector3 moveHere;
        private Command newDirection;
        private WaitForSeconds wait5Seconds;

        public void MoveAround(GameObject actor)
        {
            newDirection = actor.GetComponent<PlayerInput>().GameCommand();

            if (!IsPassable(newDirection, actor.transform))
            {
                Debug.Log("You are blocked.");
                message.text = "You are blocked";
                FindObjects.GameLogic.GetComponent<UIModeline>()
                    .PrintStaticText(message.text);
                return;
            }

            switch (newDirection)
            {
                case Command.Left:
                    moveHere
                        = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                        .Convert(-1, 0);
                    break;

                case Command.Right:
                    moveHere
                      = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                      .Convert(1, 0);
                    break;

                case Command.Down:
                    moveHere
                      = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                      .Convert(0, -1);
                    break;

                case Command.Up:
                    moveHere
                      = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                      .Convert(0, 1);
                    break;

                case Command.UpLeft:
                    moveHere
                      = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                      .Convert(-1, 1);
                    break;

                case Command.UpRight:
                    moveHere
                      = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                      .Convert(1, 1);
                    break;

                case Command.DownLeft:
                    moveHere
                      = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                      .Convert(-1, -1);
                    break;

                case Command.DownRight:
                    moveHere
                      = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                      .Convert(1, -1);
                    break;
            }

            if (newDirection == Command.EndTurn)
            {
                FindObjects.GameLogic.GetComponent<SchedulingSystem>().NextActor();
            }
            else if (newDirection == Command.Initialize)
            {
                FindObjects.GameLogic.GetComponent<Initialize>().InitializeGame();
            }
            //else if (newDirection == Command.RenderAll)
            //{
            //    FindObjects.GameLogic.GetComponent<Test>().RenderAll
            //        = !FindObjects.GameLogic.GetComponent<Test>().RenderAll;
            //}
            else if (newDirection == Command.PrintEnergy)
            {
                FindObjects.GameLogic.GetComponent<SchedulingSystem>().CurrentActor
                    .GetComponent<Energy>().PrintEnergy();
                //pc.GetComponent<Energy>().PrintEnergy();
            }
            else if (newDirection != Command.INVALID)
            //if (!string.IsNullOrEmpty(newDirection))
            {
                message.text =
                    FindObjects.GameLogic.GetComponent<RandomNumber>().RootSeed +
                    "\n2\n3\n4\n5\n6";

                actor.transform.position += moveHere;
                countStep++;

                actor.GetComponent<FieldOfView>()?.UpdateFOV();
            }

            if (newDirection != Command.INVALID)
            {
                FindObjects.GameLogic.GetComponent<UIModeline>().PrintText();
            }
        }

        private bool IsPassable(Command direction, Transform actor)
        {
            int x = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                .Convert(actor.position.x);
            int y = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                .Convert(actor.position.y);

            switch (direction)
            {
                case Command.Left:
                    x -= 1;
                    break;

                case Command.Right:
                    x += 1;
                    break;

                case Command.Up:
                    y += 1;
                    break;

                case Command.Down:
                    y -= 1;
                    break;

                case Command.UpLeft:
                    x -= 1;
                    y += 1;
                    break;

                case Command.UpRight:
                    x += 1;
                    y += 1;
                    break;

                case Command.DownLeft:
                    x -= 1;
                    y -= 1;
                    break;

                case Command.DownRight:
                    x += 1;
                    y -= 1;
                    break;
            }

            return FindObjects.GameLogic.GetComponent<DungeonBoard>()
                .CheckBlock(SubObjectTag.Floor, x, y)
                || FindObjects.GameLogic.GetComponent<DungeonBoard>()
                .CheckBlock(SubObjectTag.Pool, x, y);
        }

        private IEnumerator MoveAndWait()
        {
            while (true)
            {
                yield return wait5Seconds;

                if (countStep % 5 == 0)
                {
                    Debug.Log("This step: " + countStep);
                }
                else
                {
                    Debug.Log("Current step: " + countStep);
                }
            }
        }

        private void Start()
        {
            //if (Instance)
            //{
            //    Debug.Log("Move already exists.");
            //    return;
            //}

            //Instance = true;

            wait5Seconds = new WaitForSeconds(5.0f);
            mainUI = GameObject.FindGameObjectsWithTag("MainUI");

            for (int i = 0; i < mainUI.Length; i++)
            {
                if (mainUI[i].name == "Message")
                {
                    message = mainUI[i].GetComponent<Text>();
                    break;
                }
            }

            countStep = 0;

            StartCoroutine(MoveAndWait());
        }
    }
}
