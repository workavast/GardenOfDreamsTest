using System;
using System.Collections.Generic;
using EnumValuesLibrary;
using UnityEngine;
using Zenject;

namespace GameCode.Enemies.Factory
{
    public class EnemiesFactory : MonoBehaviour
    {
        [Inject] private readonly EnemiesPrefabsConfig _config;

        private readonly Dictionary<EnemyType, GameObject> _projectilesParents = new();
        private IReadOnlyDictionary<EnemyType, EnemyBase> ProjectilesData => _config.Data;
        
        public event Action<EnemyBase> OnCreate;
        
        private void Awake()
        {
            foreach (var projectileType in EnumValuesTool.GetValues<EnemyType>())
            {
                GameObject parent = new GameObject(projectileType.ToString()) { transform = { parent = transform } };
                _projectilesParents.Add(projectileType, parent);
            }
        }

        public EnemyBase Create(EnemyType id, Vector2 position)
        {
            if (!ProjectilesData.TryGetValue(id, out EnemyBase prefab))
                throw new Exception($"Enemy: {id}, dont present in config {_config}");
            
            var enemy = Instantiate(prefab,  position, Quaternion.identity, _projectilesParents[id].transform);
            
            OnCreate?.Invoke(enemy);
            
            return enemy;
        }
    }
}