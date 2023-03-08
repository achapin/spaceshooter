using System;
using UnityEngine;

namespace Damage
{
    [RequireComponent(typeof(DamageableHandler))]
    public class DamageOnCollision : MonoBehaviour
    {
        [SerializeField] private AnimationCurve damageBySpeed;
        
        private DamageableHandler _handler;
        
        void Awake()
        {
            _handler = GetComponent<DamageableHandler>();
        }

        public void OnCollisionEnter(Collision collision)
        {
            _handler.TakeDamage(damageBySpeed.Evaluate(collision.relativeVelocity.magnitude));
        }
    }
}
