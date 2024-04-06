using System.Collections.Generic;
using System.Linq;
using EnumValuesLibrary;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameCode.Enemies
{
    public class EnemiesSpawner : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private int enemiesCount;
        [SerializeField] private Transform[] spawnPoints;

        [Inject] private readonly EnemiesFactory _factory;

        private int _enemiesCount;
        private EnemyType[] _enemyTypes;

        private void Awake()
        {
            if (enemiesCount > spawnPoints.Length)
                Debug.LogWarning($"Enemies count more than count of spawn points");
            
            _enemiesCount = Mathf.Clamp(enemiesCount, 0, spawnPoints.Length);
            _enemyTypes = EnumValuesTool.GetValues<EnemyType>().ToArray();
        }

        private void Start()
        {
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            List<int> usedSpawnPoints = new List<int>(_enemiesCount);
            for (int i = 0; i < _enemiesCount;)
            {
                var spawnPointIndex = Random.Range(0, spawnPoints.Length);

                if (!usedSpawnPoints.Contains(spawnPointIndex))
                {
                    usedSpawnPoints.Add(spawnPointIndex);
                    var spawnPoint = spawnPoints[spawnPointIndex];
                    var enemyTypeIndex = Random.Range(0, _enemyTypes.Length);
                    var enemyType = _enemyTypes[enemyTypeIndex];
                    _factory.Create(enemyType, spawnPoint.position);
                    
                    i++;
                }
            }
        }
    }
}