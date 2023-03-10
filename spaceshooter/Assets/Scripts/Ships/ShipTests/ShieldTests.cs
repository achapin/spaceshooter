using System;
using System.Collections;
using Damage;
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
        private const string testDamageTypePath = "Assets/Data/DamageTypes/Bullet.asset";
        private const float reasonableEpsilon = .001f;
        private AsyncOperationHandle<GameObject> handle;
        private AsyncOperationHandle<DamageType> damageHandle;
        private const float sixtyFPS = 1f / 60f;

        [SetUp]
        public void OnStart()
        {
            handle = Addressables.InstantiateAsync(testShipPath);
            damageHandle = Addressables.LoadAssetAsync<DamageType>(testDamageTypePath);
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
            yield return damageHandle;
            var damageType = damageHandle.Result;
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
            ship._damageableHandler.TakeDamage(damage, damageType);
            Assert.True(Mathf.Abs(hpBeforeHit - ship._hp) < reasonableEpsilon);
        }
        
        [UnityTest]
        public IEnumerator ShieldsPreventSomeDamage()
        {
            yield return damageHandle;
            var damageType = damageHandle.Result;
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
            ship._damageableHandler.TakeDamage(damage, damageType);
            var hpDifference = hpBeforeHit - ship._hp;
            Assert.True(hpDifference > 0);
            Assert.True(hpDifference < damage);
        }
        
        [UnityTest]
        public IEnumerator ShieldsChargeAfterDamage()
        {
            yield return damageHandle;
            var damageType = damageHandle.Result;
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
            var shieldBeforeHit = ship._shieldSystem._shieldStrength;
            ship._damageableHandler.TakeDamage(damage, damageType);
            var shieldAfterHit = ship._shieldSystem._shieldStrength;
            Assert.Greater(shieldBeforeHit, shieldAfterHit);
            int framesToRecharge = Mathf.FloorToInt(ship.config.shieldRechargeTime.Evaluate(ship._shieldSystem._currentPower) / sixtyFPS);
            for (var loop = 0; loop < framesToRecharge; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                Assert.Greater(ship._shieldSystem._shieldStrength, shieldAfterHit);
                shieldAfterHit = ship._shieldSystem._shieldStrength;
            }
        }
        
        [UnityTest]
        public IEnumerator ShieldsTakeTimeToRecharge()
        {
            yield return damageHandle;
            var damageType = damageHandle.Result;
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
            var damage = ship._shieldSystem._shieldStrength * 2f; //Double to guarantee that shields are broken
            var shieldBeforeHit = ship._shieldSystem._shieldStrength;
            ship._damageableHandler.TakeDamage(damage, damageType);
            var shieldAfterHit = ship._shieldSystem._shieldStrength;
            Assert.Greater(shieldBeforeHit, shieldAfterHit);
            Assert.True(Mathf.Abs(ship._shieldSystem._shieldStrength) < reasonableEpsilon); //Shields should be 0
            int framesToRecharge = Mathf.FloorToInt(ship.config.shieldRechargeTime.Evaluate(ship._shieldSystem._currentPower) / sixtyFPS);
            for (var loop = 0; loop < framesToRecharge; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                Assert.True(Mathf.Abs(ship._shieldSystem._shieldStrength - shieldAfterHit) < reasonableEpsilon, $"Shields have started recharging at frame {loop} instead of {framesToRecharge}");
            }
            ship.SetInputState(state);
            ship.Update(sixtyFPS);
            Assert.Greater(ship._shieldSystem._shieldStrength, shieldAfterHit, $"Shields have not started charging after {framesToRecharge} update frames");
        }
    }
}
