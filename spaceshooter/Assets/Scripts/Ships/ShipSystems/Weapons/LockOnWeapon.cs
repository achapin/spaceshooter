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
                if (target != null)
                {
                    //TODO: Check maximum distance and angle as well
                    if (lockPoints < lockOnRequired)
                    {
                        Debug.Log($"Released with {lockPoints} / {lockOnRequired} points. Clearing");
                    }
                    else
                    {
                        //TODO: VFX?
                        Debug.Log($"Released with {lockPoints} points. Damaging and clearing");
                        cooldown = cooldownTimeAfterFire;
                        target.TakeDamage(damage, damageType);
                    }
                    ReleaseTarget();
                }

                return;
            }

            if (target == null)
            {
                float angle = float.MaxValue;
                DamageableHandler targetCandidate = null;
                foreach (var collider in Physics.OverlapSphere(_ship.transform.position, lockRange))
                {
                    if (collider.gameObject.GetComponent<DamageableHandler>() != null)
                    {
                        Vector3 toTarget = collider.transform.position - _ship.transform.position;
                        var angleToTarget = Vector3.Angle(toTarget, _ship.transform.forward); 
                        if (angleToTarget > maxHitAngle)
                        {
                            Debug.DrawLine(collider.transform.position, _ship.transform.position, Color.red, .5f);
                            continue;
                        }

                        var distance = Vector3.Distance(collider.transform.position, _ship.transform.position); 
                        if (distance < fireRange)
                        {
                            if(angleToTarget < angle)
                            {
                                Debug.DrawLine(collider.transform.position, _ship.transform.position, Color.green, .5f);
                                targetCandidate = collider.gameObject.GetComponent<DamageableHandler>();
                                angle = angleToTarget;
                            }
                            else
                            {
                                Debug.DrawLine(collider.transform.position, _ship.transform.position, Color.yellow, .5f);
                            }
                        }
                    }
                }
                target = targetCandidate;
                if (target != null)
                {
                    Debug.Log($"Selected target {target.gameObject.name}");
                }
                else
                {
                    return;
                }
            }
            
            Vector3 towardsTarget = target.transform.position - _ship.transform.position;
            var targetAngle = Vector3.Angle(towardsTarget, _ship.transform.forward);
            var targetDistance = Vector3.Distance(target.transform.position, _ship.transform.position);
            if (targetDistance > fireRange || targetAngle > maxHitAngle)
            {
                Debug.Log($"Target has gone out of range or angle");
                ReleaseTarget();
            }
            else
            {
                lockPoints += lockPointsPerSecondByAngle.Evaluate(targetAngle) * deltaTime;
            }
        }

        private void ReleaseTarget()
        {
            lockPoints = 0f;
            cooldown = cooldownTimeAfterRelease;
            target = null;
        }
    }
}
