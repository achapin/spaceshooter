namespace ShipSystems
{
    public interface IShipSystem
    {
        void Initialize(ShipConfig config, Ship ship);
    
        //Percentage is 0-1, representing 0-100%
        void AllocatePower(float percentage);

        void Update(float deltaTime);
    }
}
