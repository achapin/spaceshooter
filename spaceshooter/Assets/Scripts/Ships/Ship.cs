using System;
using System.Collections.Generic;
using Input;
using Ships.ShipSystems;
using UnityEngine;

namespace Ships
{
    public class Ship : MonoBehaviour
    {
        [SerializeField] private float powerAllocationSpeed = 1f;

        [SerializeField] private ShipConfig config;

        private List<IShipSystem> shipSystems;
        private EngineSystem _engineSystem;
        private WeaponSystem _weaponSystem;
        private ShieldSystem _shieldSystem;

        private InputState _inputState;

        void Start()
        {
            if (config == null)
            {
                throw new Exception("Ship cannot be created without a ship config");
            }

            _engineSystem = new EngineSystem();
            _weaponSystem = new WeaponSystem();
            _shieldSystem = new ShieldSystem();

            shipSystems = new List<IShipSystem>();
            shipSystems.Add(_engineSystem);
            shipSystems.Add(_weaponSystem);
            shipSystems.Add(_shieldSystem);

            var powerPerSystem = config.energyCapacity / shipSystems.Count;

            foreach (var shipSystem in shipSystems)
            {
                shipSystem.Initialize(config, this);
                shipSystem.AllocatePower(powerPerSystem);
            }
        }

        private void Update()
        {
            if (_inputState == null) return;
            if (_inputState.balancePower)
            {
                ReBalancePower();
            }
            else
            {
                if (_inputState.increaseEnginePower)
                {
                    SetPowerToSystem(_engineSystem);
                }

                if (_inputState.increaseWeaponPower)
                {
                    SetPowerToSystem(_weaponSystem);
                }

                if (_inputState.increaseShieldPower)
                {
                    SetPowerToSystem(_shieldSystem);
                }
            }

            foreach (var shipSystem in shipSystems)
            {
                shipSystem.Update(Time.deltaTime, _inputState);
            }

            _inputState = null;
        }

        private void SetPowerToSystem(IShipSystem toIncrease)
        {
            if (toIncrease.CurrentPower() < 1)
            {
                var systemsWithPowerToDraw = 0;
                foreach (var shipSystem in shipSystems)
                {
                    if (shipSystem != toIncrease && shipSystem.CurrentPower() > 0)
                    {
                        systemsWithPowerToDraw++;
                    }
                }

                var newPower = Mathf.Clamp01(toIncrease.CurrentPower() +
                                             Time.deltaTime * powerAllocationSpeed * systemsWithPowerToDraw);
                toIncrease.AllocatePower(newPower);

                foreach (var shipSystem in shipSystems)
                {
                    if (shipSystem != toIncrease && shipSystem.CurrentPower() > 0)
                    {
                        var lowerPower =
                            Mathf.Clamp01(shipSystem.CurrentPower() + Time.deltaTime * -powerAllocationSpeed);
                        shipSystem.AllocatePower(lowerPower);
                    }
                }
            }
        }

        private void ReBalancePower()
        {
            var avgPower = config.energyCapacity / shipSystems.Count;
            var systemsRequiringBalance = 0;
            var toIncrease = 0;
            var toDecrease = 0;
            foreach (var shipSystem in shipSystems)
            {
                if (Mathf.Abs(shipSystem.CurrentPower() - avgPower) > Mathf.Epsilon)
                {
                    systemsRequiringBalance++;
                    if (shipSystem.CurrentPower() > avgPower)
                    {
                        toDecrease++;
                    }
                    else
                    {
                        toIncrease++;
                    }
                }
            }

            var reBalanceSpeed = powerAllocationSpeed / systemsRequiringBalance;

            foreach (var shipSystem in shipSystems)
            {
                if (Mathf.Abs(shipSystem.CurrentPower() - avgPower) > Mathf.Epsilon)
                {
                    var rate = reBalanceSpeed * Time.deltaTime; 
                    if (shipSystem.CurrentPower() > avgPower)
                    {
                        rate /= toDecrease;
                    }
                    else
                    {
                        rate /= toIncrease;
                    }
                    var newPower = Mathf.MoveTowards(shipSystem.CurrentPower(), avgPower, rate);
                    shipSystem.AllocatePower(newPower);
                }
            }
        }

        public void SetInputState(InputState newState)
        {
            _inputState = newState;
        }

        public float PowerAllocated
        {
            get
            {
                var totalPower = 0f;
                foreach (var shipSystem in shipSystems)
                {
                    totalPower += shipSystem.CurrentPower();
                }

                return totalPower;
            }
        }
    }
}