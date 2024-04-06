using System;
using System.Collections.Generic;
using GameCode.Saves;
using UnityEngine;

namespace GameCode
{
    public class Inventory
    {
        private const int DefaultSize = 15;

        private readonly List<InventorySlot> _items;
        
        public int Size => _items.Count;
        public IReadOnlyList<InventorySlot> Items => _items;

        public event Action OnChange;

        public Inventory()
        {
            _items = new List<InventorySlot>(DefaultSize);
            for (int i = 0; i < DefaultSize; i++)
                _items.Add(new InventorySlot());
        }
        
        public void AddItem(ItemConfigBase itemConfig, int count = 1)
        {
            Debug.Log("try add item");

            int freeSlotIndex = TakeFreeInventorySlot();
            
            Debug.Log($"free slot index {freeSlotIndex}");
            if(freeSlotIndex <= -1)
                return;
            
            _items[freeSlotIndex].SetItem(itemConfig, count);
            
            OnChange?.Invoke();
        }

        public void ChangeItemCount(int slotIndex, int changeValue)
        {
            if (_items[slotIndex].SlotIsEmpty)
            {
                Debug.LogWarning("You try change empty slot");
                return;
            }
            
            _items[slotIndex].ChangeCurrentValue(changeValue);
            if(_items[slotIndex].CurrentValue <= 0)
                DeleteItem(slotIndex);
            else
                OnChange?.Invoke();
        }

        public void DeleteItem(int slotIndex)
        {
            _items[slotIndex].Clean();
            OnChange?.Invoke();
        }

        /// <returns>
        /// Return index of item and number of items which can be add in stack. <br/>
        /// If inventory dont have this un full item stack, return (-1, -1)
        /// </returns>
        public (int, int) ItemFreeStackValue(ItemId itemId)
        {
            for (var slotIndex = 0; slotIndex < _items.Count; slotIndex++)
            {
                var item = _items[slotIndex];
                if (!item.SlotIsEmpty && item.ItemConfig.ItemId == itemId && item.HaveFreeSpace)
                    return (slotIndex, item.FreeSpaceCount);
            }

            return (-1, -1);
        }
        
        public bool HasEmptySlot()
        {
            foreach (var item in _items)
                if (item.SlotIsEmpty)
                    return true;

            return false;
        }
        
        /// <summary>
        /// Return first free slot index. <br/>
        /// If inventory dont have free slot, return -1
        /// </summary>
        /// <returns></returns>
        private int TakeFreeInventorySlot()
        {
            for (int i = 0; i < _items.Count; i++)
                if (_items[i].SlotIsEmpty)
                    return i;

            return -1;
        }

        public void LoadSave(IReadOnlyList<InventorySlot> loadSlots)
        {
            for (int i = 0; i < loadSlots.Count; i++)
            {
                _items[i].Clean();
                if (!loadSlots[i].SlotIsEmpty)
                    _items[i].SetItem(loadSlots[i].ItemConfig, loadSlots[i].CurrentValue);
            }
        }
    }
}