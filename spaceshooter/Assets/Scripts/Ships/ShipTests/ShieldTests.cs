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
    public class ShieldTests
    {
        private const string testShipPath = "Assets/Prefabs/Testing/TestShipDamageable.prefab";
        private const float reasonableEpsilon = .001f;
        private AsyncOperationHandle<GameObject> handle;
        private const float sixtyFPS = 1f / 60f;

        [SetUp]
        public void OnStart()
        {
            handle = Addressables.InstantiateAsync(testShipPath);
        }
        
        [UnityTest]
        public IEnumerator ShieldsCharge()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            var state = new InputState
            {
                increaseShieldPower = true
            };
            var currentShieldStrength = ship._shieldSystem._shieldStrength;
            for (var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                if (Math.Abs(currentShieldStrength - ship.config.shieldCapacity) < reasonableEpsilon)
                {
                    Assert.True(Math.Abs(ship._shieldSystem._shieldStrength - ship.config.shieldCapacity) <
                                reasonableEpsilon);
                }
                else
                {
                    Assert.Greater(ship._shieldSystem._shieldStrength, currentShieldStrength);
                    currentShieldStrength = ship._shieldSystem._shieldStrength;
                }
            }
        }
        
        [UnityTest]
        public IEnumerator ShieldsPreventDamage()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            
            yield return null; //Force the Awake methods to run
            
            var state = new InputState
            {
                increaseShieldPower = true
            };
            for (var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
            }
            Assert.Greater(ship._shieldSystem._shieldStrength, 0f);
            var damage = ship._shieldSystem._shieldStrength / 2f; //Half to avoid edge cases
            var hpBeforeHit = ship._hp;
            ship._damageableHandler.TakeDamage(damage);
            Assert.True(Mathf.Abs(hpBeforeHit - ship._hp) < reasonableEpsilon);
        }
        
        [UnityTest]
        public IEnumerator ShieldsPreventSomeDamage()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            
            yield return null; //Force the Awake methods to run
            
            var state = new InputState
            {
                increaseShieldPower = true
            };
            for (var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
            }
            Assert.Greater(ship._shieldSystem._shieldStrength, 0f);
            var damage = ship._shieldSystem._shieldStrength * 2f;
            var hpBeforeHit = ship._hp;
            ship._damageableHandler.TakeDamage(damage);
            var hpDifference = hpBeforeHit - ship._hp;
            Assert.True(hpDifference > 0);
            Assert.True(hpDifference < damage);
        }
    }
}
