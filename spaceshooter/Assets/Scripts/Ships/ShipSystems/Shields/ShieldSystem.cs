using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Damage;
using Input;
using UnityEngine;

[assembly: InternalsVisibleTo("ShipTests")]
namespace Ships.ShipSystems.Shields
{
    public class ShieldSystem : IShipSystem
    {
        private ShipConfig _config;
        private Ship _ship;
        private Dictionary<DamageType, float> _damageModifiers;

        internal float _currentPower;
        internal float _shieldStrength;
        internal float _timeToRecharge;

        public void Initialize(ShipConfig config, Ship ship)
        {
            _config = config;
            _ship = ship;
            _damageModifiers = new Dictionary<DamageType, float>();
            if(config.shieldDamageConfig != null)
            {
                foreach (var damageConfigEntry in _config.shieldDamageConfig.configEntries)
                {
                    _damageModifiers.Add(damageConfigEntry.damageType, damageConfigEntry.multiplier);
                }
            }
        }

        public void AllocatePower(float percentage)
        {
            _currentPower = percentage;
        }

        public void Update(float deltaTime, InputState inputState)
        {
            var deltaTimeToUse = deltaTime;
            if (_timeToRecharge > 0f)
            {
                if (_timeToRecharge < deltaTime)
                {
                    deltaTimeToUse -= _timeToRecharge;
                    _timeToRecharge = 0f;
                }
                else
                {
                    _timeToRecharge -= deltaTime;
                    return;
                }
            }
            if (_shieldStrength < _config.shieldCapacity)
            {
                _shieldStrength = Mathf.MoveTowards(_shieldStrength, _config.shieldCapacity,
                    deltaTimeToUse * _config.shieldRechargeRate.Evaluate(_currentPower));
            }
        }

        public float ReduceDamage(float damage, DamageType damageType)
        {
            if (_damageModifiers.ContainsKey(damageType))
            {
                damage *= _damageModifiers[damageType];
            }
            
            if (damage < _shieldStrength)
            {
                _shieldStrength -= damage;
                return 0f;
            }

            _timeToRecharge = _config.shieldRechargeTime.Evaluate(_currentPower);
            var blocked = damage - _shieldStrength;
            _shieldStrength = 0f;
            return blocked;
        }

        public float CurrentPower()
        {
            return _currentPower;
        }
    }
}