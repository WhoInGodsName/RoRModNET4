using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoRModNET4
{
    internal class TeamManager : MonoBehaviour
    {
        private List<NetworkUser> networkUsers = new List<NetworkUser>();
        private CharacterMaster localPlayer;
        public TeamManager(CharacterMaster _localPlayer)
        {
            localPlayer = _localPlayer;
        }
        
        public void DisplayTeam()
        {   
            if(networkUsers.Count < 1)
            {
                return;
            }

            GUI.Box(new Rect(200, 1, (networkUsers.Count * 150), 100), "");

            for (int i = 0; i < networkUsers.Count; i++)
            {
                Rect buttonPos = new Rect((230 + (i * 150)), 20, 100, 20);
                Rect buttonPos2 = new Rect((230 + (i * 150)), 42, 100, 20);
                Rect buttonPos3 = new Rect((230 + (i * 150)), 64, 100, 20);
                Rect vecPos = new Rect((230 + (i * 150)), 1, 100, 20);
                GUI.Label(vecPos, networkUsers[i].userName);
                if (GUI.Button(buttonPos, "Sacrifice </3"))
                {
                    SacrificeUser(networkUsers[i], networkUsers[i].gameObject, networkUsers[i].gameObject);
                }
                if (GUI.Button(buttonPos2, "Teleport to"))
                {
                    Vector3 targetPos = networkUsers[i].GetCurrentBody().transform.position;
                    localPlayer.GetBody().transform.position = targetPos;
                }
                if (GUI.Button(buttonPos3, "Respawn"))
                {
                    networkUsers[i].master.RespawnExtraLife();
                }




            }
        }
        public void GetTeam()
        {
            this.networkUsers.Clear();
            if(NetworkUser.FindObjectsOfType(typeof(NetworkUser)) == null)
            {
                return;
            }
            foreach (NetworkUser netuser in NetworkUser.FindObjectsOfType(typeof(NetworkUser)))
            {
                /*if (netuser.masterController.master == LocalPlayer)
                {
                    continue;
                }*/
                this.networkUsers.Add(netuser);
            }
        }
        public void GetLocalPlayer(CharacterMaster _localPlayer)
        {
            this.localPlayer = _localPlayer;
        }
        private void SacrificeUser(NetworkUser netUser, GameObject killerOverride = null, GameObject inflictorOverride = null, DamageType damageType = DamageType.VoidDeath)
        {
            var healthComp = netUser.masterController.master.GetBody().healthComponent;
            if (healthComp.alive && !healthComp.godMode)
            {
                float combinedHealth = healthComp.combinedHealth;
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = healthComp.combinedHealth;
                damageInfo.position = netUser.transform.position;
                damageInfo.damageType = damageType;
                damageInfo.procCoefficient = 1f;
                if (killerOverride)
                {
                    damageInfo.attacker = killerOverride;
                }
                if (inflictorOverride)
                {
                    damageInfo.inflictor = inflictorOverride;
                }
                healthComp.Networkhealth = 0f;
                DamageReport damageReport = new DamageReport(damageInfo, healthComp, damageInfo.damage, combinedHealth);
                healthComp.Network_killingDamageType = (uint)damageInfo.damageType;
                IOnKilledServerReceiver[] components = netUser.GetComponents<IOnKilledServerReceiver>();
                for (int i = 0; i < components.Length; i++)
                {
                    components[i].OnKilledServer(damageReport);
                }
                GlobalEventManager.instance.OnCharacterDeath(damageReport);
            }
        }
    }
}
