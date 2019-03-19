using Fungus.Actor.FOV;
using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor.Render
{
    public class RenderSprite : MonoBehaviour
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private Color32 defaultColor;
        private GameColor gameColor;
        private StaticActor getActor;

        private delegate GameObject StaticActor(SubObjectTag tag);

        public void ChangeDefaultColor(ColorName color)
        {
            ChangeColor(color);
            defaultColor = gameColor.PickColor(color);
        }

        private void Awake()
        {
            defaultColor = GetComponent<SpriteRenderer>().color;
        }

        private void ChangeColor(ColorName color)
        {
            GetComponent<SpriteRenderer>().color = gameColor.PickColor(color);
        }

        private void ChangeColor(Color32 newColor)
        {
            GetComponent<SpriteRenderer>().color = newColor;
        }

        private void HideSprite()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        private void LateUpdate()
        {
            if (FindObjects.GameLogic.GetComponent<WizardMode>().RenderAll)
            {
                ShowSprite();
                return;
            }

            int[] goPos = coord.Convert(transform.position);
            int[] exPos = coord.Convert(getActor(SubObjectTag.Examiner)
                .transform.position);
            int x = goPos[0];
            int y = goPos[1];

            if (getActor(SubObjectTag.Examiner).activeSelf
                && (x == exPos[0]) && (y == exPos[1]))
            {
                HideSprite();
                return;
            }

            switch (FindObjects.PC.GetComponent<FieldOfView>().GetFOVStatus(x, y))
            {
                case FOVStatus.Unknown:
                    HideSprite();
                    return;

                case FOVStatus.Visited:
                    switch (GetComponent<MetaInfo>().MainTag)
                    {
                        case MainObjectTag.Building:
                            RememberSprite();
                            return;

                        case MainObjectTag.Actor:
                            HideSprite();
                            return;

                        default:
                            return;
                    }

                case FOVStatus.Insight:
                    switch (GetComponent<MetaInfo>().MainTag)
                    {
                        case MainObjectTag.Building:
                            if (actor.HasActor(x, y))
                            {
                                HideSprite();
                            }
                            else
                            {
                                ShowSprite();
                            }
                            return;

                        case MainObjectTag.Actor:
                            ShowSprite();
                            return;

                        default:
                            return;
                    }

                case FOVStatus.TEST:
                    ChangeColor(ColorName.TEST);
                    return;
            }
        }

        private void RememberSprite()
        {
            GetComponent<SpriteRenderer>().enabled = true;
            ChangeColor(ColorName.Grey);
        }

        private void ShowSprite()
        {
            GetComponent<SpriteRenderer>().enabled = true;
            ChangeColor(defaultColor);
        }

        private void Start()
        {
            gameColor = FindObjects.GameLogic.GetComponent<GameColor>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();

            getActor = FindObjects.GetStaticActor;
        }
    }
}
