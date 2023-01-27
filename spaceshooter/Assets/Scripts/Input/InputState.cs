using UnityEngine;

namespace Input
{
    public class InputState
    {
        public bool increaseEnginePower;
        public bool increaseWeaponPower;
        public bool increaseShieldPower;
        public bool balancePower;

        public bool isBoosting;

        //0-1
        public float throttle;
        
        //0-1/0-1
        public Vector2 joystick;
    }
}
