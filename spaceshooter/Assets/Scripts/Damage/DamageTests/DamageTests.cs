using System;
using System.Collections;
using Input;
using NUnit.Framework;
using Ships;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;

namespace Damage.DamageTests
{
    public class DamageTests : MonoBehaviour
    {
        private const string testShipPath = "Assets/Prefabs/Testing/TestShip.prefab";
        private const string testTargetPath = "AAssets/Prefabs/Testing/Targetbox.prefab";
        private const float reasonableEpsilon = .001f;
        private AsyncOperationHandle<GameObject> shipHandle;
        private AsyncOperationHandle<GameObject> targetHandle;
        private const float sixtyFPS = 1f / 60f;

        [SetUp]
        public void OnStart()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetPath, transform.forward * 20f, Quaternion.identity);
        }

        [UnityTest]
        public IEnumerator TargetTakesDamageAndIsDestroyed()
        {
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            var ship = testShip.GetComponent<Ship>();
            var target = testTarget.GetComponent<DamageableHandler>();
            Assert.Greater(ship.PowerAllocated, 0);
            float totalDamage = 0f;
            bool wasDestroyed = false;
            target.damageable.DamageTaken += delegate(float f) { totalDamage += f; };
            target.damageable.Destroyed += delegate { wasDestroyed = true; };
            
            var state = new InputState
            {
                isFiring = true,
                increaseWeaponPower = true
            };
            for (var loop = 0; loop < 240; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
            }
            
            Assert.Greater(totalDamage, 0f);
            Assert.True(wasDestroyed);
        }
    }
}
