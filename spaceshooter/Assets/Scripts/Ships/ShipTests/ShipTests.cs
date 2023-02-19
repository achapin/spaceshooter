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
        private const float sixtyFPS = 1f / 60f;

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
            for (var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(state);
                ship.Update(sixtyFPS);
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
                currentPower = ship.PowerAllocated;
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

            var lastPower = ship._engineSystem.CurrentPower();

            for (var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(engineState);
                yield return null;
                if (lastPower < 1f)
                {
                    Assert.Greater(ship._engineSystem.CurrentPower(), lastPower);
                    lastPower = ship._engineSystem.CurrentPower();
                }
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
                currentPower = ship.PowerAllocated;
            }
            
            lastPower = ship._shieldSystem.CurrentPower();

            for (var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(shieldState);
                yield return null;
                if (lastPower < 1f)
                {
                    Assert.Greater(ship._shieldSystem.CurrentPower(), lastPower);
                    lastPower = ship._shieldSystem.CurrentPower();
                }
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }
            
            lastPower = ship._weaponSystem.CurrentPower();

            for (var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(weaponState);
                yield return null;
                if (lastPower < 1f)
                {
                    Assert.Greater(ship._weaponSystem.CurrentPower(), lastPower);
                    lastPower = ship._weaponSystem.CurrentPower();
                }
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

            for (var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(engineState);
                yield return new WaitForSeconds(.001f);
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
                currentPower = ship.PowerAllocated;
            }

            for (var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(shieldState);
                yield return new WaitForSeconds(.001f);
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
                currentPower = ship.PowerAllocated;
            }

            for (var loop = 0; loop < 5; loop++)
            {
                ship.SetInputState(weaponState);
                yield return new WaitForSeconds(.001f);
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
                currentPower = ship.PowerAllocated;
            }

            for (var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(balanceState);
                yield return new WaitForSeconds(.001f);
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
                currentPower = ship.PowerAllocated;
            }
        }

        [UnityTest]
        public IEnumerator SettingThrottleUpMovesTheShip()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            var transform = ship.gameObject.transform;

            var throttleState = new InputState
            {
                throttle = 1f
            };

            var oldPosition = transform.position;

            for (var loop = 0; loop < 10; loop++)
            {
                yield return new WaitForFixedUpdate();
                ship.SetInputState(throttleState);
            }

            ship.SetInputState(throttleState);
            for (var loop = 0; loop < 60; loop++)
            {
                yield return new WaitForFixedUpdate();
                ship.SetInputState(throttleState);
                var position = ship.gameObject.transform.position;
                Assert.Greater(Vector3.Distance(oldPosition, position) / Time.deltaTime, reasonableEpsilon);
                oldPosition = position;
            }
        }

        [UnityTest]
        public IEnumerator IncreasingEnginePowerIncreasesSpeed()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();

            var throttleState = new InputState
            {
                throttle = 1f
            };

            var engineState = new InputState
            {
                increaseEnginePower = true,
                throttle = 1f
            };

            //Get up to full throttle
            for (var loop = 0; loop < 1000; loop++)
            {
                ship._engineSystem.Update(sixtyFPS, throttleState);
            }

            var oldSpeed = ship._engineSystem._currentSpeed;
            Assert.Greater(oldSpeed, 0f);

            //Get up to full throttle with full energy
            for (var loop = 0; loop < 1000; loop++)
            {
                ship.SetInputState(engineState);
                ship.Update(sixtyFPS);
                ship.LogPower();
                Debug.Log($"Speed {ship._engineSystem._currentSpeed}");
            }
            
            var newSpeed = ship._engineSystem._currentSpeed;

            Assert.Greater(newSpeed, oldSpeed);
        }

        [UnityTest]
        public IEnumerator DecreasingEnginePowerDecreasesSpeed()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();

            var throttleState = new InputState
            {
                throttle = 1f
            };

            var weaponState = new InputState
            {
                increaseWeaponPower = true,
                throttle = 1f
            };

            //Get up to full throttle
            for (var loop = 0; loop < 1000; loop++)
            {
                ship._engineSystem.Update(sixtyFPS, throttleState);
            }
            
            var oldSpeed = ship._engineSystem._currentSpeed;
            Debug.Log($"Old speed {oldSpeed}");

            //Get up to full throttle with full energy
            for (var loop = 0; loop < 1000; loop++)
            {
                ship.SetInputState(weaponState);
                ship.Update(sixtyFPS);
                ship.LogPower();
                Debug.Log($"Speed {ship._engineSystem._currentSpeed}");
            }
            
            var newSpeed = ship._engineSystem._currentSpeed;

            Assert.Less(newSpeed, oldSpeed);
        }

        [UnityTest]
        public IEnumerator BoostReserveChargesAndIsDepleted()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();

            var throttleState = new InputState
            {
                throttle = 1f
            };

            float boostLevel = ship._engineSystem._boostReserve;
            
            for (var loop = 0; loop < 100; loop++)
            {
                ship._engineSystem.Update(sixtyFPS, throttleState);
                if (boostLevel < ship.config.boostCapacity)
                {
                    Assert.Greater(ship._engineSystem._boostReserve, boostLevel);
                    boostLevel = ship._engineSystem._boostReserve;
                }
                else
                {
                    break;
                }
            }

            Assert.LessOrEqual(Mathf.Abs(ship._engineSystem._boostReserve - boostLevel), reasonableEpsilon);

            var boostState = new InputState
            {
                throttle = 1f,
                isBoosting = true
            };

            ship.SetInputState(boostState);
            var oldSpeed = ship._engineSystem._currentSpeed;
            var hasReachedBoostSpeed = false;
            boostLevel = ship._engineSystem._boostReserve;

            for (var loop = 0; loop < 100; loop++)
            {
                ship._engineSystem.Update(sixtyFPS, boostState);
                var speed = ship._engineSystem._currentSpeed;
                if (speed < ship.config.boostSpeed && !hasReachedBoostSpeed)
                {
                    Assert.Greater(speed, oldSpeed);
                    oldSpeed = speed;
                }
                else
                {
                    hasReachedBoostSpeed = true;
                }

                if (boostLevel > 0)
                {
                    Assert.Less(ship._engineSystem._boostReserve, boostLevel);
                    boostLevel = ship._engineSystem._boostReserve;
                }
                else
                {
                    break;
                }
            }

            Assert.LessOrEqual(boostLevel, 0f);
            Assert.True(hasReachedBoostSpeed);
        }
        
        [UnityTest]
        public IEnumerator ChangingJoystickRotatesShip()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            var transform = testShip.transform;
            
            var throttleState = new InputState
            {
                throttle = 1f
            };

            var rotateState = new InputState
            {
                throttle = 1f,
                joystick = new Vector2(1f, 0f)
            };
            
            ship.SetInputState(throttleState);
            var oldEulers = transform.eulerAngles;
            
            for (var loop = 0; loop < 100; loop++)
            {
                yield return null;
                ship.SetInputState(throttleState);
                var eulers = transform.eulerAngles;
                Assert.AreEqual(oldEulers, eulers);
                oldEulers = eulers;
            }
            
            ship.SetInputState(rotateState);

            for (var loop = 0; loop < 100; loop++)
            {
                yield return null;
                ship.SetInputState(rotateState);
                var eulers = transform.eulerAngles;
                Assert.AreNotEqual(oldEulers, eulers);
                oldEulers = eulers;
            }
        }

        [UnityTest]
        public IEnumerator WeaponSystemChargesOverTime()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            var weaponSystem = ship._weaponSystem;
            var weaponSystemCharge = weaponSystem._chargeLevel;
            
            var powerState = new InputState
            {
                increaseWeaponPower = true
            };
            
            ship.SetInputState(powerState);

            for (var loop = 0; loop < 100; loop++)
            {
                yield return null;
                Assert.Greater(weaponSystem._chargeLevel, weaponSystemCharge);
                weaponSystemCharge = weaponSystem._chargeLevel;
                if (weaponSystemCharge >= 2f)
                {
                    break;
                }
                ship.SetInputState(powerState);
            }
        }
    }
}