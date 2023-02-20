using UnityEngine;

namespace Damage
{
    public class DamagableHandler : MonoBehaviour
    {
        [SerializeField] private Damageable _damageableConfig;

        public void TakeDamage(float damage)
        {
            _damageableConfig.TakeDamage(damage);
        }
    }
}
