using Input;
using UnityEngine;

namespace Ships.ShipSystems.Weapons
{
    [CreateAssetMenu(fileName = "SimpleWeapon", menuName = "Weapons/SimpleWeapon", order = 1)]

    public class SimpleWeapon : Weapon
    {
        public float chargeToSpend;
        public float timeToFire;

        private float _fireCountdown;

        public override void Initialize(Ship ship)
        {
            Debug.Log($"Simple weapon initialized with ship {ship.gameObject.name}");
        }

        public override void UpdateWeapon(WeaponSystem weaponSystem, InputState inputState, float deltaTime)
        {
            _fireCountdown -= deltaTime;
            if (inputState.isFiring && _fireCountdown <= 0f && weaponSystem.ExpendCharge(chargeToSpend))
            {
                Debug.Log("Pew");
                _fireCountdown = timeToFire;
            }
        }
    }
}
