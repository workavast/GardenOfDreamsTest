using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCode
{
    public class Inventory
    {
        private const int DefaultSize = 17;

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
        
        public void AddItemInEmptySlot(ItemConfigBase itemConfig, int count = 1)
        {
            int freeSlotIndex = GetEmptySlotIndex();

            if (freeSlotIndex <= -1)
            {
                Debug.LogWarning($"No empty slot. Info: {itemConfig} | {count}");
                return;
            }
            
            _items[freeSlotIndex].SetItem(itemConfig, count);
            
            OnChange?.Invoke();
        }

        public void ChangeItemCount(int slotIndex, int changeValue)
        {
            if (_items[slotIndex].SlotIsEmpty)
            {
                Debug.LogWarning($"You try change empty slot. Info: {slotIndex} | {changeValue}");
                return;
            }
            
            _items[slotIndex].ChangeCurrentValue(changeValue);
            if(_items[slotIndex].CurrentValue <= 0)
                CleanSlot(slotIndex);
            else
                OnChange?.Invoke();
        }

        public void CleanSlot(int slotIndex)
        {
            _items[slotIndex].Clean();
            OnChange?.Invoke();
        }

        /// <returns>
        /// Return index of slot with this itemId and number of items which can be add in stack. <br/>
        /// If inventory dont have this un full item stack, return (-1, -1)
        /// </returns>
        public (int, int) GetSlotDataWithItemFreeStackValue(ItemId itemId)
        {
            for (var slotIndex = 0; slotIndex < _items.Count; slotIndex++)
            {
                var item = _items[slotIndex];
                if (!item.SlotIsEmpty && item.ItemConfig.ItemId == itemId && item.HaveFreeSpace)
                    return (slotIndex, item.FreeSpaceCount);
            }

            return (-1, -1);
        }
        
        public bool HaveEmptySlot()
        {
            foreach (var item in _items)
                if (item.SlotIsEmpty)
                    return true;

            return false;
        }
        
        /// <returns>
        /// Return index of first empty slot. <br/>
        /// If inventory dont have empty slot, return -1
        /// </returns>
        private int GetEmptySlotIndex()
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
        
        //TODO: need add inventory sorting
    }
}