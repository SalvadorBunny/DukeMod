using EntityStates;
using DukeMod.Survivors.Duke;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using DukeMod.Survivors.Duke.SkillStates;

namespace DukeMod.Survivors.Duke.SkillStates
{
    public class SimpleAmbush : BaseSkillState
    {
        //Stolen from Paladin :]
        protected Vector3 slipVector = Vector3.zero;
        public float duration = 0.45f;
        public float speedCoefficient = 5.5f;

        private Vector3 cachedForward;

        public override void OnEnter()
        {
            base.OnEnter();
            this.slipVector = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            this.cachedForward = this.characterDirection.forward;

            var skillLocator = characterBody.GetComponent<SkillLocator>();

            RoyalGun test = skillLocator.primary.GetComponent<RoyalGun>();

            test.ChangeCount();

            PlayAnimation("FullBody, Override", "Roll", "Roll.playbackRate", duration);
            Util.PlaySound(EntityStates.BrotherMonster.BaseSlideState.soundString, base.gameObject);
        }




        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterMotor.velocity = Vector3.zero;
            base.characterMotor.rootMotion += this.slipVector * (this.moveSpeedStat * this.speedCoefficient * Time.deltaTime) * Mathf.Cos(base.fixedAge / (this.duration * 1.3f) * 1.57079637f);

            if (base.isAuthority)
            {
                if (base.characterDirection)
                {
                    base.characterDirection.forward = this.cachedForward;
                }
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public virtual void DampenVelocity()
        {
            base.characterMotor.velocity *= 1f;
        }

        public override void OnExit()
        {
            this.DampenVelocity();
            //base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}