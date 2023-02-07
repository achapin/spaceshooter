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
        private Transform _transform;
        private Rigidbody _rigidbody;
        private float _currentPower;

        private const float maxAngle = 75f;
        private const float restitution = 3f;

        //0-1
        private float _throttle;

        private float _currentSpeed;

        internal float _boostReserve;

        public void Initialize(ShipConfig config, Ship ship)
        {
            _config = config;
            _ship = ship;
            _transform = ship.GetComponent<Transform>();
            _rigidbody = ship.GetComponent<Rigidbody>();
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
            
            _transform.Rotate(Vector3.up, inputState.joystick.x * _config.yawSpeed.Evaluate(_currentPower) * deltaTime, Space.World);
            var groundAngleDelta = inputState.joystick.y * _config.pitchSpeed.Evaluate(_currentPower) * deltaTime;
            if (Mathf.DeltaAngle(_transform.localEulerAngles.x, groundAngleDelta + _transform.localEulerAngles.x) > maxAngle)
            {
                groundAngleDelta = maxAngle - _transform.localEulerAngles.x;
            } 
            else if (groundAngleDelta + _transform.localEulerAngles.x < -maxAngle)
            {
                groundAngleDelta = -maxAngle - _transform.localEulerAngles.x;
            }

            if (Mathf.Abs(inputState.joystick.y) < Mathf.Epsilon)
            {
                groundAngleDelta = Mathf.MoveTowardsAngle(_transform.localEulerAngles.x, 0f, -deltaTime * restitution);
            }

            Quaternion targetRotation = Quaternion.AngleAxis(groundAngleDelta, _transform.right);
            _transform.rotation = targetRotation * _transform.rotation;

            _rigidbody.velocity = _transform.forward * _currentSpeed;
        }

        public float CurrentPower()
        {
            return _currentPower;
        }
    }
}