using Damage;
using Ships.ShipSystems.Weapons;
using UnityEngine;

namespace Ships
{
    [CreateAssetMenu(fileName = "ShipConfig", menuName = "Config/CreateShipConfig", order = 1)]
    public class ShipConfig : ScriptableObject
    {
        public float energyCapacity;
        public float maxHp;
        public AnimationCurve maximumSpeed;
        public AnimationCurve pitchSpeed;
        public AnimationCurve yawSpeed;
        public AnimationCurve boostChargeRate;
        public AnimationCurve frictionAtSpeed;
        public AnimationCurve acceleration;
        public float boostSpeed;
        public float boostAcceleration;
        public float boostCapacity;
        public float boostBurnRate;
        public AnimationCurve weaponSystemRecharge;
        public AnimationCurve weaponBonusDamage;
        public float shieldCapacity;
        public AnimationCurve shieldRechargeRate;
        public AnimationCurve shieldRechargeTime;
        public Weapon[] weapons;
        public DamageConfig shieldDamageConfig;
        public DamageConfig shipDamageConfig;

    }
}
