using RoR2;
using UnityEngine;
using DukeMod.Modules;
using System;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;

namespace DukeMod.Survivors.Duke
{
    public static class DukeAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //the buffward
        public static GameObject railgunnerBuffWardClone;
        public static BuffWard replicatorBuffWardComponent;

        //projectiles
        public static GameObject bombProjectilePrefab;
        public static GameObject replicatorProjectilePrefab;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();
        }

        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");
        }

        private static void CreateBombExplosionEffect()
        {
            bombExplosionEffect = _assetBundle.LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (!bombExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            CreateBombProjectile();
            CreateReplicatorProjectile();
            Content.AddProjectilePrefab(bombProjectilePrefab);
            Content.AddProjectilePrefab(replicatorProjectilePrefab);
        }

        private static void CreateBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();
            
            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombProjectilePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
                bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");
            
            bombController.startSound = "";
        }

        private static void CreateReplicatorProjectile()
        {

            //Cloning commando's grenade projectile
            replicatorProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "DukeReplicatorProjectile");

            //removing components I don't want
            UnityEngine.Object.Destroy(replicatorProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            UnityEngine.Object.Destroy(replicatorProjectilePrefab.GetComponent<ProjectileDamage>());

            //adding components
            ProjectileStickOnImpact replicatorStickOnImpact = replicatorProjectilePrefab.AddComponent<ProjectileStickOnImpact>();

            //the buffward
            railgunnerBuffWardClone = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerMineAltDetonated.prefab").WaitForCompletion();
            replicatorBuffWardComponent = railgunnerBuffWardClone.GetComponent<BuffWard>();

            BuffWard replicatorBuffWard = replicatorProjectilePrefab.AddComponent<BuffWard>();


            //Getting the components I want
            ProjectileController replicatorController = replicatorProjectilePrefab.GetComponent<ProjectileController>();
            ProjectileSimple replicatorSimple = replicatorProjectilePrefab.GetComponent<ProjectileSimple>();

    
            //projectile controller stuff
            if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
                replicatorController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost"); 
            replicatorController.startSound = "";

            //projectile simple stuff
            replicatorSimple.lifetime = 16f;

            //projectile sticky stuff
            replicatorStickOnImpact.NetworkhitHurtboxIndex = -1;
            replicatorStickOnImpact.stickSoundString = "Play_railgunner_shift_land";
            replicatorStickOnImpact.ignoreCharacters = false;
            replicatorStickOnImpact.ignoreWorld = false;
            replicatorStickOnImpact.alignNormals = true;

            //projectile buffward
            replicatorBuffWard.Networkradius = 15f;
            replicatorBuffWard.radius = 15f;
            replicatorBuffWard.interval = 0.5f;
            replicatorBuffWard.expireDuration = 10;
            replicatorBuffWard.removalTime = 0;

            replicatorBuffWard.shape = BuffWard.BuffWardShape.Sphere;
            replicatorBuffWard.rangeIndicator = replicatorBuffWardComponent.rangeIndicator;
            //replicatorBuffWard.buffDef = BuffDef.;
            replicatorBuffWard.radiusCoefficientCurve = replicatorBuffWardComponent.radiusCoefficientCurve;

            replicatorBuffWard.expires = true;
            replicatorBuffWard.floorWard = false;
            replicatorBuffWard.animateRadius = false;
            replicatorBuffWard.invertTeamFilter = true;
            replicatorBuffWard.requireGrounded = false;
        }   

        #endregion projectiles
    }
}
