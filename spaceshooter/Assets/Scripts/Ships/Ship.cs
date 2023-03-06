using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Damage;
using Input;
using Ships.ShipSystems;
using UnityEngine;

[assembly: InternalsVisibleTo("ShipTests")]
[assembly: InternalsVisibleTo("DamageTests")]
namespace Ships
{
    public class Ship : MonoBehaviour
    {
        [SerializeField] private float powerAllocationSpeed = 1f;

        [SerializeField] internal ShipConfig config;

        private List<IShipSystem> shipSystems;
        internal EngineSystem _engineSystem;
        internal WeaponSystem _weaponSystem;
        internal ShieldSystem _shieldSystem;

        private InputState _inputState;
        private InputListener _listener;

        internal DamageableHandler _damageableHandler;
        internal float _hp;

        void Start()
        {
            if (config == null)
            {
                throw new Exception("Ship cannot be created without a ship config");
            }

            _engineSystem = new EngineSystem();
            _weaponSystem = new WeaponSystem();
            _shieldSystem = new ShieldSystem();

            shipSystems = new List<IShipSystem>
            {
                _engineSystem,
                _weaponSystem,
                _shieldSystem
            };

            _listener = GetComponent<InputListener>();

            var powerPerSystem = config.energyCapacity / shipSystems.Count;

            foreach (var shipSystem in shipSystems)
            {
                shipSystem.Initialize(config, this);
                shipSystem.AllocatePower(powerPerSystem);
            }

            _hp = config.maxHp;
            _damageableHandler = GetComponent<DamageableHandler>();
            if(_damageableHandler != null)
            {
                _damageableHandler.DamageTaken += DamageableOnDamageTaken;
            }
            else
            {
                Debug.Log("No DamageableHandler found");
            }
        }

        private void Update()
        {
            Update(Time.deltaTime);
        }

        internal void Update(float deltaTime)
        {
            if (_listener != null)
            {
                _inputState = _listener.GetState();
            }

            if (_inputState == null) return;
            if (_inputState.balancePower)
            {
                ReBalancePower(deltaTime);
            }
            else
            {
                if (_inputState.increaseEnginePower)
                {
                    SetPowerToSystem(_engineSystem, deltaTime);
                }

                if (_inputState.increaseWeaponPower)
                {
                    SetPowerToSystem(_weaponSystem, deltaTime);
                }

                if (_inputState.increaseShieldPower)
                {
                    SetPowerToSystem(_shieldSystem, deltaTime);
                }
            }

            foreach (var shipSystem in shipSystems)
            {
                shipSystem.Update(deltaTime, _inputState);
            }

            _inputState = null;
        }

        private void SetPowerToSystem(IShipSystem toIncrease, float deltaTime)
        {
            if (toIncrease.CurrentPower() >= 1)
            {
                return;
            }

            var systemsWithPowerToDraw = 0;
            foreach (var shipSystem in shipSystems)
            {
                if (shipSystem != toIncrease && shipSystem.CurrentPower() > 0)
                {
                    systemsWithPowerToDraw++;
                }
            }

            if (systemsWithPowerToDraw <= 0)
            {
                return;
            }

            var amountToFull = 1f - toIncrease.CurrentPower();
            var maxDelta = deltaTime * powerAllocationSpeed;
            var lowerAmount =  -maxDelta / systemsWithPowerToDraw;
            if (amountToFull < maxDelta)
            {
                lowerAmount = -amountToFull / systemsWithPowerToDraw;
            }
            var amountToGive = 0f;

            foreach (var shipSystem in shipSystems)
            {
                if (shipSystem != toIncrease && shipSystem.CurrentPower() > 0)
                {
                    //Can't pull the correct proportion out, so set it to 0 and give what remained
                    if (Mathf.Abs(lowerAmount) > shipSystem.CurrentPower())
                    {
                        amountToGive = shipSystem.CurrentPower();
                        shipSystem.AllocatePower(0f);
                    }
                    else
                    {
                        amountToGive -= lowerAmount;
                        var lowerPower = Mathf.Clamp01(shipSystem.CurrentPower() + lowerAmount);
                        shipSystem.AllocatePower(lowerPower);
                    }
                }
            }

            var newPower = Mathf.Clamp01(toIncrease.CurrentPower() + amountToGive);
            toIncrease.AllocatePower(newPower);
        }

        private void ReBalancePower(float deltaTime)
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
                if (!(Mathf.Abs(shipSystem.CurrentPower() - avgPower) > Mathf.Epsilon)) continue;
                var rate = reBalanceSpeed * deltaTime;
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

        internal void LogPower()
        {
            float totalPower = 0f;
            foreach (var shipSystem in shipSystems)
            {
                totalPower += shipSystem.CurrentPower();
            }

            Debug.Log(
                $"Engine: {_engineSystem.CurrentPower()} Shields: {_shieldSystem.CurrentPower()} Weapons: {_weaponSystem.CurrentPower()} Total: {totalPower}");
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

        private void DamageableOnDamageTaken(float damageIn)
        {
            var offsetDamage = _shieldSystem.ReduceDamage(damageIn);
            _hp -= offsetDamage;
            Debug.Log($"HP now {_hp}");
            if (_hp <= 0)
            {
                _damageableHandler.DestroyDamageable();
                Debug.Log("Ship destroyed");
            }
        }
    }
}