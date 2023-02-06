using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        
        private Vector3 _offset;
        private float _yOffset;

        void Start()
        {
            _offset = target.transform.position - transform.position;
            _yOffset = -_offset.y;
            _offset.y = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            var targetPosition = target.transform.TransformPoint(_offset);
            targetPosition.y = target.transform.position.y + _yOffset;
            transform.position = targetPosition;
            transform.LookAt(target.transform, Vector3.up);
        }
    }
}
