using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameCode.CollectableItems.Factory
{
    public class CollectableItemsFactory : MonoBehaviour
    {
        [SerializeField] private CollectableItem collectableItemPrefab;
        
        [Inject] private readonly ItemsConfigsConverter _itemConfigs;
        
        public event Action<CollectableItem> OnCreate;

        public CollectableItem Create(ItemId id, Vector2 position)
        {
            if (!_itemConfigs.ItemConfigs.TryGetValue(id, out ItemConfigBase itemConfig))
                throw new Exception($"ItemConfig: {id}, dont present in config {_itemConfigs}");

            //need use pool
            var collectableItem = Instantiate(collectableItemPrefab,  position, Quaternion.identity, transform);
            var itemCount = Random.Range(1, itemConfig.StackMaxValue + 1);
            collectableItem.Init(itemConfig, itemCount);
            
            OnCreate?.Invoke(collectableItem);
            
            return collectableItem;
        }
    }
}