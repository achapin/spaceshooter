using System;
using UnityEngine;

namespace Damage
{
    [CreateAssetMenu(menuName = "Create RemoveOnDestroy", fileName = "Damageable/RemoveOnDestroy", order = 0)]
    public class RemoveOnDestroy : Damageable
    {
        [SerializeField] internal float totalHp;

        internal float _currentHp;

        private void OnEnable()
        {
            _currentHp = totalHp;
            Destroyed += OnDestroyed;
        }

        public override void TakeDamage(float damage)
        {
            OnDamageTaken(damage);
            _currentHp -= damage;
            Debug.Log($"Ouch. Took {damage} total damage now {_currentHp}");
            if (_currentHp <= 0f)
            {
                OnDestroyed();
            }
        }
        
        private void OnDestroyed()
        {
            Debug.Log("I Have been destroyed");
            //TODO: Remove gameobject
        }
    }
}
