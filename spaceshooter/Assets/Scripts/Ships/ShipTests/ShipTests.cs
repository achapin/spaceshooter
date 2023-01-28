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
        private const float reasonableEpsilon = .0001f;
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
            for (var loop = 0; loop < 60; loop++)
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

            for (var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(engineState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }

            for (var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(shieldState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }

            for (var loop = 0; loop < 10; loop++)
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

            for (var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(engineState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }

            for (var loop = 0; loop < 10; loop++)
            {
                ship.SetInputState(shieldState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }

            for (var loop = 0; loop < 5; loop++)
            {
                ship.SetInputState(weaponState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
            }

            for (var loop = 0; loop < 60; loop++)
            {
                ship.SetInputState(balanceState);
                yield return null;
                Assert.Less(Math.Abs(ship.PowerAllocated - currentPower), reasonableEpsilon);
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
                yield return null;
                ship.SetInputState(throttleState);
            }

            ship.SetInputState(throttleState);
            for (var loop = 0; loop < 60; loop++)
            {
                yield return null;
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
            var transform = ship.gameObject.transform;

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
                yield return null;
                ship.SetInputState(throttleState);
            }

            var oldPosition = transform.position;
            ship.SetInputState(throttleState);
            yield return null;
            var position = transform.position;
            var oldSpeed = Vector3.Distance(position, oldPosition) / Time.deltaTime;

            //Get up to full throttle with full energy
            for (var loop = 0; loop < 1000; loop++)
            {
                yield return null;
                ship.SetInputState(engineState);
            }

            oldPosition = ship.gameObject.transform.position;
            ship.SetInputState(engineState);
            yield return null;
            position = transform.position;
            var newSpeed = Vector3.Distance(position, oldPosition) / Time.deltaTime;

            Assert.Greater(newSpeed, oldSpeed);
        }

        [UnityTest]
        public IEnumerator DecreasingEnginePowerDecreasesSpeed()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            var transform = ship.gameObject.transform;

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
                yield return null;
                ship.SetInputState(throttleState);
            }

            var oldPosition = transform.position;
            ship.SetInputState(throttleState);
            yield return null;
            var position = transform.position;
            var oldSpeed = Vector3.Distance(position, oldPosition) / Time.deltaTime;

            //Get up to full throttle with full energy
            for (var loop = 0; loop < 1000; loop++)
            {
                yield return null;
                ship.SetInputState(weaponState);
            }

            oldPosition = ship.gameObject.transform.position;
            ship.SetInputState(weaponState);
            yield return null;
            position = transform.position;
            var newSpeed = Vector3.Distance(position, oldPosition) / Time.deltaTime;

            Assert.Less(newSpeed, oldSpeed);
        }

        [UnityTest]
        public IEnumerator BoostReserveChargesAndIsDepleted()
        {
            yield return handle;
            var testShip = handle.Result;
            var ship = testShip.GetComponent<Ship>();
            var transform = testShip.transform;

            var throttleState = new InputState
            {
                throttle = 1f
            };

            float boostLevel = ship._engineSystem._boostReserve;
            ship.SetInputState(throttleState);

            for (var loop = 0; loop < 1000; loop++)
            {
                yield return null;
                ship.SetInputState(throttleState);
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
            var oldPosition = transform.position;
            var oldSpeed = 0f;
            var hasReachedBoostSpeed = false;

            for (var loop = 0; loop < 1000; loop++)
            {
                yield return null;
                var position = transform.position;
                var speed = Vector3.Distance(oldPosition, position) / Time.deltaTime;
                if (speed < ship.config.boostSpeed)
                {
                    Assert.Greater(speed, oldSpeed);
                }
                else
                {
                    hasReachedBoostSpeed = true;
                }

                ship.SetInputState(boostState);
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
            Vector3 oldEulers = transform.eulerAngles;
            
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
    }
}