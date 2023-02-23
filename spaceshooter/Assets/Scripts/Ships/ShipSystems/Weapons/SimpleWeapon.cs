using Damage;
using Input;
using UnityEngine;

namespace Ships.ShipSystems.Weapons
{
    [CreateAssetMenu(fileName = "SimpleWeapon", menuName = "Weapons/SimpleWeapon", order = 1)]

    public class SimpleWeapon : Weapon
    {
        public float chargeToSpend;
        public float timeToFire;
        public float damageToDo;

        private float _fireCountdown;

        private Ship _ship;

        public override void Initialize(Ship ship)
        {
            Debug.Log($"Simple weapon initialized with ship {ship.gameObject.name}");
            _ship = ship;
        }

        public override void UpdateWeapon(WeaponSystem weaponSystem, InputState inputState, float deltaTime)
        {
            _fireCountdown -= deltaTime;
            if (inputState.isFiring && _fireCountdown <= 0f && weaponSystem.ExpendCharge(chargeToSpend))
            {
                FireWeapon();
            }
        }

        protected virtual void FireWeapon()
        {
            _fireCountdown = timeToFire;
            if (Physics.Raycast(_ship.transform.position, _ship.transform.forward, out RaycastHit hitInfo))
            {
                Debug.Log($"Pew! hit {hitInfo.collider.gameObject.name}");
                var damageHandler = hitInfo.collider.GetComponent<DamageableHandler>();
                if( damageHandler != null)
                {
                    damageHandler.TakeDamage(damageToDo);
                }
            }
            else
            {
                Debug.Log("MISSED");
            }
        }
    }
}
