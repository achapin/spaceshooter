using System;
using UnityEngine;

namespace Damage
{
    public class DamageableHandler : MonoBehaviour
    {
        public event Action<float, DamageType> DamageTaken;
        public event Action Destroyed;

        public void TakeDamage(float damage, DamageType damageType)
        {
            DamageTaken?.Invoke(damage, damageType);
        }

        public void DestroyDamageable()
        {
            Destroyed?.Invoke();
        }
        
    }
}
