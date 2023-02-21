using System;
using UnityEngine;

namespace Damage
{
    [CreateAssetMenu(fileName = "SimpleDestroy", menuName = "Damageable/SimpleDestroy", order = 0)]
    public class SimpleDestroy : Damageable
    {
        [SerializeField] internal float totalHp;

        internal float _currentHp;

        private void OnEnable()
        {
            _currentHp = totalHp;
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

    }
}
