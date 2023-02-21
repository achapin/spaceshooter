using UnityEngine;

namespace Damage
{
    public class DamageableHandler : MonoBehaviour
    {
        [SerializeField] private Damageable _damageableConfig;

        public Damageable damageable => _damageableConfig;

        public void TakeDamage(float damage)
        {
            _damageableConfig.TakeDamage(damage);
        }
    }
}
