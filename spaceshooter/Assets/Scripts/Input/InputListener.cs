using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputListener : MonoBehaviour
    {
        private InputState _inputState = new InputState();

        private float _throttleDelta = 0f;

        void Start()
        {
            var playerInput = gameObject.GetComponent<PlayerInput>();
            playerInput.onActionTriggered += PlayerInputOnActionTriggered;
        }

        private void PlayerInputOnActionTriggered(InputAction.CallbackContext obj)
        {
            //Debug.Log($"{obj.action} {obj.action.name} {obj.phase}");
            switch (obj.action.name)
            {
                case "Joystick":
                    if (obj.phase == InputActionPhase.Performed)
                    {
                        _inputState.joystick = obj.ReadValue<Vector2>();
                    }
                    else if (obj.phase == InputActionPhase.Canceled)
                    {
                        _inputState.joystick = Vector2.zero;
                    }

                    break;
                case "Throttle":
                    if (obj.phase == InputActionPhase.Performed)
                    {
                        _throttleDelta = obj.ReadValue<float>();
                    }
                    else if (obj.phase == InputActionPhase.Canceled)
                    {
                        _throttleDelta = 0f;
                    }

                    break;
                case "Boost":
                    if (obj.performed)
                    {
                        _inputState.isBoosting = true;
                    }
                    else if (obj.canceled)
                    {
                        _inputState.isBoosting = false;
                    }

                    break;
                case "PowerBalance":
                    if (obj.performed)
                    {
                        _inputState.balancePower = true;
                    }
                    else
                    {
                        _inputState.balancePower = false;
                    }
                    break;
                case "PowerIncreaseEngineSystem":
                    if (obj.performed)
                    {
                        _inputState.increaseEnginePower = true;
                    }
                    else
                    {
                        _inputState.increaseEnginePower = false;
                    }
                    break;
                case "PowerIncreaseShieldSystem":
                    if (obj.performed)
                    {
                        _inputState.increaseShieldPower = true;
                    }
                    else
                    {
                        _inputState.increaseShieldPower = false;
                    }
                    break;
                case "PowerIncreaseWeaponSystem":
                    if (obj.performed)
                    {
                        _inputState.increaseWeaponPower = true;
                    }
                    else
                    {
                        _inputState.increaseWeaponPower = false;
                    }
                    break;
                case "FirePrimary":
                    if (obj.performed)
                    {
                        _inputState.isFiring = true;
                    }
                    else
                    {
                        _inputState.isFiring = false;
                    }
                    break;
            }
        }

        void Update()
        {
            _inputState.throttle = Mathf.Clamp01(_inputState.throttle + _throttleDelta * Time.deltaTime);
        }

        public InputState GetState()
        {
            return _inputState;
        }
    }
}