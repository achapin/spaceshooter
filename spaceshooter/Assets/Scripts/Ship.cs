using System;
using System.Collections.Generic;
using ShipSystems;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private float powerAllocationSpeed = 1f;

    private ShipConfig config;

    private List<IShipSystem> shipSystems;
    private EngineSystem _engineSystem;
    private InputState _inputState;

    void Start()
    {
        if (config == null)
        {
            throw new Exception("Ship cannot be created without a ship config");
        }

        _engineSystem = new EngineSystem();

        shipSystems.Add(_engineSystem);

        var powerPerSystem = config.energyCapacity / shipSystems.Count;

        foreach (var shipSystem in shipSystems)
        {
            shipSystem.Initialize(config, this);
            shipSystem.AllocatePower(powerPerSystem);
        }
    }

    private void Update()
    {
        if (_inputState != null)
        {
            if (_inputState.increaseEnginePower && _engineSystem.CurrentPower() < 1)
            {
                var newPower = Mathf.Clamp01(_engineSystem.CurrentPower() + Time.deltaTime * powerAllocationSpeed);
                _engineSystem.AllocatePower(newPower);
            }
            else if (_inputState.increaseWeaponPower || _inputState.increaseShieldPower)
            {
                var newPower = Mathf.Clamp01(_engineSystem.CurrentPower() + Time.deltaTime * -powerAllocationSpeed);
                _engineSystem.AllocatePower(newPower);
            }

            foreach (var shipSystem in shipSystems)
            {
                shipSystem.Update(Time.deltaTime, _inputState);
            }
        }
    }
}