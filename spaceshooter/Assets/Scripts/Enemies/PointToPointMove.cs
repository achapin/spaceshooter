using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class PointToPointMove : MonoBehaviour
    {
        [SerializeField] private List<Vector3> targetPositions;
        [SerializeField] private float speed;

        private int targetPositionIndex;
        
        void Update()
        {
            if (targetPositions.Count <= 0)
            {
                return;
            }
            
            var speedThisFrame = speed * Time.deltaTime;
            
            while (speedThisFrame > 0f)
            {
                var distanceToNext = Vector3.Distance(targetPositions[targetPositionIndex], transform.position);
                if (distanceToNext < speedThisFrame)
                {
                    speedThisFrame -= distanceToNext;
                    transform.position = targetPositions[targetPositionIndex];
                    targetPositionIndex++;
                    if (targetPositionIndex >= targetPositions.Count)
                    {
                        targetPositionIndex = 0;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPositions[targetPositionIndex],
                        speedThisFrame);
                    speedThisFrame = 0f;
                }
            }
        }
    }
}
