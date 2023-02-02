using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        
        private Vector3 _offset;

        void Start()
        {
            _offset = target.transform.position - transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            var targetPosition = target.transform.TransformPoint(_offset);
            targetPosition.y = target.transform.position.y;
            transform.position = targetPosition;
            transform.LookAt(target.transform, Vector3.up);
        }
    }
}
