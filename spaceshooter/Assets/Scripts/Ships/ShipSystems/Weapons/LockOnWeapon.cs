using Damage;
using Input;
using UnityEngine;

namespace Ships.ShipSystems.Weapons
{
    
    [CreateAssetMenu(fileName = "LockOnWeapon", menuName = "Weapons/LockOnWeapon", order = 2)]
    public class LockOnWeapon : Weapon
    {
        //Flat damage amount done
        public float damage;
        public DamageType damageType;
        //Uses the concept of abstract "lock on points", which accumulate frame over frame, faster if the target is close to the reticle
        public float lockOnRequired;
        //How many points per second accumulate, based on the angle between the player's target line and the target
        public AnimationCurve lockPointsPerSecondByAngle;
        //If the weapon successfully fires, how long to cooldown
        public float cooldownTimeAfterFire;
        //If the weapon isn't fired successfully (no target, not fully charged, etc), how long to cooldown
        public float cooldownTimeAfterRelease;
        //How far to cast when acquiring potential targets
        public float lockRange;
        //Maximum range that the target can be and still get hit
        public float fireRange;
        //Maximum angle from the forward vector which the weapon will still hit
        public float maxHitAngle;
        
        private Ship _ship;

        private DamageableHandler target;
        private float lockPoints;
        private float cooldown;
        
        public override void Initialize(Ship ship)
        {
            _ship = ship;
        }

        public override void UpdateWeapon(WeaponSystem weaponSystem, InputState inputState, float deltaTime)
        {
            if (cooldown > 0f)
            {
                cooldown -= deltaTime;
                return;
            }

            if (!inputState.isFiring)
            {
                //TODO: Check maximum distance and angle as well
                if (target == null || lockPoints < lockOnRequired)
                {
                    cooldown = cooldownTimeAfterRelease;
                }
                else
                {
                    cooldown = cooldownTimeAfterFire;
                    target.TakeDamage(damage, damageType);
                }
                return;
            }

            if (target == null)
            {
                //TODO:
                //Spherecast, and find the closest target to nominate
            }
            //TODO:
            //Charge lock-on percentage based on how close the target is to the lock-on line
        }
    }
}
