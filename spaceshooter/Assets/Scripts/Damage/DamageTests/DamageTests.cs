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
        private const string testTargetShipLaserPath = "Assets/Prefabs/Testing/TestShipDamageableLaser.prefab";
        private const string testTargetShip0ShieldBulletPath = "Assets/Prefabs/Testing/TestShipDamageable0ShieldBullet.prefab";
        private const string testTargetShip0ShieldLaserPath = "Assets/Prefabs/Testing/TestShipDamageable0ShieldLaser.prefab";
        private const string testDamageTypePath = "Assets/Data/DamageTypes/Bullet.asset";
        private AsyncOperationHandle<GameObject> shipHandle;
        private AsyncOperationHandle<GameObject> targetHandle;
        private AsyncOperationHandle<DamageType> damageHandle;
        private const float sixtyFPS = 1f / 60f;

        [SetUp]
        public void Setup()
        {
            damageHandle = Addressables.LoadAssetAsync<DamageType>(testDamageTypePath);
        }

        [UnityTest]
        public IEnumerator TargetTakesDamageAndIsDestroyed()
        {
            yield return damageHandle;
            var damageType = damageHandle.Result;
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            var testTarget = targetHandle.Result;
            var target = testTarget.GetComponent<DamageableHandler>();
            float totalDamage = 0f;
            bool wasDestroyed = false;
            target.DamageTaken += delegate(float f, DamageType _) { totalDamage += f; };
            target.Destroyed += delegate { wasDestroyed = true; };
            
            for (var loop = 0; loop < 240; loop++)
            {
                target.TakeDamage(.1f, damageType);
                yield return new WaitForEndOfFrame();
            }
            
            Assert.Greater(totalDamage, 0f);
            Assert.True(wasDestroyed);
        }
        
        [UnityTest]
        public IEnumerator TargetTakesDamageAndIsDestroyedByAWeapon()
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
            target.DamageTaken += delegate(float f, DamageType _) { totalDamage += f; };
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
            target.DamageTaken += delegate(float f, DamageType _) { totalDamage += f; };
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
        public IEnumerator LasersAreMoreEffectiveAgainstShields()
        {
            shipHandle = Addressables.InstantiateAsync(testTargetShipPath);
            var laserShipHandle = Addressables.InstantiateAsync(testTargetShipLaserPath);
            yield return laserShipHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testLaserShip = laserShipHandle.Result;
            testShip.transform.position = Vector3.back * 10f;
            testShip.transform.forward = Vector3.forward;
            testLaserShip.transform.position = Vector3.zero;
            testLaserShip.transform.forward = Vector3.back;
            var bulletShip = testShip.GetComponent<Ship>();
            var laserShip = testLaserShip.GetComponent<Ship>();
            Assert.Greater(bulletShip.PowerAllocated, 0);
            Assert.Greater(laserShip.PowerAllocated, 0);

            var waitState = new InputState
            {
                increaseWeaponPower = true
            };
            
            //Give the shields some time to charge
            bulletShip.SetInputState(waitState);
            bulletShip.Update(sixtyFPS * 100f);
            laserShip.SetInputState(waitState);
            laserShip.Update(sixtyFPS * 100f);
            
            Assert.Greater(bulletShip._shieldSystem._shieldStrength, 0);
            Assert.Greater(laserShip._shieldSystem._shieldStrength, 0);
            
            var firingState = new InputState
            {
                isFiring = true,
                increaseWeaponPower = true
            };
            
            yield return null;

            var laserShipPreHealth = laserShip._hp + laserShip._shieldSystem._shieldStrength;
            
            bulletShip.SetInputState(firingState);
            bulletShip.Update(sixtyFPS);
            
            var laserShipAfterHealth = laserShip._hp + laserShip._shieldSystem._shieldStrength;

            float bulletDamage = laserShipPreHealth - laserShipAfterHealth;
            //If this isn't before the laser ship update, then the laser ship update will cause it to regen shields, and set the bullet damage to 0
            
            var bulletShipPreHealth = bulletShip._hp + bulletShip._shieldSystem._shieldStrength;
            
            laserShip.SetInputState(firingState);
            laserShip.Update(sixtyFPS);
            
            var bulletShipAfterHealth = bulletShip._hp + bulletShip._shieldSystem._shieldStrength;

            float laserDamage = bulletShipPreHealth - bulletShipAfterHealth;

            Assert.Greater(laserDamage, 0f, $"Laser damage was only {laserDamage}");
            Assert.Greater(bulletDamage, 0f, $"Bullet damage was only {bulletDamage}");
            Assert.Greater(laserDamage, bulletDamage); //Since lasers do more damage than bullets against shields, this should be true

        }
        
        [UnityTest]
        public IEnumerator BulletAreMoreEffectiveAgainstHull()
        {
            shipHandle = Addressables.InstantiateAsync(testTargetShip0ShieldBulletPath);
            var laserShipHandle = Addressables.InstantiateAsync(testTargetShip0ShieldLaserPath);
            yield return laserShipHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testLaserShip = laserShipHandle.Result;
            testShip.transform.position = Vector3.back * 10f;
            testShip.transform.forward = Vector3.forward;
            testLaserShip.transform.position = Vector3.zero;
            testLaserShip.transform.forward = Vector3.back;
            var bulletShip = testShip.GetComponent<Ship>();
            var laserShip = testLaserShip.GetComponent<Ship>();
            Assert.Greater(bulletShip.PowerAllocated, 0);
            Assert.Greater(laserShip.PowerAllocated, 0);

            var firingState = new InputState
            {
                isFiring = true,
                increaseWeaponPower = true
            };
            
            yield return null;

            var laserShipPreHealth = laserShip._hp + laserShip._shieldSystem._shieldStrength;
            
            bulletShip.SetInputState(firingState);
            bulletShip.Update(sixtyFPS);
            
            var laserShipAfterHealth = laserShip._hp + laserShip._shieldSystem._shieldStrength;

            float bulletDamage = laserShipPreHealth - laserShipAfterHealth;

            var bulletShipPreHealth = bulletShip._hp + bulletShip._shieldSystem._shieldStrength;
            
            laserShip.SetInputState(firingState);
            laserShip.Update(sixtyFPS);
            
            var bulletShipAfterHealth = bulletShip._hp + bulletShip._shieldSystem._shieldStrength;

            float laserDamage = bulletShipPreHealth - bulletShipAfterHealth;

            Assert.Greater(laserDamage, 0f, $"Laser damage was only {laserDamage}");
            Assert.Greater(bulletDamage, 0f, $"Bullet damage was only {bulletDamage}");
            Assert.Greater(bulletDamage, laserDamage, $"Bullet damage was {bulletDamage} which should have been > than laser damage {laserDamage}"); //Since the shields haven't charged yet, the bullets should do more damage to the hull
        }
    }
}
