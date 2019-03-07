using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public enum ActorGroupTag
    {
        Fungus
    }

    public enum CombatRoleTag
    {
        Minion, Soldier
    }

    public class ActorGroupData : MonoBehaviour
    {
        private Dictionary<ActorGroupTag,
            Dictionary<CombatRoleTag, List<SubObjectTag>>> actorGroup;

        public List<SubObjectTag> GetMinion(ActorGroupTag agt)
        {
            Dictionary<CombatRoleTag, List<SubObjectTag>> crt;
            List<SubObjectTag> sot;

            if (actorGroup.TryGetValue(agt, out crt)
                && crt.TryGetValue(CombatRoleTag.Minion, out sot))
            {
                return sot;
            }
            throw new MemberAccessException();
        }

        public List<SubObjectTag> GetSoldier(ActorGroupTag agt)
        {
            Dictionary<CombatRoleTag, List<SubObjectTag>> crt;
            List<SubObjectTag> sot;

            if (actorGroup.TryGetValue(agt, out crt)
                && crt.TryGetValue(CombatRoleTag.Soldier, out sot))
            {
                return sot;
            }
            throw new MemberAccessException();
        }

        private void InitializeData()
        {
            // Fungus
            actorGroup[ActorGroupTag.Fungus][CombatRoleTag.Minion]
                = new List<SubObjectTag> { SubObjectTag.GreyOoze };

            actorGroup[ActorGroupTag.Fungus][CombatRoleTag.Soldier]
                = new List<SubObjectTag>
                {
                    SubObjectTag.Corpse,
                    SubObjectTag.BloodFly,
                    SubObjectTag.YellowOoze
                };
        }

        private void Start()
        {
            actorGroup = new Dictionary<ActorGroupTag,
                Dictionary<CombatRoleTag, List<SubObjectTag>>>();

            foreach (ActorGroupTag agt in Enum.GetValues(typeof(ActorGroupTag)))
            {
                actorGroup[agt]
                    = new Dictionary<CombatRoleTag, List<SubObjectTag>>();
            }

            InitializeData();
        }
    }
}
