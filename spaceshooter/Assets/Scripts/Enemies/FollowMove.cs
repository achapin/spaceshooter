using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class FollowMove : MonoBehaviour
    {
        [SerializeField] private GameObject toFollow;
        [SerializeField] private float speed;
        
        private List<Vector3> targetPositions = new List<Vector3>();
        
        // Start is called before the first frame update
        void Start()
        {
            targetPositions.Add(toFollow.transform.position);
        }

        // Update is called once per frame
        void Update()
        {
            if (targetPositions.Count <= 0)
            {
                return;
            }

            if (targetPositions[^1] != toFollow.transform.position)
            {
                targetPositions.Add(toFollow.transform.position);
            }
            
            var speedThisFrame = speed * Time.deltaTime;
            
            while (speedThisFrame > 0f)
            {
                var distanceToNext = Vector3.Distance(targetPositions[0], transform.position);
                if (distanceToNext < speedThisFrame)
                {
                    speedThisFrame -= distanceToNext;
                    transform.position = targetPositions[0];
                    targetPositions.RemoveAt(0);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPositions[0],
                        speedThisFrame);
                    speedThisFrame = 0f;
                }
            }
        }
    }
}
