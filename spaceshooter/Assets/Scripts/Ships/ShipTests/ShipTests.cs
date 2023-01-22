using System;
using System.Collections;
using Input;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;

namespace Ships.ShipTests
{
    public class ShipTests
    {
        private const string testShipPath = "Assets/Prefabs/Testing/TestShip.prefab";
        private const float reasonableEpsilon = .001f;
        private AsyncOperationHandle<GameObject> handle;
        
        [SetUp]
        public void OnStart()
        {
            handle = Addressables.InstantiateAsync(testShipPath);
        }
        
        [UnityTest]
        public IEnumerator ShipCanBeInstantiated()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            Assert.Greater(ship.PowerAllocated, 0);
        }
        

        [UnityTest]
        public IEnumerator ShipPowerRemainsConstant()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            Assert.Greater(ship.PowerAllocated, 0);
            var currentPower = ship.PowerAllocated;
            var state = new InputState
            {
                increaseEnginePower = true
            };
            for(var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(state);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }
        }
        
        [UnityTest]
        public IEnumerator ShipPowerRemainsConstantForAllSystems()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            Assert.Greater(ship.PowerAllocated, 0);
            var currentPower = ship.PowerAllocated;
            var engineState = new InputState
            {
                increaseEnginePower = true
            };
            
            var shieldState = new InputState
            {
                increaseShieldPower = true
            };
            
            var weaponState = new InputState
            {
                increaseWeaponPower = true
            };

            for(var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(engineState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }
            
            for(var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(shieldState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }
            
            for(var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(weaponState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }
        }
        
        [UnityTest]
        public IEnumerator ShipPowerRemainsConstantDuringReBalance()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            Assert.Greater(ship.PowerAllocated, 0);
            var currentPower = ship.PowerAllocated;
            var engineState = new InputState
            {
                increaseEnginePower = true
            };
            
            var shieldState = new InputState
            {
                increaseShieldPower = true
            };
            
            var weaponState = new InputState
            {
                increaseWeaponPower = true
            };
            
            var balanceState = new InputState
            {
                balancePower = true
            };
            
            for(var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(engineState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }
            
            for(var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(shieldState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }
            
            for(var loop = 0; loop < 5; loop++)
            {
                ship.SetInputState(weaponState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }
            
            for(var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(balanceState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }
        }
    }
}
