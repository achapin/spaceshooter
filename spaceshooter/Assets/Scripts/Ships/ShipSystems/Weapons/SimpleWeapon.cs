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

        public DamageType damageType;

        public SimpleWeaponDisplay displayPrefab;

        private float _fireCountdown;

        private Ship _ship;
        private SimpleWeaponDisplay _displayInstance;

        private void OnValidate()
        {
            if (damageType == null)
            {
                Debug.LogError($"No Damage Type set for Simple Weapon {name}");
            }

            if (displayPrefab == null)
            {
                Debug.LogError($"No Display Prefab set for Simple Weapon {name}");
            }

            if (damageToDo <= 0)
            {
                Debug.LogError($"Simple weapon {name} will do no Damage");
            }

            if (timeToFire <= 0)
            {
                Debug.Log($"Simple Weapon {name} has an invalid time to fire");
            }
        }

        public override void Initialize(Ship ship)
        {
            Debug.Log($"Simple weapon initialized with ship {ship.gameObject.name}");
            _ship = ship;
            _displayInstance = Instantiate(displayPrefab);
        }

        public override void UpdateWeapon(WeaponSystem weaponSystem, InputState inputState, float deltaTime)
        {
            _fireCountdown -= deltaTime;
            if (inputState.isFiring && _fireCountdown <= 0f && weaponSystem.ExpendCharge(chargeToSpend))
            {
                FireWeapon();
            }
        }

        private void FireWeapon()
        {
            _fireCountdown = timeToFire;
            if (Physics.Raycast(_ship.transform.position, _ship.transform.forward, out RaycastHit hitInfo))
            {
                _displayInstance.ShowShot(_ship.transform.position, hitInfo.point, true);
                var damageHandler = hitInfo.collider.GetComponentInParent<DamageableHandler>();
                if( damageHandler != null)
                {
                    damageHandler.TakeDamage(damageToDo, damageType);
                }
            }
            else
            {
                _displayInstance.ShowShot(_ship.transform.position, _ship.transform.position + _ship.transform.forward.normalized * 100f, false);
            }
        }
    }
}
