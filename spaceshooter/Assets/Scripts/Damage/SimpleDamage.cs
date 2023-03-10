using UnityEngine;

namespace Damage
{
    [RequireComponent(typeof(DamageableHandler))]
    public class SimpleDamage : MonoBehaviour
    {
        [SerializeField] private float hp;

        private DamageableHandler handler;
        private void Awake()
        {
            handler = GetComponent<DamageableHandler>();
            handler.DamageTaken += DamageableOnDamageTaken;
            handler.Destroyed += DamageableOnDestroyed;
        }
        
        private void DamageableOnDamageTaken(float damageAmount, DamageType damageType)
        {
            Debug.Log($"Damaged for {damageAmount}");
            hp -= damageAmount;
            if (hp <= 0f)
            {
                handler.DestroyDamageable();
            }
        }

        private void DamageableOnDestroyed()
        {
            Debug.Log($"{gameObject.name} being destroyed");
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            handler.DamageTaken -= DamageableOnDamageTaken;
            handler.Destroyed -= DamageableOnDestroyed;
        }
    }
}
