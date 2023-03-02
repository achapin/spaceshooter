using System;
using UnityEngine;

namespace Damage
{
    public class DamageableHandler : MonoBehaviour
    {
        public event Action<float> DamageTaken;
        public event Action Destroyed;

        public void TakeDamage(float damage)
        {
            DamageTaken?.Invoke(damage);
        }

        public void DestroyDamageable()
        {
            Destroyed?.Invoke();
        }
        
    }
}
