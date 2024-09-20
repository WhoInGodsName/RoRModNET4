using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace RoRModNET4
{
    internal class TeamManager : MonoBehaviour
    {
        private List<NetworkUser> networkUsers = new List<NetworkUser>();
        private CharacterMaster localPlayer;
        private NetworkUser localNetUser;
        //private CharacterVars characterVars;

        Vector2 scrollPosition2 = Vector2.zero;
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

            GUI.Box(new Rect(200, 1, (networkUsers.Count * 150), 130), "");

            for (int i = 0; i < networkUsers.Count; i++)
            {
                Rect buttonPos = new Rect((230 + (i * 150)), 20, 100, 20);
                Rect buttonPos2 = new Rect((230 + (i * 150)), 42, 100, 20);
                Rect buttonPos3 = new Rect((230 + (i * 150)), 64, 100, 20);
                Rect buttonPos4 = new Rect((230 + (i * 150)), 86, 100, 20);
                Rect buttonPos5 = new Rect((230 + (i * 150)), 108, 100, 20);
                Rect vecPos = new Rect((230 + (i * 150)), 1, 100, 20);
                GUI.Label(vecPos, networkUsers[i].userName);
                if (GUI.Button(buttonPos, "Sacrifice </3"))
                {
                    SacrificeUser(networkUsers[i], networkUsers[i].gameObject, networkUsers[i].gameObject);
                }
                
                if (GUI.Button(buttonPos2, "Respawn"))
                {
                    networkUsers[i].master.RespawnExtraLife();
                }
                if (GUI.Button(buttonPos3, "Kick"))
                {
                    //networkUsers[i].CallCmdSendConsoleCommand("parent_volume_music", new string[] {  "100" });
                    this.localNetUser.CallCmdSendConsoleCommand("kick_steam", new string[] { (networkUsers[i].Network_id.steamId).ToString() });
                }
                if (GUI.Button(buttonPos4, "Turret Time"))
                {
                    networkUsers[i].master.CallCmdRespawn("MissileDroneBody");
                   
                }
                if (GUI.Button(buttonPos5, "Clear inv"))
                {
                    networkUsers[i].master.inventory.CleanInventory();

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
                this.networkUsers.Add(netuser);
                if(netuser.master == localPlayer)
                {
                    this.localNetUser = netuser;
                }
            }
        }
        public List<NetworkUser> ReturnTeam()
        {
            return this.networkUsers;
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

        private void TeamTurretGo()
        {
            for (int i = 0; i < this.networkUsers.Count(); i++)
            {
                if (networkUsers[i].masterController.master == this.localPlayer)
                {
                    continue;
                }
                this.networkUsers[i].master.CallCmdRespawn("EngiTurretBody");
            }
        }
    }
}
