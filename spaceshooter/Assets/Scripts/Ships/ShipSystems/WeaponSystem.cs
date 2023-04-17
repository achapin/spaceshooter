using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Input;
using Ships.ShipSystems.Weapons;
using UnityEngine;

[assembly: InternalsVisibleTo("ShipTests")]
namespace Ships.ShipSystems
{
    public class WeaponSystem : IShipSystem
    {
        private ShipConfig _config;
        private Ship _ship;
        internal List<Weapon> _weapons;

        private float _currentPower;
        
        //Between 0-2. Values over 1 result in a bonus damage multiplier
        internal float _chargeLevel = 1f;

        public void Initialize(ShipConfig config, Ship ship)
        {
            _config = config;
            _ship = ship;

            _weapons = new List<Weapon>();
            foreach (var weapon in _config.weapons)
            {
                var weaponInstance = Object.Instantiate(weapon);
                _weapons.Add(weaponInstance);
                weaponInstance.Initialize(ship);
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