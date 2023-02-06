using Input;
using UnityEngine;

namespace Ships.Display
{
    public class Banker : MonoBehaviour
    {
        [SerializeField] private float _restitutionSpeed;

        [SerializeField] private float _maxAngle;

        private InputListener _inputListener;

        // Start is called before the first frame update
        void Start()
        {
            _inputListener = GetComponentInParent<InputListener>();
        }

        // Update is called once per frame
        void Update()
        {
            var currentState = _inputListener.GetState();
            var targetRotation = currentState.joystick.x * -_maxAngle;
            transform.localEulerAngles = Vector3.forward * Mathf.MoveTowardsAngle(transform.localEulerAngles.z,
                targetRotation, _restitutionSpeed * Time.deltaTime);
        }
    }
}