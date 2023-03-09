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
            var speed = collision.relativeVelocity.magnitude;
            var damage = damageBySpeed.Evaluate(speed);
            Debug.Log($"Speed {speed} results in {damage} damage");
            _handler.TakeDamage(damage);
        }
    }
}
