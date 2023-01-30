using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputListener : MonoBehaviour
    {
        private InputState _inputState = new InputState();

        void Start()
        {
            var playerInput = gameObject.GetComponent<PlayerInput>();
            playerInput.onActionTriggered += PlayerInputOnActionTriggered;
        }

        private void PlayerInputOnActionTriggered(InputAction.CallbackContext obj)
        {
            Debug.Log($"{obj.action} {obj.action.name} {obj.phase}");
            //TODO: How to map the inputactions to the InputState...
        }

        InputState GetState()
        {
            return _inputState;
        }
    }
}
