using System.Collections;
using Damage;
using Input;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;

namespace Ships.ShipSystems.Weapons.WeaponTests
{
    public class WeaponTests : MonoBehaviour
    {
        private const string testShipPath = "Assets/Prefabs/Testing/TestShipLockOnWeapon.prefab";
        private const string testTargetPath = "Assets/Prefabs/Testing/Targetbox.prefab";
        
        private const float sixtyFPS = 1f / 60f;
        
        private AsyncOperationHandle<GameObject> shipHandle;
        private AsyncOperationHandle<GameObject> targetHandle;

        [UnityTest]
        public IEnumerator LockOnWeaponLocksOn()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            var ship = testShip.GetComponent<Ship>();
            
            testShip.transform.position = Vector3.zero;
            testTarget.transform.position = Vector3.forward * 5f;

            yield return null;
            
            Assert.IsNotNull(ship);
            Assert.IsNotNull(ship._weaponSystem);
            var weapon = ship._weaponSystem._weapons[0];
            Assert.IsNotNull(weapon);
            var lockOnWeapon = weapon as LockOnWeapon;
            Assert.IsNotNull(lockOnWeapon);
            
            var state = new InputState
            {
                isFiring = true
            };

            yield return null;
            for (var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
            }
            
            Assert.IsNotNull(lockOnWeapon.target);
            Assert.AreSame(testTarget, lockOnWeapon.target.gameObject);
        }
        
        [UnityTest]
        public IEnumerator LockOnWeaponLocksOnOverTime()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            var ship = testShip.GetComponent<Ship>();
            
            testShip.transform.position = Vector3.zero;
            testTarget.transform.position = Vector3.forward * 5f;

            yield return null;
            
            Assert.IsNotNull(ship);
            Assert.IsNotNull(ship._weaponSystem);
            var weapon = ship._weaponSystem._weapons[0];
            Assert.IsNotNull(weapon);
            var lockOnWeapon = weapon as LockOnWeapon;
            Assert.IsNotNull(lockOnWeapon);
            
            var state = new InputState
            {
                isFiring = true
            };

            var lastPoints = lockOnWeapon.lockPoints;
            Assert.AreEqual(0f, lastPoints);

            yield return null;
            for (var loop = 0; loop < 100; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                var newPoints = lockOnWeapon.lockPoints;
                if(lastPoints < lockOnWeapon.lockOnRequired)
                {
                    Assert.Greater(newPoints, lastPoints);
                }
                lastPoints = newPoints;
            }
            
            Assert.GreaterOrEqual(lastPoints, lockOnWeapon.lockOnRequired);
        }
        
        [UnityTest]
        public IEnumerator LockOnWeaponDoesntLockOnIfTheTargetIsBehindTheShip()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            var ship = testShip.GetComponent<Ship>();
            
            testShip.transform.position = Vector3.zero;
            testTarget.transform.position = Vector3.back * 5f;

            yield return null;
            
            Assert.IsNotNull(ship);
            Assert.IsNotNull(ship._weaponSystem);
            var weapon = ship._weaponSystem._weapons[0];
            Assert.IsNotNull(weapon);
            var lockOnWeapon = weapon as LockOnWeapon;
            Assert.IsNotNull(lockOnWeapon);
            
            var state = new InputState
            {
                isFiring = true
            };

            yield return null;
            for (var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
            }
            
            Assert.IsNull(lockOnWeapon.target);
        }
        
        [UnityTest]
        public IEnumerator LockOnWeaponLocksOnFasterWithCloserAngle()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            var ship = testShip.GetComponent<Ship>();
            
            testShip.transform.position = Vector3.zero;
            testTarget.transform.position = Vector3.forward * 5f;

            yield return null;
            
            Assert.IsNotNull(ship);
            Assert.IsNotNull(ship._weaponSystem);
            var weapon = ship._weaponSystem._weapons[0];
            Assert.IsNotNull(weapon);
            var lockOnWeapon = weapon as LockOnWeapon;
            Assert.IsNotNull(lockOnWeapon);
            
            var state = new InputState
            {
                isFiring = true
            };

            float timeToChargeAhead = 0f;
            bool chargedFullyAhead = false;
            var lastPoints = lockOnWeapon.lockPoints;
            Assert.AreEqual(0f, lastPoints);

            yield return null;
            for (var loop = 0; loop < 1000; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                var newPoints = lockOnWeapon.lockPoints;
                if(lastPoints < lockOnWeapon.lockOnRequired)
                {
                    Assert.Greater(newPoints, lastPoints);
                }
                lastPoints = newPoints;
                if (newPoints >= lockOnWeapon.lockOnRequired)
                {
                    loop = 1000;
                    chargedFullyAhead = true;
                }
                else
                {
                    timeToChargeAhead += sixtyFPS;
                }
            }
            
            Assert.True(chargedFullyAhead);
            
            ship.SetInputState(new InputState());
            ship.Update(sixtyFPS); //Release, and fire
            ship.SetInputState(new InputState());
            ship.Update(3.1f); //Go through cooldown

            float timeToChargeOffset = 0f;
            bool chargedOffset = false;
            lastPoints = lockOnWeapon.lockPoints;
            Assert.AreEqual(0f, lastPoints);
            testTarget.transform.position = Vector3.forward * 5f + Vector3.left;

            for (var loop = 0; loop < 1000; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                var newPoints = lockOnWeapon.lockPoints;
                if(lastPoints < lockOnWeapon.lockOnRequired)
                {
                    Assert.Greater(newPoints, lastPoints);
                }
                lastPoints = newPoints;
                if (newPoints >= lockOnWeapon.lockOnRequired)
                {
                    loop = 1000;
                    chargedOffset = true;
                }
                else
                {
                    timeToChargeOffset += sixtyFPS;
                }
            }
            
            Assert.True(chargedOffset);
            Assert.Greater(timeToChargeOffset, timeToChargeAhead);
        }
        
        [UnityTest]
        public IEnumerator LockOnWeaponLocksDoesntDoDamageIfReleasedEarly()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            var ship = testShip.GetComponent<Ship>();
            
            testShip.transform.position = Vector3.zero;
            testTarget.transform.position = Vector3.forward * 5f;

            var target = testTarget.GetComponent<DamageableHandler>();
            Assert.IsNotNull(target);
            float damageTaken = 0f;
            target.DamageTaken += (f, type) => damageTaken += f; 

            yield return null;
            
            Assert.IsNotNull(ship);
            Assert.IsNotNull(ship._weaponSystem);
            var weapon = ship._weaponSystem._weapons[0];
            Assert.IsNotNull(weapon);
            var lockOnWeapon = weapon as LockOnWeapon;
            Assert.IsNotNull(lockOnWeapon);
            
            var state = new InputState
            {
                isFiring = true
            };

            var lastPoints = lockOnWeapon.lockPoints;
            Assert.AreEqual(0f, lastPoints);

            yield return null;
            for (var loop = 0; loop < 3; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                var newPoints = lockOnWeapon.lockPoints;
                if(lastPoints < lockOnWeapon.lockOnRequired)
                {
                    Assert.Greater(newPoints, lastPoints);
                }
                lastPoints = newPoints;
            }
            
            Assert.Less(lastPoints, lockOnWeapon.lockOnRequired);
            
            ship.SetInputState(new InputState());
            ship.Update(sixtyFPS); //Release, before damage should be done

            Assert.AreEqual(0f, damageTaken);
            Assert.Greater(lockOnWeapon.cooldown, 0f); //The weapon is on cooldown
        }
        
        [UnityTest]
        public IEnumerator LockOnWeaponLocksDoesDamageIfHeldLongEnough()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            var ship = testShip.GetComponent<Ship>();
            
            testShip.transform.position = Vector3.zero;
            testTarget.transform.position = Vector3.forward * 5f;

            var target = testTarget.GetComponent<DamageableHandler>();
            Assert.IsNotNull(target);
            float damageTaken = 0f;
            target.DamageTaken += (f, type) => damageTaken += f; 

            yield return null;
            
            Assert.IsNotNull(ship);
            Assert.IsNotNull(ship._weaponSystem);
            var weapon = ship._weaponSystem._weapons[0];
            Assert.IsNotNull(weapon);
            var lockOnWeapon = weapon as LockOnWeapon;
            Assert.IsNotNull(lockOnWeapon);
            
            var state = new InputState
            {
                isFiring = true
            };

            var lastPoints = lockOnWeapon.lockPoints;
            Assert.AreEqual(0f, lastPoints);

            yield return null;
            for (var loop = 0; loop < 1000; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                var newPoints = lockOnWeapon.lockPoints;
                if(lastPoints < lockOnWeapon.lockOnRequired)
                {
                    Assert.Greater(newPoints, lastPoints);
                }
                lastPoints = newPoints;
                if (newPoints >= lockOnWeapon.lockOnRequired)
                {
                    loop = 1001;
                }
            }
            
            Assert.GreaterOrEqual(lastPoints, lockOnWeapon.lockOnRequired);
            
            ship.SetInputState(new InputState());
            ship.Update(sixtyFPS); //Release, which should do damage

            Assert.Greater(damageTaken, 0f);
            Assert.Greater(lockOnWeapon.cooldown, 0f); //The weapon is on cooldown
        }
        
        [UnityTest]
        public IEnumerator LockOnWeaponCooldownIsHigherIfFired()
        {
            shipHandle = Addressables.InstantiateAsync(testShipPath);
            targetHandle = Addressables.InstantiateAsync(testTargetPath);
            yield return targetHandle;
            yield return shipHandle;
            var testShip = shipHandle.Result;
            var testTarget = targetHandle.Result;
            var ship = testShip.GetComponent<Ship>();
            
            testShip.transform.position = Vector3.zero;
            testTarget.transform.position = Vector3.forward * 5f;

            var target = testTarget.GetComponent<DamageableHandler>();
            Assert.IsNotNull(target);
            float damageTaken = 0f;
            target.DamageTaken += (f, type) => damageTaken += f; 

            yield return null;
            
            Assert.IsNotNull(ship);
            Assert.IsNotNull(ship._weaponSystem);
            var weapon = ship._weaponSystem._weapons[0];
            Assert.IsNotNull(weapon);
            var lockOnWeapon = weapon as LockOnWeapon;
            Assert.IsNotNull(lockOnWeapon);
            
            var state = new InputState
            {
                isFiring = true
            };
            
            var lastPoints = lockOnWeapon.lockPoints;
            Assert.AreEqual(0f, lastPoints);

            yield return null;
            for (var loop = 0; loop < 3; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                var newPoints = lockOnWeapon.lockPoints;
                if(lastPoints < lockOnWeapon.lockOnRequired)
                {
                    Assert.Greater(newPoints, lastPoints);
                }
                lastPoints = newPoints;
            }
            
            Assert.Less(lastPoints, lockOnWeapon.lockOnRequired);
            
            ship.SetInputState(new InputState());
            ship.Update(sixtyFPS); //Release, before damage should be done

            Assert.AreEqual(0f, damageTaken);
            Assert.Greater(lockOnWeapon.cooldown, 0f); //The weapon is on cooldown
            var releaseCooldown = lockOnWeapon.cooldown;
            
            ship.SetInputState(new InputState());
            ship.Update(releaseCooldown + .1f); //Clear the cooldown before trying again
            
            Assert.LessOrEqual(lockOnWeapon.cooldown, 0f);

            lastPoints = lockOnWeapon.lockPoints;
            Assert.AreEqual(0f, lastPoints);
            
            for (var loop = 0; loop < 1000; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                var newPoints = lockOnWeapon.lockPoints;
                if(lastPoints < lockOnWeapon.lockOnRequired)
                {
                    Assert.Greater(newPoints, lastPoints);
                }
                lastPoints = newPoints;
                if (newPoints >= lockOnWeapon.lockOnRequired)
                {
                    loop = 1001;
                }
            }
            
            Assert.GreaterOrEqual(lastPoints, lockOnWeapon.lockOnRequired);
            
            ship.SetInputState(new InputState());
            ship.Update(sixtyFPS); //Release, which should do damage

            Assert.Greater(damageTaken, 0f);
            Assert.Greater(lockOnWeapon.cooldown, 0f); //The weapon is on cooldown
            var fireCooldown = lockOnWeapon.cooldown;
            Assert.Greater(fireCooldown, releaseCooldown);
        }
    }
}
