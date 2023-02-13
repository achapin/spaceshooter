using System.Collections.Generic;
using System.Linq;
using Input;
using Ships.ShipSystems.Weapons;
using UnityEngine;

namespace Ships.ShipSystems
{
    public class WeaponSystem : IShipSystem
    {
        private ShipConfig _config;
        private Ship _ship;
        private List<Weapon> _weapons;

        private float _currentPower;
        
        //Between 0-2. Values over 1 result in a bonus damage multiplier
        private float _chargeLevel = 1f;

        public void Initialize(ShipConfig config, Ship ship)
        {
            _config = config;
            _ship = ship;

            _weapons = _config.weapons.ToList();
            foreach (var weapon in _weapons)
            {
                weapon.Initialize(ship);
            }
        }

        public void AllocatePower(float percentage)
        {
            _currentPower = percentage;
        }

        public void Update(float deltaTime, InputState inputState)
        {
            _chargeLevel = Mathf.MoveTowards(_chargeLevel, 2f,
                deltaTime * _config.weaponSystemRecharge.Evaluate(_currentPower));
            foreach (var weapon in _weapons)
            {
                weapon.UpdateWeapon(this, inputState, deltaTime);
            }
        }

        public bool ExpendCharge(float chargeAmount)
        {
            bool canFire = chargeAmount <= _chargeLevel;
            if (canFire)
            {
                _chargeLevel -= chargeAmount;
            }

            return canFire;
        }

        public float BonusDamageMultiplier()
        {
            return _config.weaponBonusDamage.Evaluate(Mathf.Clamp01(_chargeLevel - 1f));
        }

        public float CurrentPower()
        {
            return _currentPower;
        }
    }
}