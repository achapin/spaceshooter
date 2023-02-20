using System;
using UnityEngine;

namespace Damage
{
    public abstract class Damageable : ScriptableObject
    {
        public abstract void TakeDamage(float damage);

        public event Action<float> DamageTaken;
        public event Action Destroyed;

        protected void OnDamageTaken(float damageTaken)
        {
            DamageTaken?.Invoke(damageTaken);
        }

        protected void OnDestroyed()
        {
            Destroyed?.Invoke();
        }
    }
}
