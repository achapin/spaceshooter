using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{
    [CreateAssetMenu(fileName = "DamageConfig", menuName = "DamageConfig", order = 1)]
    public class DamageConfig : ScriptableObject
    {
        public List<DamageConfigEntry> configEntries;
    }

    [Serializable]
    public struct DamageConfigEntry
    {
        public DamageType damageType;
        public float multiplier;
    }
}
