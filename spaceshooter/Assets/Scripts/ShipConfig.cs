using UnityEngine;

[CreateAssetMenu(fileName = "ShipConfig", menuName = "Config/CreateShipConfig", order = 1)]
public class ShipConfig : ScriptableObject
{
    public float energyCapacity;
    public float maxHp;
    public AnimationCurve maximumSpeed;
    public AnimationCurve rotationSpeed;
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
    
}
