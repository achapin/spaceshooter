using UnityEngine;

namespace Damage
{
    [RequireComponent(typeof(DamageableHandler))]
    public class SimpleDamage : MonoBehaviour
    {

        private DamageableHandler handler;
        private void Awake()
        {
            handler = GetComponent<DamageableHandler>();
            handler.DamageTaken += DamageableOnDamageTaken;
            handler.Destroyed += DamageableOnDestroyed;
        }
        
        private void DamageableOnDamageTaken(float obj)
        {
            Debug.Log($"Damaged for {obj}");
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
