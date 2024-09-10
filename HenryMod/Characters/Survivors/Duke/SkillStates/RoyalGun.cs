using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.AddressableAssets;

namespace DukeMod.Survivors.Duke.SkillStates
{
    class RoyalGun : BaseSkillState, SteppedSkillDef.IStepSetter, ISkillState
    {
        public static float damageCoefficient = 3.2f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 2f;
        public static float range = 256f;
        public static float damageMultiplier = 1f; //there surely is a more fancy way of doing this
        public int shotCount;

        public GameObject tracerEffectPrefab;


        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private string audioString;
        public GenericSkill activatorSkillSlot { get; set; }


        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            shotCount = i;
        }

        //To anyone reading this, hi.

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";



            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            //ChangeCount(); testing if this thing even worked to begin with
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);

                if (shotCount == 3)
                {
                    //For some reason Railgunner's tracer disappears when it actually hits something, either way, only temporary.
                    tracerEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/TracerRailgun.prefab").WaitForCompletion();
                    audioString = "Play_railgunner_m2_fire";
                    damageMultiplier = 2f;
                }
                else
                {
                    tracerEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/GoldGat/TracerGoldGat.prefab").WaitForCompletion();
                    audioString = "HenryShootPistol";
                    damageMultiplier = 1f;
                }
                

                Util.PlaySound(audioString, gameObject);

                //System.Console.WriteLine("The current tracer: ", tracerEffectPrefabString, "current shot count: ", shotCount);
                //System.Console.WriteLine(tracerEffectPrefabString);
                System.Console.WriteLine("current shot count" + shotCount);

                if (isAuthority)
                {
                    //Exact copy of Henry's shoot skill
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damageCoefficient * damageStat * damageMultiplier,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = range,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = RollCrit(),
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        procChainMask = default,
                        procCoefficient = procCoefficient,
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = tracerEffectPrefab,
                        spreadPitchScale = 1f,
                        spreadYawScale = 1f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    }.Fire();
                }
            }
        }

        public void ChangeCount()
        {
            ((SteppedSkillDef.InstanceData)skillLocator.primary.skillInstanceData).step = 3;
            System.Console.WriteLine("changed shot count to: " + shotCount);
        }
        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
