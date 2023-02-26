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

        public SimpleWeaponDisplay displayPrefab;

        private float _fireCountdown;

        private Ship _ship;
        private SimpleWeaponDisplay _displayInstance;

        public override void Initialize(Ship ship)
        {
            Debug.Log($"Simple weapon initialized with ship {ship.gameObject.name}");
            _ship = ship;
            _displayInstance = GameObject.Instantiate(displayPrefab);
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
                var damageHandler = hitInfo.collider.GetComponent<DamageableHandler>();
                if( damageHandler != null)
                {
                    damageHandler.TakeDamage(damageToDo);
                }
            }
            else
            {
                _displayInstance.ShowShot(_ship.transform.position, _ship.transform.TransformPoint(Vector3.forward) * 100f, false);
            }
        }
    }
}
