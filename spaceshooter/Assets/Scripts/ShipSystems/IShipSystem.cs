namespace ShipSystems
{
    public interface IShipSystem
    {
        void AssignConfig(ShipConfig config);
    
        //Percentage is 0-1, representing 0-100%
        void AllocatePower(float percentage);

        void Update(float deltaTime);
    }
}
