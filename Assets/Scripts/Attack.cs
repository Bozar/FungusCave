using Fungus.GameSystem;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor
{
    public class Attack : MonoBehaviour
    {
        private ActorBoard actorBoard;
        private ConvertCoordinates coord;

        public void MeleeAttack(int x, int y)
        {
            if (!GetComponent<Energy>().HasEnoughEnergy()
                || !actorBoard.HasActor(x, y))
            {
                return;
            }

            int attackEnergy = GetComponent<Energy>().GetAttackEnergy(
                coord.Convert(transform.position), new int[2] { x, y });
            GetComponent<Energy>().LoseEnergy(attackEnergy);

            GameObject target = actorBoard.GetActor(x, y);
            target.GetComponent<ICombatMessage>().IsHit(gameObject);

            int drain = GetComponent<IEnergy>().Drain;
            if (drain > 0)
            {
                target.GetComponent<ICombatMessage>().IsExhausted();
            }

            target.GetComponent<HP>().LoseHP(
                GetComponent<IDamage>().CurrentDamage);

            if (target.GetComponent<HP>().CurrentHP < 1)
            {
                target.GetComponent<ICombatMessage>().IsKilled(gameObject);
                GetComponent<IDeath>().DefeatTarget(target);
            }
            else
            {
                target.GetComponent<Infection>().GainInfection(gameObject);
                target.GetComponent<Energy>().LoseEnergy(drain);
            }
        }

        private void Start()
        {
            actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        }
    }
}
