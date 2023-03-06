using System.Runtime.CompilerServices;
using Input;
using UnityEngine;

[assembly: InternalsVisibleTo("ShipTests")]
namespace Ships.ShipSystems
{
    public class ShieldSystem : IShipSystem
    {
        private ShipConfig _config;
        private Ship _ship;

        internal float _currentPower;
        internal float _shieldStrength;
        internal float _timeToRecharge;

        public void Initialize(ShipConfig config, Ship ship)
        {
            _config = config;
            _ship = ship;
        }

        public void AllocatePower(float percentage)
        {
            _currentPower = percentage;
        }

        public void Update(float deltaTime, InputState inputState)
        {
            if (_timeToRecharge > 0f)
            {
                if (_timeToRecharge < deltaTime)
                {
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
                    deltaTime * _config.shieldRechargeRate.Evaluate(_currentPower));
            }
        }

        public float ReduceDamage(float damage)
        {
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