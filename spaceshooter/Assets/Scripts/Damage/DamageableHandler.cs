using System;
using UnityEngine;

namespace Damage
{
    [DefaultExecutionOrder(-50)] //Has to comme before anything that references damageable, because it won't be set in awake
    public class DamageableHandler : MonoBehaviour
    {
        [SerializeField] private float hp;

        public event Action<float> DamageTaken;
        public event Action Destroyed;

        public void TakeDamage(float damage)
        {
            DamageTaken?.Invoke(damage);
            hp -= damage;
            if (hp <= 0f)
            {
                Destroyed?.Invoke();
            }
        }
        
    }
}
