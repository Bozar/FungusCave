using Fungus.Actor.FOV;
using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor.Render
{
    public class RenderSprite : MonoBehaviour
    {
        private ConvertCoordinates coordinate;
        private Color32 defaultColor;
        private GameColor gameColor;
        private GameObject pc;
        private int[] position;
        private int x;
        private int y;

        public void ChangeColor(Color32 newColor)
        {
            GetComponent<SpriteRenderer>().color = newColor;
        }

        private void Awake()
        {
            defaultColor = GetComponent<SpriteRenderer>().color;
        }

        private void HideSprite()
        {
            ChangeColor(gameColor.PickColor(ColorName.Black));
        }

        private void LateUpdate()
        {
            if (FindObjects.GameLogic.GetComponent<WizardMode>().RenderAll)
            {
                ShowSprite();
                return;
            }

            position = coordinate.GetComponent<ConvertCoordinates>().Convert(
                transform.position);
            x = position[0];
            y = position[1];

            switch (pc.GetComponent<FieldOfView>().GetFOVStatus(x, y))
            {
                case FOVStatus.Unknown:
                    HideSprite();
                    break;

                case FOVStatus.Visited:
                    switch (GetComponent<ObjectMetaInfo>().MainTag)
                    {
                        case MainObjectTag.Building:
                            RememberSprite();
                            break;

                        case MainObjectTag.Actor:
                            HideSprite();
                            break;
                    }
                    break;

                case FOVStatus.Insight:
                    ShowSprite();
                    break;

                case FOVStatus.TEST:
                    ChangeColor(gameColor.PickColor(ColorName.TEST));
                    break;
            }
        }

        private void RememberSprite()
        {
            ChangeColor(gameColor.PickColor(ColorName.Grey));
        }

        private void ShowSprite()
        {
            ChangeColor(defaultColor);
        }

        private void Start()
        {
            gameColor = FindObjects.GameLogic.GetComponent<GameColor>();
            coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            pc = FindObjects.PC;
        }
    }
}
