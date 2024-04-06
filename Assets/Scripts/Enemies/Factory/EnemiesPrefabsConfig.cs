using System;
using System.Collections.Generic;
using SerializableDictionaryExtension;
using UnityEngine;

namespace GameCode.Enemies
{
    [CreateAssetMenu(fileName = nameof(EnemiesPrefabsConfig), menuName = "Configs/" + nameof(EnemiesPrefabsConfig))]
    public class EnemiesPrefabsConfig : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<EnemyType, EnemyBase> data;
        
        public IReadOnlyDictionary<EnemyType, EnemyBase> Data => data;
    }
}