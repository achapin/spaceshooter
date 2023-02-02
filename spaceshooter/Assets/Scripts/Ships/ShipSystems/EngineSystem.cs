using System;
using System.Runtime.CompilerServices;
using Input;
using UnityEngine;

[assembly: InternalsVisibleTo("ShipTests")]

namespace Ships.ShipSystems
{
    public class EngineSystem : IShipSystem
    {
        private ShipConfig _config;
        private Ship _ship;
        private float _currentPower;

        private const float maxAngle = 75f; 

        //0-1
        private float _throttle;

        private float _currentSpeed;
        private float _groundAngle = 0f;

        internal float _boostReserve;

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
            _throttle = inputState.throttle;

            var maxSpeed = _config.maximumSpeed.Evaluate(_currentPower);
            var targetSpeed = _throttle * maxSpeed;

            if (_currentSpeed > targetSpeed)
            {
                _currentSpeed -= _config.frictionAtSpeed.Evaluate(_currentSpeed) * Time.deltaTime;
            }

            if (_currentSpeed < targetSpeed)
            {
                _currentSpeed += Mathf.MoveTowards(_currentSpeed, targetSpeed,
                    _config.acceleration.Evaluate(_currentPower) * Time.deltaTime);
            }

            if (inputState.isBoosting)
            {
                if (_boostReserve > 0)
                {
                    _currentSpeed = Mathf.Clamp(_currentSpeed + _config.boostAcceleration * deltaTime, 0f,
                        _config.boostSpeed);
                }

                _boostReserve = Mathf.MoveTowards(_boostReserve, 0f, _config.boostBurnRate * deltaTime);
            }
            else
            {
                _boostReserve += _config.boostChargeRate.Evaluate(_currentPower) * deltaTime;
                _boostReserve = Mathf.Clamp(_boostReserve, 0f, _config.boostCapacity);
            }
            
            _ship.transform.Rotate(Vector3.up, inputState.joystick.x * _config.rotationSpeed.Evaluate(_currentPower) * deltaTime, Space.World);
            _groundAngle += inputState.joystick.y * _config.rotationSpeed.Evaluate(_currentPower) * deltaTime;
            _groundAngle = Mathf.Clamp(_groundAngle, -maxAngle, maxAngle);
            _ship.transform.localRotation = Quaternion.AngleAxis(_groundAngle, Vector3.right); 
            //_ship.transform.Rotate(_ship.transform.right, inputState.joystick.y * _config.rotationSpeed.Evaluate(_currentPower) * deltaTime, Space.World);

            _ship.transform.Translate(Vector3.forward * _currentSpeed * deltaTime);
        }

        public float CurrentPower()
        {
            return _currentPower;
        }
    }
}