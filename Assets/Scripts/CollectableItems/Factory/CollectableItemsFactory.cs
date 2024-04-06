using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameCode.CollectableItems.Factory
{
    public class CollectableItemsFactory : MonoBehaviour
    {
        [SerializeField] private CollectableItem collectableItemPrefab;
        
        [Inject] private ItemsConfig _config;
        
        private readonly Dictionary<ItemId, ItemConfigBase> _itemConfigs = new();

        public event Action<CollectableItem> OnCreate;

        private void Awake()
        {
            foreach (var item in _config.Items)
            {
                if (_itemConfigs.ContainsKey(item.ItemId))
                    throw new ArgumentException($"item {item.name} with itemId {item.ItemId} already exist");
                
                _itemConfigs.Add(item.ItemId, item);
            }
        }

        public CollectableItem Create(ItemId id, Vector2 position)
        {
            if (!_itemConfigs.TryGetValue(id, out ItemConfigBase itemConfig))
                throw new Exception($"ItemConfig: {id}, dont present in config {_config}");

            var collectableItem = Instantiate(collectableItemPrefab,  position, Quaternion.identity, transform);
            var itemCount = Random.Range(1, itemConfig.StackMaxValue + 1);
            collectableItem.Init(itemConfig, itemCount);
            
            OnCreate?.Invoke(collectableItem);
            
            return collectableItem;
        }
    }
}