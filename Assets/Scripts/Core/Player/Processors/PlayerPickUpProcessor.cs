using GameCode.CollectableItems;
using UnityEngine;

namespace GameCode.Core
{
    public class PlayerPickUpProcessor
    {
        private readonly TriggerZone _itemsPickUpZone;
        private readonly Inventory _inventory;

        public PlayerPickUpProcessor(TriggerZone itemsPickUpZone, Inventory inventory)
        {
            _itemsPickUpZone = itemsPickUpZone;
            _inventory = inventory;
            
            _itemsPickUpZone.OnEnter += OnEnterInItemsPickUpZone;
        }
        
        private void OnEnterInItemsPickUpZone(Collider2D other)
        {
            if (other.TryGetComponent(out CollectableItem item))
            {
                var freeStack = _inventory.ItemFreeStackValue(item.ItemConfig.ItemId);
                
                if (freeStack.Item1 <= -1)
                {
                    if (_inventory.HasEmptySlot())
                    {
                        var itemCount = item.ItemCount;
                        var itemConfig = item.PickUp(item.ItemCount);
                        _inventory.AddItem(itemConfig, itemCount); 
                    }
                }
                else
                {
                    var pickUpCount = Mathf.Clamp(item.ItemCount, 0, freeStack.Item2);
                    var itemConfig = item.PickUp(pickUpCount);
                    _inventory.ChangeItemCount(freeStack.Item1, pickUpCount);

                    if (item.ItemCount > 0 && _inventory.HasEmptySlot())
                    {
                        var itemCount = item.ItemCount;
                        item.PickUp(itemCount);
                        _inventory.AddItem(itemConfig, itemCount);
                    }
                }
            }
        }

    }
}