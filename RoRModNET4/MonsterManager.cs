using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoRModNET4
{
    internal class MonsterManager : MonoBehaviour
    {
        private IReadOnlyCollection<TeamComponent> GetMonsters()
        {          
            var monsters = TeamComponent.GetTeamMembers(TeamIndex.Monster);

            return monsters;
        }

        public void KillMonsters(CharacterMaster localPlayer)
        {
            var monsters = this.GetMonsters();
            DamageInfo dmg = new DamageInfo()
            {
                damage = 9999999999,
                crit = true,
                attacker = localPlayer.gameObject,
                damageType = DamageType.Generic,

            };
            if(monsters.Count() > 0)
            {
                foreach (var monster in monsters)
                {
                    monster.body.healthComponent.TakeDamage(dmg);
                }
            }
            
        }
    }
}
