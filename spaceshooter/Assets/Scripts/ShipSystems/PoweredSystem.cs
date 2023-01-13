using UnityEngine;

namespace ShipSystems
{
    public class PoweredSystem : IShipSystem
    {
        private float assignedPower;
        private float totalPower;
    
        public void AssignConfig(ShipConfig config)
        {
            totalPower = config.energyCapacity;
        }

        public void AllocatePower(float percentage)
        {
            assignedPower = percentage;
            Debug.Log($"This system is at { percentage / totalPower } power");
        }

        public void Update(float deltaTime)
        {
            Debug.Log($"Updating at { assignedPower } power");
        }
    }
}
