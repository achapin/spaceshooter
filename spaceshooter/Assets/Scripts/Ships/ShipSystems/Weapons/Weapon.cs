
using Input;
using UnityEngine;

namespace Ships.ShipSystems.Weapons
{
    public abstract class Weapon : ScriptableObject
    {
        public abstract void Initialize(Ship ship);
        public abstract void UpdateWeapon(WeaponSystem weaponSystem, InputState inputState, float deltaTime);
    }
}
