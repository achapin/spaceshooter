using Input;

namespace Ships.ShipSystems
{
    public class WeaponSystem : IShipSystem
    {
        private ShipConfig _config;
        private Ship _ship;

        private float _currentPower;
        
        public void Initialize(ShipConfig config, Ship ship)
        {
            _config = config;
            _ship = ship;
        }

        public void AllocatePower(float percentage)
        {
            _currentPower = percentage;
        }

        public void Update(float deltaTime, InputState inputState)
        {
            //TODO: Implement
        }

        public float CurrentPower()
        {
            return _currentPower;
        }
    }
}