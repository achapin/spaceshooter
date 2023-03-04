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
        private const string testTargetPath = "Assets/Prefabs/Testing/Targetbox.prefab";
        private const string testTargetShipPath = "Assets/Prefabs/Testing/TestShipDamageable.prefab";
        private AsyncOperationHandle<GameObject> shipHandle;
        private AsyncOperationHandle<GameObject> targetHandle;
        private const float sixtyFPS = 1f / 60f;

        [UnityTest]
        public IEnumerator TargetTakesDamageAndIsDestroyed()
        {
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            var testTarget = targetHandle.Result;
            var target = testTarget.GetComponent<DamageableHandler>();
            float totalDamage = 0f;
            bool wasDestroyed = false;
            target.DamageTaken += delegate(float f) { totalDamage += f; };
            target.Destroyed += delegate { wasDestroyed = true; };
            
            for (var loop = 0; loop < 240; loop++)
            {
                target.TakeDamage(.1f);
                yield return new WaitForEndOfFrame();
            }
            
            Assert.Greater(totalDamage, 0f);
            Assert.True(wasDestroyed);
        }
        
        [UnityTest]
        public IEnumerator TargetTakesDamageFromWeaponAndIsDestroyed()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            testShip.transform.position = Vector3.back * 10f;
            testShip.transform.forward = Vector3.forward;
            testTarget.transform.position = Vector3.zero;
            var ship = testShip.GetComponent<Ship>();
            var target = testTarget.GetComponent<DamageableHandler>();
            Assert.Greater(ship.PowerAllocated, 0);
            float totalDamage = 0f;
            bool wasDestroyed = false;
            target.DamageTaken += delegate(float f) { totalDamage += f; };
            target.Destroyed += delegate { wasDestroyed = true; };

            var state = new InputState
            {
                isFiring = true,
                increaseWeaponPower = true
            };

            yield return null;
            for (var loop = 0; loop < 240; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
            }
            
            Debug.Log($"Total damage {totalDamage} Was Destroyed? {wasDestroyed}");
            Assert.Greater(totalDamage, 0f);
            Assert.True(wasDestroyed);
        }

        [UnityTest]
        public IEnumerator ShipCanBeDestroyed()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetShipPath);
            yield return targetHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            testShip.transform.position = Vector3.back * 10f;
            testShip.transform.forward = Vector3.forward;
            testTarget.transform.position = Vector3.zero;
            var ship = testShip.GetComponent<Ship>();
            var target = testTarget.GetComponent<DamageableHandler>();
            Assert.Greater(ship.PowerAllocated, 0);
            float totalDamage = 0f;
            bool wasDestroyed = false;
            target.DamageTaken += delegate(float f) { totalDamage += f; };
            target.Destroyed += delegate { wasDestroyed = true; };

            var state = new InputState
            {
                isFiring = true,
                increaseWeaponPower = true
            };

            yield return null;
            for (var loop = 0; loop < 240; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
            }
            
            Debug.Log($"Total damage {totalDamage} Was Destroyed? {wasDestroyed}");
            Assert.Greater(totalDamage, 0f);
            Assert.True(wasDestroyed);
        }
    }
}
