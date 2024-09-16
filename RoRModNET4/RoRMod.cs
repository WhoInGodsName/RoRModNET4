
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System.Threading;
using System.Linq;

using UnityEngine;

using UnityEngine.Networking;

using static RoR2.SpawnCard;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;

namespace RoRModNET4
{
    internal class RoRMod : MonoBehaviour
    {
        static CharacterBody _Body;
        public static CharacterMaster LocalPlayer = null;
        TeamManager _TeamManager = new TeamManager(LocalPlayer);
        CharacterVars characterVars = new CharacterVars();
        FriendlyFireManager.FriendlyFireMode friendlyFireMode;


        NetworkWriter _Writer;
        string nameToken = "";

        //Toggle menu
        bool menuToggle = false;
        bool teamMenu = false;
        bool bodyMenu = false;
        bool itemMenu = false;

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
        bool getVals = false;

        //ESP
        bool itemESP = false;
        //bool portalESP = false;

        Vector2 scrollPosition = Vector2.zero;
        Vector2 scrollPosition2 = Vector2.zero;

        //Item menu vars
        CharacterMaster chosenUser;
        bool loadUsers = false;
        int itemMenuColourInt = 0;
        Color itemMenuColour;

        int renderCount = 2000;
        List<PurchaseInteraction> itemList = new List<PurchaseInteraction>();

        public void OnGUI()
        {
            string _speedLabel = $"Character Speed: {characterVars.baseSpeed}";
            string _infJumpLabel = $"Inf Jump: {jumpCount}";
            string _godModeLabel = $"Godmode: {godMode}";


            if (menuToggle == true)
            {
                if(teamMenu == true)
                {
                    _TeamManager.DisplayTeam();
                }

                Render.Begin("Risk of Tears 1.1.4", 4f, 1f, 180f, 600f, 10f, 20f, 2f);
                /*if (debugger)
                {
                    GUI.Box(new Rect(10f, 1000f, 600f, 600f), "");
                    GUI.Label(new Rect(10f, 1000f, 600f, 600f), "DEBUGGER");
                    GUI.Label(new Rect(20f, 1010f, 600f, 600f), nameToken);
                }*/

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
                

                Render.Label(">Coins / Exp / Misc<");
                if (Render.Button("+100k Lunar coins"))
                {
                    foreach (NetworkUser netuser in GetAllNetworkPlayers())
                    {
                        if (netuser.master == LocalPlayer)
                        {
                            netuser.CallRpcAwardLunarCoins(100000);
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

                Render.Label(">ESP<");
                if (Render.Button("Items"))
                {
                    itemESP = !itemESP;
                }
                //if (Render.Button("Debugger")) { debugger = !debugger; getVals = true; }
                
                if (Render.Button("Spawn prefab on team"))
                {
                    spawnOnTeam = !spawnOnTeam;
                }
                
                Render.Label("> Menus <");
                if (Render.Button("Toggle Team Menu")) { teamMenu = !teamMenu; }
                if (Render.Button("Toggle Body Menu")) { bodyMenu = !bodyMenu; }
                if (Render.Button("Toggle Item Menu")) { itemMenu = !itemMenu;  loadUsers = !loadUsers; }
                if (Render.Button("Toggle Menu")) { menuToggle = !menuToggle; }



                if(itemMenu == true)
                {
                    if(loadUsers == true)
                    {
                        _TeamManager.GetTeam();
                        loadUsers = !loadUsers;
                        if(_TeamManager.ReturnTeam() != null)
                        {
                            chosenUser = _TeamManager.ReturnTeam()[0].master;
                        }
                    }
                    GUI.Label(new Rect(120, 700, 100, 20), "ITEMS");
                    GUI.Label(new Rect(200, 700, 100, 20), "Currently held");

                    List<NetworkUser> netUsers = _TeamManager.ReturnTeam();

                    for (int i = 0; i < netUsers.Count(); i++) 
                    {
                        Rect buttonPos = new Rect((0 + (i * 120)), 650, 70, 20);

                        switch (itemMenuColourInt)
                        {
                            case 1:
                                
                                itemMenuColour = GUI.color = Color.cyan;
                                break;
                            case 2:
                                itemMenuColour = GUI.color = Color.grey;
                                break;
                            case 3:
                                itemMenuColour = GUI.color = Color.green;
                                break;
                            case 4:
                                itemMenuColour = GUI.color = Color.blue;
                                break;
                            default:
                                itemMenuColour = GUI.color = Color.white;
                                break;
                        }

                        if (GUI.Button(buttonPos, netUsers[i].userName))
                        {
                            chosenUser = netUsers[i].master;
                            itemMenuColourInt = i;
                        }
                    }

                    scrollPosition = GUI.BeginScrollView(new Rect(0, 720, 300, 100), scrollPosition, new Rect(0, 0, 300, 2200));
                    GUI.Label(new Rect(0, 200, 300, 2200), "");

                    

                    for (int i = 0; i < characterVars.itemNames.Length; i++)
                    {
                        GUI.color = itemMenuColour;
                        
                        GUI.Label(new Rect(10, 22 * i, 100, 20), characterVars.itemNames[i]);
                        if (GUI.Button(new Rect(100, 22 * i, 70, 20), "Add")) { chosenUser.inventory.GiveItemString(characterVars.itemNames[i], 1); }
                        if (GUI.Button(new Rect(180, 22 * i, 70, 20), "Remove")) { chosenUser.inventory.GiveItemString(characterVars.itemNames[i], -1); }
                        GUI.color = Color.yellow;
                        GUI.Label(new Rect(250, 22 * i, 50, 20), (chosenUser.inventory.GetItemCount(ItemCatalog.FindItemIndex(characterVars.itemNames[i]))).ToString());
                    }
                    //GUI.color = Color.white;
                }

                if (itemESP == true)
                {
                    renderCount += 1;
                    if (renderCount >= 2000)
                    {
                        itemList = new List<PurchaseInteraction>();
                        renderCount = 0;
                        itemList = LoadInteractables();
                    }
                    try
                    {
                        RenderInteractables(itemList);
                    }
                    catch
                    {
                        itemList = new List<PurchaseInteraction>();
                        itemList = LoadInteractables();
                        renderCount = 2000;
                    }                                   
                }

                GUI.EndScrollView();

                if(bodyMenu == true)
                {
                    GUI.Label(new Rect(210, 140, 100, 20), "BODIES");
                    scrollPosition2 = GUI.BeginScrollView(new Rect(210, 160, 200, 100), scrollPosition2, new Rect(0, 0, 200, 3600));
                    GUI.Label(new Rect(0, 200, 200, 3600), "");
                    for (int i = 0; i < characterVars.bodyArray.Length; i++)
                    {
                        if (GUI.Button(new Rect(10, 22 * i, 150, 20), characterVars.bodyArray[i])) { LocalPlayer.CallCmdRespawn(characterVars.bodyArray[i]); }
                    }
                    GUI.EndScrollView();

                }   
            }
            else
            {
                                                                                                                
            }
            {
                Render.Begin("Risk of Tears 1.1.4", 4f, 1f, 180f, 100f, 10f, 20f, 2f);
                
                if (Render.Button("Toggle Menu")) { menuToggle = !menuToggle; }
                if (Render.Button("Unload Menu")) { Loader.Unload(); }
            }
        }
        public void Start()
        {
            useGUILayout = false;
            UpdateLocalPlayer();
        }

        int teamReloadCount = 2000;
        public void FixedUpdate()
        {
            UpdateLocalPlayer();
            
            _Body = LocalPlayer.GetBody();

            if(teamMenu == true)
            {
                _TeamManager.GetLocalPlayer(LocalPlayer);
                if(teamReloadCount >= 2000)
                {
                    _TeamManager.GetTeam();
                    teamReloadCount = 0;
                }
                teamReloadCount += 1;
            }

            
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
                NetworkServer.Spawn(gameObject);
                //UnityEngine.Object.Destroy(gameObject);
                

            }
        }

        public void Teleporter()
        {
            TeleporterInteraction tele = FindObjectOfType<TeleporterInteraction>();
            
            tele.shouldAttemptToSpawnGoldshoresPortal = true;
            tele.AddShrineStack();
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

        public static void UpdateLocalPlayer()
        {
            if (RoRMod.LocalPlayer != null)
            {
                return;
            }
            LocalPlayer = LocalUserManager.GetFirstLocalUser().cachedMasterController.master;
        }

        //Item ESP is from NicksMenuV2 https://www.unknowncheats.me/forum/downloads.php?do=file&id=34094
        public List<PurchaseInteraction> LoadInteractables()
        {
            List<PurchaseInteraction> itemList = new List<PurchaseInteraction>();
            foreach (PurchaseInteraction purchaseInteraction in UnityEngine.Object.FindObjectsOfType(typeof(PurchaseInteraction)))
            {
                bool available = purchaseInteraction.available;
                if (available)
                {
                    itemList.Add(purchaseInteraction);
                    
                }
            }
            return itemList;
        }

        public void RenderInteractables(List<PurchaseInteraction> purchaseInteraction)
        {
            for(int i = 0; i < purchaseInteraction.Count(); i++)
            {
                Vector3 vector = Camera.main.WorldToScreenPoint(purchaseInteraction[i].transform.position);
                bool flag = (double)vector.z > 0.01;
                if (flag)
                {
                    GUI.color = Color.yellow;
                    string displayName = purchaseInteraction[i].GetDisplayName() + " - $" + purchaseInteraction[i].cost;
                    GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y, 80f, 50f), displayName);
                }
            }
            
        }        
    }
}
