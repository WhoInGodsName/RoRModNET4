﻿using HG;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static RoR2.SpawnCard;
using static UnityEngine.Rendering.DebugUI;

namespace RoRModNET4
{
    internal class RoRMod : MonoBehaviour
    {
        static CharacterBody _Body;
        public static CharacterMaster LocalPlayer = null;
        NetworkUser _NetworkUser;
        TeamManager _TeamManager = new TeamManager(LocalPlayer);
        TeleporterInteraction _Teleporter;
        CharacterVars characterVars = new CharacterVars();
        FriendlyFireManager.FriendlyFireMode friendlyFireMode;


        NetworkWriter _Writer;
        string nameToken = "";

        //Toggle menu
        bool menuToggle = false;

        //Toggles for menu
        bool maxFireRate = false;
        bool godMode = false;

        //Player Speed
        bool increaseSpeed = false;
        bool decreaseSpeed = false;

        //Player jump
        
        bool jumpCount = false;

        //No skill reload time  
        string noReloadLabel = "> No Cooldown Timer <";
        bool noSkillReload = false;

        //Friendly fire
        bool ff = false;

        bool spawnOnTeam = false;
        bool debugger = false;
        bool getVals = false;


        public void OnGUI()
        {
            string _speedLabel = $"Character Speed: {characterVars.baseSpeed.ToString()}";
            string _infJumpLabel = $"Inf Jump: {jumpCount}";
            string _godModeLabel = $"Godmode: {godMode}";


            
            
            if(menuToggle == true)
            {
                _TeamManager.DisplayTeam();

                Render.Begin("Risk of Tears 1.0.3", 4f, 1f, 180f, 790f, 10f, 20f, 2f);
                if (debugger)
                {
                    GUI.Box(new Rect(10f, 1000f, 600f, 600f), "");
                    GUI.Label(new Rect(10f, 1000f, 600f, 600f), "DEBUGGER");
                    GUI.Label(new Rect(20f, 1010f, 600f, 600f), nameToken);
                }

                if (Render.Button("Toggle Firerate")) { maxFireRate = !maxFireRate; }
                Render.Label(_godModeLabel);
                if (Render.Button("Toggle Godmode")) { godMode = !godMode; }
                Render.Label(_speedLabel);
                if (Render.Button("+ Speed")) { increaseSpeed = true; }
                if (Render.Button("- Speed")) { decreaseSpeed = true; }
                Render.Label(_infJumpLabel);
                if (Render.Button("Toggle Inf Jump")) { jumpCount = !jumpCount; }
                Render.Label(noReloadLabel);
                if (Render.Button("Toggle No Cooldown")) { noSkillReload = !noSkillReload; }
                Render.Label("Unlock All");
                if (Render.Button("Unlock")) { UnlockAll(); }
                Render.Label("Spawn Body");
                if (Render.Button("SuperRoboBallBossBody")) { LocalPlayer.CallCmdRespawn("SuperRoboBallBossBody"); }
                if (Render.Button("MegaDroneBody")) { LocalPlayer.CallCmdRespawn("MegaDroneBody"); }
                if (Render.Button("EnforcerBody")) { LocalPlayer.CallCmdRespawn("EnforcerBody"); }
                if (Render.Button("GolemBody")) { LocalPlayer.CallCmdRespawn("GolemBody"); }
                if (Render.Button("ElectricWormBody")) { LocalPlayer.CallCmdRespawn("ElectricWormBody"); }
                if (Render.Button("HereticBody")) { LocalPlayer.CallCmdRespawn("HereticBody"); }
                if (Render.Button("Huntress")) { LocalPlayer.CallCmdRespawn("HuntressBody"); }
                if (Render.Button("GravekeeperBody")) { LocalPlayer.CallCmdRespawn("GravekeeperBody"); }
                if (Render.Button("BrotherBody")) { LocalPlayer.CallCmdRespawn("BrotherBody"); }
                if (Render.Button("Loader")) { LocalPlayer.CallCmdRespawn("LoaderBody"); }

                Render.Label(">Coins / Exp / Misc<");
                if (Render.Button("+1000 Lunar coins"))
                {
                    foreach (NetworkUser netuser in GetAllNetworkPlayers())
                    {
                        if (netuser.master == LocalPlayer)
                        {
                            netuser.CallRpcAwardLunarCoins(1000);
                        }
                    }
                }
                if (Render.Button("+10k Money")) { LocalPlayer.GiveMoney(10000); }
                if (Render.Button("Pickup Message")) { Broadcastpickup(1); }
                if (Render.Button("Spawn Prefab"))
                {
                    PrefabDraw draw = new PrefabDraw();
                    draw.Draw(_Body.GetComponent<Transform>().position, _Body.GetComponent<Transform>().rotation, "JellyfishBody");
                }
                if (Render.Button("Debugger")) { debugger = !debugger; getVals = true; }
                Render.Label("> Team <");
                if (Render.Button("Sacrifice team </3"))
                {
                    SacrificeTeam();
                }
                if (Render.Button("Spawn prefab on team"))
                {
                    spawnOnTeam = !spawnOnTeam;
                }
                if (Render.Button("Deduct Lunar coins"))
                {
                    NoLunarCoins();
                }
                Render.Label("> menu <");
                if (Render.Button("Toggle Menu")) { menuToggle = !menuToggle; }
            }
            else
            {
                Render.Begin("Risk of Tears 1.0.3", 4f, 1f, 180f, 50f, 10f, 20f, 2f);
                
                if (Render.Button("Toggle Menu")) { menuToggle = !menuToggle; }
            }

            

        }
        public void Start()
        {
            UpdateLocalPlayer();
            _NetworkUser = FindObjectOfType<NetworkUser>();
            _Teleporter = FindObjectOfType<TeleporterInteraction>();
        }

        public void Update()
        {
            UpdateLocalPlayer();
            ExpansionInfo();
            _TeamManager.GetLocalPlayer(LocalPlayer);
            _TeamManager.GetTeam();
            

            _Body = LocalPlayer.GetBody();

            if (_Body)
            {
                characterVars.baseSpeed = _Body.baseMoveSpeed;

                if (maxFireRate == true)
                {
                    _Body.baseAttackSpeed = 200f;
                }
                else
                {
                    _Body.baseAttackSpeed = 1f;
                }
                if (godMode == true)
                {
                    _Body.healthComponent.godMode = true;
                    _Body.healthComponent.health = 9999f;
                }
                else if (godMode == false)
                {
                    _Body.healthComponent.godMode = false;
                }

                //Character speed toggle up and down.
                if (increaseSpeed == true)
                {
                    increaseSpeed = !increaseSpeed;
                    _Body.baseMoveSpeed += 1f;
                }
                if (decreaseSpeed == true)
                {
                    decreaseSpeed = !decreaseSpeed;
                    _Body.baseMoveSpeed -= 1f;
                }

                //Infinite jump toggle.
                if (jumpCount == true)
                {
                    _Body.baseJumpCount = 99999;
                }
                else if (jumpCount == false)
                {
                    _Body.baseJumpCount = 1;
                }

                if (noSkillReload == true)
                {
                    _Body.skillLocator.primary.rechargeStopwatch = 0f;
                    _Body.skillLocator.primary.stock = 999;
                    _Body.skillLocator.secondary.rechargeStopwatch = 0f;
                    _Body.skillLocator.secondary.stock = 999;
                    _Body.skillLocator.utility.rechargeStopwatch = 0f;
                    _Body.skillLocator.utility.stock = 999;
                    _Body.skillLocator.special.rechargeStopwatch = 0f;
                    _Body.skillLocator.special.stock = 999;
                }
                if(spawnOnTeam == true)
                {
                    SpawnOnTeam("ExplosivePotDestructibleBody");
                }
                if(ff == true)
                {
                    friendlyFireMode = FriendlyFireManager.FriendlyFireMode.FriendlyFire;
                }
                else
                {
                    friendlyFireMode = FriendlyFireManager.FriendlyFireMode.Off;
                }

            }
            
            _NetworkUser = FindObjectOfType<NetworkUser>();
        }

        public void UnlockAll()
        {
            UserProfile userProfile = LocalUserManager.GetFirstLocalUser().userProfile;

            foreach (ItemIndex item in ItemCatalog.allItems)
            {
                userProfile.DiscoverPickup(PickupCatalog.FindPickupIndex(item));
            }
            foreach (EquipmentIndex equipmentIndex in EquipmentCatalog.allEquipment)
            {
                userProfile.DiscoverPickup(PickupCatalog.FindPickupIndex(equipmentIndex));
            }

            UserAchievementManager UAM = AchievementManager.GetUserAchievementManager(RoR2.LocalUserManager.GetFirstLocalUser());
            foreach (AchievementDef AD in AchievementManager.allAchievementDefs)
            {
                UAM.GrantAchievement(AD);
            }
        }

        public NetworkUser[] GetAllNetworkPlayers()
        {
            int count = 0;
            NetworkUser[] netUsers = new NetworkUser[10];
            foreach(NetworkUser netuser in NetworkUser.FindObjectsOfType(typeof(NetworkUser)))
            {
                netUsers[count] = netuser;
                count += 1;
            }

            return netUsers;
        }

        public void SacrificeTeam()
        {
            foreach (NetworkUser netuser in NetworkUser.FindObjectsOfType(typeof(NetworkUser)))
            {
                if (netuser)
                {
                    if(netuser.masterController.master == LocalPlayer)
                    {
                        continue;
                    }
                    TeamSac(netuser, netuser.gameObject, netuser.gameObject);
                }
            }
        }

        public void Broadcastpickup(uint amount)
        {

            var random = new System.Random();

            string text = characterVars.pickupLines[random.Next(characterVars.pickupLines.Count())];
            Color color = characterVars.colours[random.Next(characterVars.colours.Count())];

            foreach (NetworkUser netuser in GetAllNetworkPlayers())
            {
                if (netuser.master == LocalPlayer)
                {
                    Chat.SendBroadcastChat(new Chat.PlayerPickupChatMessage
                    {
                        pickupColor = color,
                        pickupQuantity = amount,
                        pickupToken = text,
                        baseToken = "PLAYER_PICKUP",
                        subjectAsCharacterBody = netuser.master.GetBody(),
                        subjectAsNetworkUser = netuser

                    });
                }
            }
        }

        public void TeamSac(NetworkUser netUser, GameObject killerOverride = null, GameObject inflictorOverride = null, DamageType damageType = DamageType.VoidDeath)
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

        public void SpawnOnTeam(string prefabString)
        {
            GameObject toSpawn = new GameObject();
            toSpawn = BodyCatalog.FindBodyPrefab(prefabString);
            foreach (NetworkUser netuser in NetworkUser.FindObjectsOfType(typeof(NetworkUser)))
            {
                if (netuser.masterController.master == LocalPlayer)
                {
                    continue;
                }
                Vector3 bodyTrans = Vector3.zero;
                bodyTrans = netuser.GetCurrentBody().GetComponent<Transform>().position;
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(toSpawn, bodyTrans, netuser.GetComponent<Transform>().rotation);
                //gameObject.GetComponent<MeshRenderer>().enabled = false;
                NetworkServer.Spawn(gameObject);
                

            }
        }

        public void ExpansionInfo()
        {
            ExpansionDef[] x = ContentManager.expansionDefs;
            if (getVals == true)
            {
                nameToken = "";
                foreach (ReadOnlyContentPack ROCP in ContentManager.allLoadedContentPacks)
                {
                    nameToken += $"\nROCP identifier: {ROCP.identifier}\n" +
                        $"ROCP hashcode: {ROCP.GetHashCode()}\n" +
                        $"ROCP is valid: {ROCP.isValid}\n";
                }
                getVals = false;
            }
        }
        
        public void NoLunarCoins()
        {
            foreach (NetworkUser netuser in NetworkUser.FindObjectsOfType(typeof(NetworkUser)))
            {
                if (netuser.masterController.master == LocalPlayer)
                {
                    continue;
                }

                netuser.CallRpcDeductLunarCoins(10);
                
            }
        }

        public static void UpdateLocalPlayer()
        {
            if (RoRMod.LocalPlayer != null)
            {
                return;
            }
            LocalPlayer = LocalUserManager.GetFirstLocalUser().cachedMasterController.master;
        }

    }
}
