using UnityEngine;

namespace Damage
{
    [DefaultExecutionOrder(-50)] //Has to comme before anything that references damageable, because it won't be set in awake
    public class DamageableHandler : MonoBehaviour
    {
        [SerializeField] private Damageable _damageableConfig;

        private Damageable _damageableInstance;

        public Damageable damageable => _damageableInstance;

        private void Awake()
        {
            _damageableInstance = Instantiate(_damageableConfig);
        }

        public void TakeDamage(float damage)
        {
            damageable.TakeDamage(damage);
        }

        public void OnDestroy()
        {
            Destroy(damageable);
        }
    }
}
