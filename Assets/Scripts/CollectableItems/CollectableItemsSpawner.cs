using GameCode.CollectableItems.Factory;
using GameCode.Enemies;
using UnityEngine;
using Zenject;

namespace GameCode.CollectableItems
{
    public class CollectableItemsSpawner : MonoBehaviour
    {
        [Inject] private readonly CollectableItemsFactory _collectableItemsFactory;
        [Inject] private readonly EnemiesFactory _enemiesFactory;

        private void Awake()
        {
            _enemiesFactory.OnCreate += SubscribeOnEnemyDrop;
        }

        private void SubscribeOnEnemyDrop(EnemyBase enemy)
        {
            enemy.OnDropItem += TrySpawnItem;
        }

        private void TrySpawnItem(ItemId itemId, Vector2 spawnPosition)
        {
            _collectableItemsFactory.Create(itemId, spawnPosition);
        }
    }
}