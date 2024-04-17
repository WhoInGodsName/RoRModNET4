using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoRModNET4
{
    internal class RoRMod : MonoBehaviour
    {
        static CharacterBody _Body;
        public static CharacterMaster LocalPlayer = null;
        NetworkUser _NetworkUser;
        TeamManager _TeamManager;
        TeleporterInteraction _Teleporter;
        CharacterVars characterVars = new CharacterVars();

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


        public void OnGUI()
        {
            string _speedLabel = $"> Character Speed: {characterVars.baseSpeed.ToString()}<";
            string _infJumpLabel = $"> Inf Jump: {jumpCount}<";
            string _godModeLabel = $"Godmode: {godMode}";

            Render.Begin("Risk of Tears", 4f, 1f, 180f, 600f, 10f, 20f, 2f);
            //GUI.Box(new Rect(0f, 0f, 300f, 500f), godMode.ToString());
            if (Render.Button("Toggle Firerate")) { maxFireRate = true; }
            Render.Label(_godModeLabel);
            if (Render.Button("Toggle Godmode")) { godMode = !godMode; }
            Render.Label(_speedLabel);
            if (Render.Button("+ Speed")) { increaseSpeed = true;  }
            if (Render.Button("- Speed")) { decreaseSpeed = true; }
            Render.Label(_infJumpLabel);
            if (Render.Button("Toggle Inf Jump")) { jumpCount = !jumpCount; }
            Render.Label(noReloadLabel);
            if (Render.Button("Toggle No Cooldown")) { noSkillReload = !noSkillReload; }
            Render.Label("> Unlock All <");
            if (Render.Button("Unlock")) { UnlockAll(); }
            Render.Label("> Spawn Body <");
            if (Render.Button("LunarGolemBody")) { LocalPlayer.CallCmdRespawn("LunarGolemBody"); }
            if (Render.Button("MegaDroneBody")) { LocalPlayer.CallCmdRespawn("MegaDroneBody"); }
            if (Render.Button("GrandParentBody")) { LocalPlayer.CallCmdRespawn("GrandParentBody"); }
            if (Render.Button("GolemBody")) { LocalPlayer.CallCmdRespawn("GolemBody"); }
            if (Render.Button("Engineer")) { LocalPlayer.CallCmdRespawn("EngiBody"); }
            if (Render.Button("Huntress")) { LocalPlayer.CallCmdRespawn("HuntressBody"); }
            if (Render.Button("Captain")) { LocalPlayer.CallCmdRespawn("CaptainBody"); }
            if (Render.Button("Loader")) { LocalPlayer.CallCmdRespawn("LoaderBody"); }
            Render.Label("       > Lunar Coins <");
            if (Render.Button("inf Lunar coin"))
            {
                _NetworkUser.CallCmdSetNetLunarCoins(9999999u);
            }
            Render.Label("       > Money <");
            if (Render.Button("+10k Money")) { LocalPlayer.GiveMoney(10000); }
            if (Render.Button("Change Username"))
            {
                _NetworkUser.userName = "Phat coc <3";
            }
        }
        public void Start()
        {
            UpdateLocalPlayer();
            _NetworkUser = FindObjectOfType<NetworkUser>();
            _TeamManager = FindObjectOfType<TeamManager>();
            _Teleporter = FindObjectOfType<TeleporterInteraction>();
        }

        public void Update()
        {
            UpdateLocalPlayer();
            if (_Body)
            {
                characterVars.baseSpeed = _Body.baseMoveSpeed;
            }
            
            _NetworkUser = FindObjectOfType<NetworkUser>();
            _Body = LocalPlayer.GetBody();

            if (maxFireRate == true)
            {
                _Body.baseAttackSpeed = 50f;
            }
            if (godMode == true)
            {
                _Body.healthComponent.godMode = true;
                _Body.healthComponent.health = 999999999999f;
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

        public static void UpdateLocalPlayer()
        {
            if (RoRMod.LocalPlayer != null)
            {
                return;
            }
            LocalPlayer = LocalUserManager.GetFirstLocalUser().cachedMasterController.master;
            /*for(int i = 0; i < LocalUserManager.readOnlyLocalUsersList.Count; i++) 
            {
                if (LocalUserManager.readOnlyLocalUsersList[i].currentNetworkUser != null 
                    && LocalUserManager.readOnlyLocalUsersList[i].currentNetworkUser.isLocalPlayer 
                    && LocalUserManager.readOnlyLocalUsersList[i].cachedMasterController != null 
                    && LocalUserManager.readOnlyLocalUsersList[i].cachedMasterController.master != null)
                {
                    RoRMod.LocalPlayer = LocalUserManager.readOnlyLocalUsersList[i].cachedMasterController.master;
                    break;
                }
            }*/
        }
    }
}
