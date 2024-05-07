﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoRModNET4
{
    internal class CharacterVars : MonoBehaviour
    {
        public float health = 250f;
        public string bodyName = "";
        public float baseSpeed = 0f;

        public Color[] colours = new Color[]
        {
            Color.white,
            Color.yellow,
            Color.blue,
            Color.red,
            Color.cyan,
            Color.green,
            Color.magenta,
            Color.black
        };

        public string[] pickupLines = new string[]
        {
            "Your juicy phat cock! <3",
            "A deadly disease",
            "Some dog shit I think?",
            "You. He is carrying the crap out of this team.",
            "A small pigeon",
            "That sexy ass ;)",
            "Inconveniently oversized coconut",
            "Homeless mans pocket lint",
            "Mysterious Soggy Sock",
            "Straight up HEROIN",
            "A smidgen of cocaine",
            "Misplaced Condom Wrapper",
            "Disco Ball of Shame",
            "Lube-Soaked Feather Duster",
            "Bootylicious Spanking Paddle"

        };

        public string[] bodyArray = new string[] {
            "AcidLarvaBody",
            "AffixEarthHealerBody",
            "AltarSkeletonBody",
            "AncientWispBody",
            "ArchWispBody",
            "ArtifactShellBody",
            "Assassin2Body",
            "AssassinBody",
            "BackupDroneBody",
            "BackupDroneOldBody",
            "Bandit2Body",
            "BanditBody",
            "BeetleBody",
            "BeetleCrystalBody",
            "BeetleGuardAllyBody",
            "BeetleGuardBody",
            "BeetleGuardCrystalBody",
            "BeetleQueen2Body",
            "BeetleWard",
            "BellBody",
            "BirdsharkBody",
            "BisonBody",
            "BomberBody",
            "BrotherBody",
            "BrotherGlassBody",
            "BrotherHauntBody",
            "BrotherHurtBody",
            "CaptainBody",
            "ClayBody",
            "ClayBossBody",
            "ClayBruiserBody",
            "ClayGrenadierBody",
            "CommandoBody",
            "CommandoPerformanceTestBody",
            "CrocoBody",
            "DeathProjectile",
            "Drone1Body",
            "Drone2Body",
            "DroneCommanderBody",
            "ElectricWormBody",
            "EmergencyDroneBody",
            "EnforcerBody",
            "EngiBeamTurretBody",
            "EngiBody",
            "EngiTurretBody",
            "EngiWalkerTurretBody",
            "EquipmentDroneBody",
            "ExplosivePotDestructibleBody",
            "FlameDroneBody",
            "FlyingVerminBody",
            "FusionCellDestructibleBody",
            "GeepBody",
            "GipBody",
            "GolemBody",
            "GolemBodyInvincible",
            "GrandParentBody",
            "GravekeeperBody",
            "GravekeeperTrackingFireball",
            "GreaterWispBody",
            "GupBody",
            "HANDBody",
            "HaulerBody",
            "HereticBody",
            "HermitCrabBody",
            "HuntressBody",
            "ImpBody",
            "ImpBossBody",
            "JellyfishBody",
            "LemurianBody",
            "LemurianBruiserBody",
            "LoaderBody",
            "LunarExploderBody",
            "LunarGolemBody",
            "LunarWispBody",
            "LunarWispTrackingBomb",
            "MageBody",
            "MagmaWormBody",
            "MajorConstructBody",
            "MegaConstructBody",
            "MegaDroneBody",
            "MercBody",
            "MiniMushroomBody",
            "MiniVoidRaidCrabBodyBase",
            "MiniVoidRaidCrabBodyPhase1",
            "MiniVoidRaidCrabBodyPhase2",
            "MiniVoidRaidCrabBodyPhase3",
            "MinorConstructAttachableBody",
            "MinorConstructBody",
            "MinorConstructOnKillBody",
            "MissileDroneBody",
            "NullifierAllyBody",
            "NullifierBody",
            "PaladinBody",
            "ParentBody",
            "ParentPodBody",
            "Pot2Body",
            "PotMobile2Body",
            "PotMobileBody",
            "RailgunnerBody",
            "RoboBallBossBody",
            "RoboBallGreenBuddyBody",
            "RoboBallMiniBody",
            "RoboBallRedBuddyBody",
            "SMInfiniteTowerMaulingRockLarge",
            "SMInfiniteTowerMaulingRockMedium",
            "SMInfiniteTowerMaulingRockSmall",
            "SMMaulingRockLarge",
            "SMMaulingRockMedium",
            "SMMaulingRockSmall",
            "ScavBody",
            "ScavLunar1Body",
            "ScavLunar2Body",
            "ScavLunar3Body",
            "ScavLunar4Body",
            "ScavSackProjectile",
            "ShopkeeperBody",
            "SniperBody",
            "SpectatorBody",
            "SpectatorSlowBody",
            "SquidTurretBody",
            "SulfurPodBody",
            "SuperRoboBallBossBody",
            "TimeCrystalBody",
            "TitanBody",
            "TitanGoldBody",
            "ToolbotBody",
            "TreebotBody",
            "Turret1Body",
            "UrchinTurretBody",
            "VagrantBody",
            "VagrantTrackingBomb",
            "VerminBody",
            "VoidBarnacleBody",
            "VoidBarnacleNoCastBody",
            "VoidInfestorBody",
            "VoidJailerAllyBody",
            "VoidJailerBody",
            "VoidMegaCrabAllyBody",
            "VoidMegaCrabBody",
            "VoidRaidCrabBody",
            "VoidRaidCrabJointBody",
            "VoidSurvivorBody",
            "VultureBody",
            "VultureEggBody",
            "WispBody",
            "WispSoulBody"
        };
        public string[] itemNames = new string[]
        {
            "Syringe",
            "Bear",
            "Behemoth",
            "Missile",
            "ExplodeOnDeath",
            "Dagger",
            "Tooth",
            "CritGlasses",
            "Hoof",
            "Feather",
            "AACannon",
            "ChainLightning",
            "PlasmaCore",
            "Seed",
            "Icicle",
            "GhostOnKill",
            "Mushroom",
            "Crowbar",
            "LevelBonus",
            "AttackSpeedOnCrit",
            "BleedOnHit",
            "SprintOutOfCombat",
            "FallBoots",
            "CooldownOnCrit",
            "WardOnLevel",
            "Phasing",
            "HealOnCrit",
            "HealWhileSafe",
            "TempestOnKill",
            "PersonalShield",
            "EquipmentMagazine",
            "NovaOnHeal",
            "ShockNearby",
            "Infusion",
            "WarCryOnCombat",
            "Clover",
            "Medkit",
            "Bandolier",
            "BounceNearby",
            "IgniteOnKill",
            "PlantOnHit",
            "StunChanceOnHit",
            "Firework",
            "LunarDagger",
            "GoldOnHit",
            "MageAttunement",
            "WarCryOnMultiKill",
            "BoostHp",
            "BoostDamage",
            "ShieldOnly",
            "AlienHead",
            "Talisman",
            "Knurl",
            "BeetleGland",
            "BurnNearby",
            "CritHeal",
            "CrippleWardOnLevel",
            "SprintBonus",
            "SecondarySkillMagazine",
            "StickyBomb",
            "TreasureCache",
            "BossDamageBonus",
            "SprintArmor",
            "IceRing",
            "FireRing",
            "SlowOnHit",
            "ExtraLife",
            "ExtraLifeConsumed",
            "UtilitySkillMagazine",
            "HeadHunter",
            "KillEliteFrenzy",
            "RepeatHeal",
            "Ghost",
            "HealthDecay",
            "AutoCastEquipment",
            "IncreaseHealing",
            "JumpBoost",
            "DrizzlePlayerHelper",
            "ExecuteLowHealthElite",
            "EnergizedOnEquipmentUse",
            "BarrierOnOverHeal",
            "TonicAffliction",
            "TitanGoldDuringTP",
            "SprintWisp",
            "BarrierOnKill",
            "ArmorReductionOnHit",
            "TPHealingNova",
            "NearbyDamageBonus",
            "LunarUtilityReplacement",
            "MonsoonPlayerHelper",
            "Thorns",
            "RegenOnKill",
            "Pearl",
            "ShinyPearl",
            "BonusGoldPackOnKill",
            "LaserTurbine",
            "LunarPrimaryReplacement",
            "NovaOnLowHealth",
            "LunarTrinket"
        };

    }
}
