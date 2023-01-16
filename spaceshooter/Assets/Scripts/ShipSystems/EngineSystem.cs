using UnityEngine;

namespace ShipSystems
{
    public class EngineSystem : IShipSystem
    {
        private ShipConfig _config;
        private Ship _ship;
        private float _currentPower;

        //0-1
        private float _throttle;

        private float _currentSpeed;

        private float _boostReserve;

        public void Initialize(ShipConfig config, Ship ship)
        {
            _config = config;
            _ship = ship;
        }

        public void AllocatePower(float percentage)
        {
            _currentPower = percentage;
        }

        public void Update(float deltaTime)
        {
            _boostReserve = _config.boostChargeRate.Evaluate(_currentPower) * deltaTime;
            var maxSpeed = _config.maximumSpeed.Evaluate(_currentPower);
            var targetSpeed = _throttle * maxSpeed;

            if (_currentSpeed > targetSpeed)
            {
                _currentSpeed -= _config.frictionAtSpeed.Evaluate(_currentSpeed) * Time.deltaTime;
            }

            if (_currentSpeed < targetSpeed)
            {
                _currentSpeed += _config.acceleration.Evaluate(_currentPower) * Time.deltaTime;
            }
        }
    }
}