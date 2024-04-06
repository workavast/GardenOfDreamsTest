using System;
using System.Collections.Generic;

namespace GameCode
{
    public class ItemsConfigsConverter
    {
        private readonly Dictionary<ItemId, ItemConfigBase> _itemConfigs = new();

        public IReadOnlyDictionary<ItemId, ItemConfigBase> ItemConfigs => _itemConfigs;
        
        public ItemsConfigsConverter(ItemsConfig config)
        {
            foreach (var item in config.Items)
            {
                if (_itemConfigs.ContainsKey(item.ItemId))
                    throw new ArgumentException($"item {item.name} with itemId {item.ItemId} already exist");
                
                _itemConfigs.Add(item.ItemId, item);
            }
        }
    }
}