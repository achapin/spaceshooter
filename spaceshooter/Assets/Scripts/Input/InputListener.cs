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
            playerInput.onActionTriggered += PlayerInputOnonActionTriggered;
        }

        private void PlayerInputOnonActionTriggered(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();
        }

        private void LateUpdate()
        {
            throw new NotImplementedException();
        }

        InputState GetState()
        {
            return _inputState;
        }
    }
}
